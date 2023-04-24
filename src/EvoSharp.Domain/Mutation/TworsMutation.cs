using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Mutation;

public class TworsMutation : MutationBase
{
    protected override void PerformMutate<T>(IChromosome<T> chromosome, float probability)
    {
        if (_random.NextSingle() > probability) return;

        var (firstIndex, secondIndex) = (_random.Next(chromosome.Length), _random.Next(chromosome.Length));
        var (firstGene, secondGene) = (chromosome[firstIndex], chromosome[secondIndex]);

        chromosome[firstIndex] = secondGene;
        chromosome[secondIndex] = firstGene;
    }
}
