using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Mutation;

public interface IMutation : IChromosomeOperator
{
    void Mutate<T>(IChromosome<T> chromosome, float probability);
}
