using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Crossover;

public interface ICrossover : IChromosomeOperator
{
    int ParentCount { get; }
    int ChildCount { get; }
    int MinLength { get; }

    IList<IChromosome<T>> Cross<T>(IList<IChromosome<T>> parents);
}