using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;

public interface ISelection<T>
{
    IList<IChromosome<T>> Selection(int number, Generation<T> generation);
}