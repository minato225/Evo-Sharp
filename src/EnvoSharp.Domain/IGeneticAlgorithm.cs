using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain;

public interface IGeneticAlgorithm<T>
{
    int GenerationsNumber { get; }

    IChromosome<T> BestChromosome { get; }

    TimeSpan TimeEvolving { get; }
}
