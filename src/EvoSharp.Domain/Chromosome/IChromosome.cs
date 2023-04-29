using System;

namespace EvoSharp.Domain.Chromosome
{
    public interface IChromosome<T> : IComparable<IChromosome<T>>
    {
        double? FitnessValue { get; set; }
        int Length { get; }
        T this[int index] { get; set; }
        T[] Genes { get; }

        T GenerateGene();
        void ReplaceGenes(int startIndex, T[] genes);
        IChromosome<T> CreateNew();
        IChromosome<T> Clone();
    }
}