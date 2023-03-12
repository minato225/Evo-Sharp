namespace EvoSharp.Domain.Chromosome;

public interface IChromosome<T> : IComparable<IChromosome<T>>
{
    double? Fitness { get; set; }
    int Length { get; }

    T this[int index] { get; set; }

    T GenerateGene();
    void ReplaceGenes(int startIndex, T[] genes);

    void Resize(int newLength);
    T[] Genes { get; }
    IChromosome<T> CreateNew();
    IChromosome<T> Clone();
}