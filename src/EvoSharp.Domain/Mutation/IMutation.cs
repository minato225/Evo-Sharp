using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Mutation;

public interface IMutation
{
    void Mutate<T>(IChromosome<T> chromosome, float probability);
}
