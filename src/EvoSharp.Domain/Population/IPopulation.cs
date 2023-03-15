using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Population;

public interface IPopulation<T>
{
    event EventHandler BestChromosomeChanged;

    IList<Generation<T>> Generations { get; }
    Generation<T> CurrentGeneration { get; } 
    int GenerationsNumber { get; }
    int MinSize { get; set; }
    int MaxSize { get; set; }
    IChromosome<T> BestChromosome { get; }

    void InitGeneration();
    void CreateNewGeneration(IList<IChromosome<T>> chromosomes);
    void EndCurrentGeneration();
}