using EvoSharp.Domain.Chromosome;
using System.Collections.Generic;

namespace EvoSharp.Domain.Crossover
{
    public interface ICrossover
    {
        int ParentCount { get; }
        int MinLength { get; }

        IList<IChromosome<T>> Cross<T>(IList<IChromosome<T>> parents);
    }
}