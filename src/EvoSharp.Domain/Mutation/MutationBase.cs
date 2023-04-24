using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Mutation;

public abstract class MutationBase : IMutation
{
    protected readonly Random _random = new();

    public void Mutate<T>(IChromosome<T> chromosome, float probability)
    {
        ArgumentNullException.ThrowIfNull(chromosome, nameof(chromosome));

        PerformMutate(chromosome, probability);
    }

    protected abstract void PerformMutate<T>(IChromosome<T> chromosome, float probability);
}
