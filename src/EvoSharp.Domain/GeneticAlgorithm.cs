using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Crossover;
using EvoSharp.Domain.Mutation;
using EvoSharp.Domain.Population;
using EvoSharp.Domain.Selection;
using EvoSharp.Domain.Termination;
using System.Diagnostics;

namespace EvoSharp.Domain;

public enum GeneticAlgorithmState
{
    NotStarted,
    Started,

    /// <summary>
    /// The GA has been resumed after a stop or termination reach and is running.
    /// </summary>
    Resumed,

    /// <summary>
    /// The GA has reach the termination condition and is not running.
    /// </summary>
    TerminationReached
}

public sealed class GeneticAlgorithm<T>
{
    public const float BaseCrosProbability = 0.75f;
    public const float BaseMutProbability = 0.1f;
    private Stopwatch _stopwatch;

    public GeneticAlgorithm(
        IPopulation<T> population,
        Func<IChromosome<T>, double> fitness,
        ISelection<T> selection,
        ICrossover crossover,
        IMutation mutation)
    {
        ArgumentNullException.ThrowIfNull(nameof(population));
        ArgumentNullException.ThrowIfNull(nameof(fitness));
        ArgumentNullException.ThrowIfNull(nameof(selection));
        ArgumentNullException.ThrowIfNull(nameof(crossover));
        ArgumentNullException.ThrowIfNull(nameof(mutation));

        Population = population;
        Fitness = fitness;
        Selection = selection;
        Crossover = crossover;
        Mutation = mutation;
        Termination = new GenNumberTermination { MaxGenCount = 50 };

        CrosProbability = BaseCrosProbability;
        MutProbability = BaseMutProbability;
        TotalTime = TimeSpan.Zero;
        State = GeneticAlgorithmState.NotStarted;
    }

    public event EventHandler GenerationRan;
    public event EventHandler TerminationReached;

    public IPopulation<T> Population { get; private set; }
    public Func<IChromosome<T>, double> Fitness { get; private set; }
    public ISelection<T> Selection { get; set; }
    public ICrossover Crossover { get; set; }
    public float CrosProbability { get; set; }
    public IMutation Mutation { get; set; }
    public float MutProbability { get; set; }
    public ITermination Termination { get; set; }
    public TimeSpan TotalTime { get; private set; }

    public GeneticAlgorithmState State { get; private set; }

    public void Start()
    {
        State = GeneticAlgorithmState.Started;
        _stopwatch = Stopwatch.StartNew();

        Population.InitGeneration();

        _stopwatch.Stop();
        TotalTime = _stopwatch.Elapsed;

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
            if (Population.GenerationsNumber == 0)
            {
                throw new InvalidOperationException("Attempt to resume a genetic algorithm which was not yet started.");
            }

            if (Termination.HasReached(this))
            {
                throw new InvalidOperationException("Attempt to resume a genetic algorithm with a termination already reached. Please, specify a new termination or extend the current one.");
            }

            State = GeneticAlgorithmState.Resumed;

            if (EndCurrentGeneration()) return;

            bool terminationConditionReached = false;

            do
            {
                _stopwatch.Restart();
                terminationConditionReached = EvolveOneGeneration();
                _stopwatch.Stop();

                TotalTime += _stopwatch.Elapsed;
            }
            while (!terminationConditionReached);
        }
        catch
        {
            throw;
        }
    }

    private bool EvolveOneGeneration()
    {
        var parents = SelectParents();
        var offspring = Cross(parents);
        Mutate(offspring);

        Population.CreateNewGeneration(offspring);
        return EndCurrentGeneration();
    }

    private bool EndCurrentGeneration()
    {
        EvaluateFitness();
        Population.EndCurrentGeneration();

        GenerationRan?.Invoke(this, EventArgs.Empty);

        if (Termination.HasReached(this))
        {
            State = GeneticAlgorithmState.TerminationReached;

            TerminationReached?.Invoke(this, EventArgs.Empty);

            return true;
        }

        return false;
    }

    private void EvaluateFitness()
    {
        var chromosomesWithoutFitness = Population.CurrentGeneration.Chromosomes.Where(c => !c.Fitness.HasValue).ToList();

        foreach (var c in chromosomesWithoutFitness)
        {
            RunEvaluateFitness(c);
        }

        Population.CurrentGeneration.Chromosomes = Population.CurrentGeneration.Chromosomes.OrderByDescending(c => c.Fitness.Value).ToList();
    }

    private void RunEvaluateFitness(IChromosome<T> chromosome)
    {
        try
        {
            chromosome.Fitness = Fitness.Invoke(chromosome);
        }
        catch (Exception ex)
        {
            throw new ArgumentNullException($"Error executing Fitness.Evaluate for chromosome: {ex.Message}");
        }
    }

    private IList<IChromosome<T>> SelectParents() =>
        Selection.SelectChromosomes(Population.MinSize, Population.CurrentGeneration);

    private IList<IChromosome<T>> Cross(IList<IChromosome<T>> parents)
    {
        var rnd = new Random();
        var minSize = Population.MinSize;
        var offspring = new List<IChromosome<T>>(minSize);

        for (var i = 0; i < minSize; i += Crossover.ParentsNumber)
        {
            var selectedParents = parents.Skip(i).Take(Crossover.ParentsNumber).ToList();
            if (selectedParents.Count == Crossover.ParentsNumber && rnd.NextSingle() <= CrosProbability)
            {
                var children = Crossover.Cross(selectedParents);
                offspring.AddRange(children);
            }
        }

        return offspring;
    }

    private void Mutate(IList<IChromosome<T>> chromosomes)
    {
        foreach (var chromosome in chromosomes)
        {
            Mutation.Mutate(chromosome, MutProbability);
        }
    }
}
