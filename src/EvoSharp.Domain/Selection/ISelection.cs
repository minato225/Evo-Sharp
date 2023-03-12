using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;

public interface ISelection
{
    IList<IChromosome<T>> SelectChromosomes<T>(int number, Generation<T> generation);
}