namespace EvoSharp.Domain.Chromosome;

public interface IBinaryChromosome<T> : IChromosome<T>
{
    void FlipGene(int index);
}

