using EvoSharp.Domain.Chromosome;
using System.Collections.Generic;

namespace EvoSharp.Domain.Selection
{
    public interface ISelection<T>
    {
        IList<IChromosome<T>> Selection(int minSize, IList<IChromosome<T>> chromosomes);
    }
}