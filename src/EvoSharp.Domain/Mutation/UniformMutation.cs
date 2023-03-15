using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Mutation;

public class UniformMutation : MutationBase
{
    private readonly int[] _mutableGenesIndexes;

    public UniformMutation(params int[] mutableGenesIndexes) => 
        _mutableGenesIndexes = mutableGenesIndexes;

    protected override void PerformMutate<T>(IChromosome<T> chromosome, float probability)
    {
        ArgumentNullException.ThrowIfNull(nameof(chromosome));

        var genesLength = chromosome.Length;

        foreach (var geneIndex in _mutableGenesIndexes)
        {
            if (geneIndex >= genesLength)
            {
                throw new ArgumentNullException($"The chromosome has no gene on index {geneIndex}. The chromosome genes length is {genesLength}.");
            }

            if (_random.NextSingle() <= probability)
            {
                chromosome[geneIndex] = chromosome.GenerateGene();
            }
        }
    }
}