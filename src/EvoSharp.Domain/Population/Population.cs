using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Population;

public class Population<T> : IPopulation<T>
{
    public event EventHandler BestChromosomeChanged;

    public Population(int minSize, int maxSize, IChromosome<T> adamChromosome)
    {
        if (minSize < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(minSize), "The minimum size for a population is 2 chromosomes.");
        }

        if (maxSize < minSize)
        {
            throw new ArgumentOutOfRangeException(nameof(maxSize), "The maximum size for a population should be equal or greater than minimum size.");
        }

        //ExceptionHelper.ThrowIfNull(nameof(adamChromosome), adamChromosome);

        MinSize = minSize;
        MaxSize = maxSize;
        AdamChromosome = adamChromosome;
        Generations = new List<Generation<T>>();
    }

    public IList<Generation<T>> Generations { get; protected set; }
    public Generation<T> CurrentGeneration { get; protected set; }
    public int GenerationsNumber { get; protected set; }
    public int MinSize { get; set; }
    public int MaxSize { get; set; }
    public IChromosome<T> BestChromosome { get; protected set; }
    protected IChromosome<T> AdamChromosome { get; set; }

    public virtual void CreateInitialGeneration()
    {
        Generations = new List<Generation<T>>();
        GenerationsNumber = 0;

        var chromosomes = new List<IChromosome<T>>();

        for (int i = 0; i < MinSize; i++)
        {
            var c = AdamChromosome.CreateNew();

            if (c == null)
            {
                throw new InvalidOperationException("The Adam chromosome's 'CreateNew' method generated a null chromosome. This is a invalid behavior, please, check your chromosome code.");
            }

            //c.ValidateGenes();

            chromosomes.Add(c);
        }

        CreateNewGeneration(chromosomes);
    }
    public virtual void CreateNewGeneration(IList<IChromosome<T>> chromosomes)
    {
        ArgumentNullException.ThrowIfNull(nameof(chromosomes));

        CurrentGeneration = new Generation<T>(++GenerationsNumber, chromosomes);
        Generations.Add(CurrentGeneration);
    }     
    public virtual void EndCurrentGeneration()
    {
        CurrentGeneration.End();

        if (BestChromosome is null || BestChromosome.CompareTo(CurrentGeneration.BestChromosome) != 0)
        {
            BestChromosome = CurrentGeneration.BestChromosome;

            OnBestChromosomeChanged(EventArgs.Empty);
        }
    }

    protected virtual void OnBestChromosomeChanged(EventArgs args) => BestChromosomeChanged?.Invoke(this, args);
}