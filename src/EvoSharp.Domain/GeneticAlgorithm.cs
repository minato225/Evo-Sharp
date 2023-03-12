using EnvoSharp.Domain.Fitnesses;
using EnvoSharp.Domain.Reinsertions;
using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Crossover;
using EvoSharp.Domain.Fitness;
using EvoSharp.Domain.Mutation;
using EvoSharp.Domain.OperatorsStrategy;
using EvoSharp.Domain.Population;
using EvoSharp.Domain.Reinsertions;
using EvoSharp.Domain.Selection;
using EvoSharp.Domain.Termination;
using System.Diagnostics;

namespace EvoSharp.Domain;

public enum GeneticAlgorithmState
{
    NotStarted,
    Started,
    Stopped,

    /// <summary>
    /// The GA has been resumed after a stop or termination reach and is running.
    /// </summary>
    Resumed,

    /// <summary>
    /// The GA has reach the termination condition and is not running.
    /// </summary>
    TerminationReached
}

public sealed class GeneticAlgorithm<T> : IGeneticAlgorithm<T>
{
    public const float DefaultCrossoverProbability = 0.75f;
    public const float DefaultMutationProbability = 0.1f;

    private bool m_stopRequested;
    private readonly object m_lock = new();
    private GeneticAlgorithmState m_state;
    private Stopwatch m_stopwatch;

    #region Constructors
    /// <summary>
    /// Initializes a new instance of the <see cref="GeneticAlgorithm"/> class.
    /// </summary>
    /// <param name="population">The chromosomes population.</param>
    /// <param name="fitness">The fitness evaluation function.</param>
    /// <param name="selection">The selection operator.</param>
    /// <param name="crossover">The crossover operator.</param>
    /// <param name="mutation">The mutation operator.</param>
    public GeneticAlgorithm(
                      IPopulation population,
                      IFitness fitness,
                      ISelection selection,
                      ICrossover crossover,
                      IMutation mutation)
    {
        ExceptionHelper.ThrowIfNull("population", population);
        ExceptionHelper.ThrowIfNull("fitness", fitness);
        ExceptionHelper.ThrowIfNull("selection", selection);
        ExceptionHelper.ThrowIfNull("crossover", crossover);
        ExceptionHelper.ThrowIfNull("mutation", mutation);

        Population = population;
        Fitness = fitness;
        Selection = selection;
        Crossover = crossover;
        Mutation = mutation;
        Reinsertion = new ElitistReinsertion();
        Termination = new GenerationNumberTermination(1);

        CrossoverProbability = DefaultCrossoverProbability;
        MutationProbability = DefaultMutationProbability;
        TimeEvolving = TimeSpan.Zero;
        State = GeneticAlgorithmState.NotStarted;
        TaskExecutor = new LinearTaskExecutor();
        OperatorsStrategy = new DefaultOperatorsStrategy();
    }
    #endregion

    #region Events
    /// <summary>
    /// Occurs when generation ran.
    /// </summary>
    public event EventHandler GenerationRan;

    /// <summary>
    /// Occurs when termination reached.
    /// </summary>
    public event EventHandler TerminationReached;

    /// <summary>
    /// Occurs when stopped.
    /// </summary>
    public event EventHandler Stopped;
    #endregion

    #region Properties
    /// <summary>
    /// Gets the operators strategy
    /// </summary>
    public IOperatorsStrategy OperatorsStrategy { get; set; }

    /// <summary>
    /// Gets the population.
    /// </summary>
    /// <value>The population.</value>
    public IPopulation<T> Population { get; private set; }

    /// <summary>
    /// Gets the fitness function.
    /// </summary>
    public IFitness<T> Fitness { get; private set; }

    /// <summary>
    /// Gets or sets the selection operator.
    /// </summary>
    public ISelection Selection { get; set; }

    /// <summary>
    /// Gets or sets the crossover operator.
    /// </summary>
    /// <value>The crossover.</value>
    public ICrossover Crossover { get; set; }

    /// <summary>
    /// Gets or sets the crossover probability.
    /// </summary>
    public float CrossoverProbability { get; set; }

    /// <summary>
    /// Gets or sets the mutation operator.
    /// </summary>
    public IMutation Mutation { get; set; }

    /// <summary>
    /// Gets or sets the mutation probability.
    /// </summary>
    public float MutationProbability { get; set; }

    /// <summary>
    /// Gets or sets the termination condition.
    /// </summary>
    public ITermination Termination { get; set; }

    /// <summary>
    /// Gets the generations number.
    /// </summary>
    /// <value>The generations number.</value>
    public int GenerationsNumber => Population.GenerationsNumber;

    /// <summary>
    /// Gets the best chromosome.
    /// </summary>
    /// <value>The best chromosome.</value>
    public IChromosome<T> BestChromosome => Population.BestChromosome;

    /// <summary>
    /// Gets the time evolving.
    /// </summary>
    public TimeSpan TimeEvolving { get; private set; }

    /// <summary>
    /// Gets the state.
    /// </summary>
    public GeneticAlgorithmState State
    {
        get => m_state;
        private set
        {
            var shouldStop = Stopped != null && m_state != value && value == GeneticAlgorithmState.Stopped;

            m_state = value;

            if (shouldStop)
                Stopped.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Gets a value indicating whether this instance is running.
    /// </summary>
    /// <value><c>true</c> if this instance is running; otherwise, <c>false</c>.</value>
    public bool IsRunning => State == GeneticAlgorithmState.Started || State == GeneticAlgorithmState.Resumed;

    /// <summary>
    /// Gets or sets the task executor which will be used to execute fitness evaluation.
    /// </summary>
    //public ITaskExecutor TaskExecutor { get; set; }
    #endregion

    #region Methods

    /// <summary>
    /// Starts the genetic algorithm using population, fitness, selection, crossover, mutation and termination configured.
    /// </summary>
    public void Start()
    {
        lock (m_lock)
        {
            State = GeneticAlgorithmState.Started;
            m_stopwatch = Stopwatch.StartNew();
            Population.CreateInitialGeneration();
            m_stopwatch.Stop();
            TimeEvolving = m_stopwatch.Elapsed;
        }

        Resume();
    }

    /// <summary>
    /// Resumes the last evolution of the genetic algorithm.
    /// <remarks>
    /// If genetic algorithm was not explicit Stop (calling Stop method), you will need provide a new extended Termination.
    /// </remarks>
    /// </summary>
    public void Resume()
    {
        try
        {
            lock (m_lock)
            {
                m_stopRequested = false;
            }

            if (Population.GenerationsNumber == 0)
            {
                throw new InvalidOperationException("Attempt to resume a genetic algorithm which was not yet started.");
            }

            if (Population.GenerationsNumber > 1)
            {
                if (Termination.HasReached(this))
                {
                    throw new InvalidOperationException("Attempt to resume a genetic algorithm with a termination ({0}) already reached. Please, specify a new termination or extend the current one.".With(Termination));
                }

                State = GeneticAlgorithmState.Resumed;
            }

            if (EndCurrentGeneration())
            {
                return;
            }

            bool terminationConditionReached = false;

            do
            {
                if (m_stopRequested)
                {
                    break;
                }

                m_stopwatch.Restart();
                terminationConditionReached = EvolveOneGeneration();
                m_stopwatch.Stop();
                TimeEvolving += m_stopwatch.Elapsed;
            }
            while (!terminationConditionReached);
        }
        catch
        {
            State = GeneticAlgorithmState.Stopped;
            throw;
        }
    }

    /// <summary>
    /// Stops the genetic algorithm..
    /// </summary>
    public void Stop()
    {
        if (Population.GenerationsNumber == 0)
        {
            throw new InvalidOperationException("Attempt to stop a genetic algorithm which was not yet started.");
        }

        lock (m_lock)
        {
            m_stopRequested = true;
        }
    }

    /// <summary>
    /// Evolve one generation.
    /// </summary>
    /// <returns>True if termination has been reached, otherwise false.</returns>
    private bool EvolveOneGeneration()
    {
        var parents = SelectParents();
        var offspring = Cross(parents);
        Mutate(offspring);
        var newGenerationChromosomes = Reinsert(offspring, parents);
        Population.CreateNewGeneration(newGenerationChromosomes);
        return EndCurrentGeneration();
    }

    /// <summary>
    /// Ends the current generation.
    /// </summary>
    /// <returns><c>true</c>, if current generation was ended, <c>false</c> otherwise.</returns>
    private bool EndCurrentGeneration()
    {
        EvaluateFitness();
        Population.EndCurrentGeneration();

        var handler = GenerationRan;
        handler?.Invoke(this, EventArgs.Empty);

        if (Termination.HasReached(this))
        {
            State = GeneticAlgorithmState.TerminationReached;

            handler = TerminationReached;
            handler?.Invoke(this, EventArgs.Empty);

            return true;
        }

        if (m_stopRequested)
        {
            TaskExecutor.Stop();
            State = GeneticAlgorithmState.Stopped;
        }

        return false;
    }

    /// <summary>
    /// Evaluates the fitness.
    /// </summary>
    private void EvaluateFitness()
    {
        try
        {
            var chromosomesWithoutFitness = Population.CurrentGeneration.Chromosomes.Where(c => !c.Fitness.HasValue).ToList();

            for (int i = 0; i < chromosomesWithoutFitness.Count; i++)
            {
                var c = chromosomesWithoutFitness[i];

                TaskExecutor.Add(() =>
                {
                    RunEvaluateFitness(c);
                });
            }

            if (!TaskExecutor.Start())
            {
                throw new TimeoutException("The fitness evaluation reached the {0} timeout.".With(TaskExecutor.Timeout));
            }
        }
        finally
        {
            TaskExecutor.Stop();
            TaskExecutor.Clear();
        }

        Population.CurrentGeneration.Chromosomes = Population.CurrentGeneration.Chromosomes.OrderByDescending(c => c.Fitness.Value).ToList();
    }

    /// <summary>
    /// Runs the evaluate fitness.
    /// </summary>
    /// <param name="chromosome">The chromosome.</param>
    private void RunEvaluateFitness(object chromosome)
    {
        try
        {
            if (chromosome is IChromosome<T> c)
            {
                c.Fitness = Fitness.Evaluate(c);
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException($"Error executing Fitness.Evaluate for chromosome: {ex.Message}");
        }
    }

    /// <summary>
    /// Selects the parents.
    /// </summary>
    /// <returns>The parents.</returns>
    private IList<IChromosome<T>> SelectParents() =>
        Selection.SelectChromosomes(Population.MinSize, Population.CurrentGeneration);

    /// <summary>
    /// Crosses the specified parents.
    /// </summary>
    /// <param name="parents">The parents.</param>
    /// <returns>The result chromosomes.</returns>
    private IList<IChromosome<T>> Cross(IList<IChromosome<T>> parents) =>
        OperatorsStrategy.Cross(Population, Crossover, CrossoverProbability, parents);

    /// <summary>
    /// Mutate the specified chromosomes.
    /// </summary>
    /// <param name="chromosomes">The chromosomes.</param>
    private void Mutate(IList<IChromosome<T>> chromosomes) =>
        OperatorsStrategy.Mutate(Mutation, MutationProbability, chromosomes);
    #endregion
}
