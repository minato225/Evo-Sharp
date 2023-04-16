using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Selection;

public interface ISelection<T>
{
    IList<IChromosome<T>> Selection(int minSize, IList<IChromosome<T>> chromosomes);
}