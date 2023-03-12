using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Crossover;

public interface ICrossover : IChromosomeOperator
{
    int ParentsNumber { get; }
    int ChildrenNumber { get; }
    int MinChromosomeLength { get; }

    IList<IChromosome<T>> Cross<T>(IList<IChromosome<T>> parents);
}