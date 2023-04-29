using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Utils;
using System;

namespace EvoSharp.Domain.Mutation
{
    public class UniformMutation : MutationBase
    {
        private readonly int[] _mutableGenesIndexes;

        public UniformMutation(params int[] mutableGenesIndexes) =>
            _mutableGenesIndexes = mutableGenesIndexes;

        protected override void PerformMutate<T>(IChromosome<T> chromosome, float probability)
        {
            foreach (var geneIndex in _mutableGenesIndexes)
            {
                if (geneIndex >= chromosome.Length)
                {
                    throw new ArgumentOutOfRangeException($"The chromosome has no gene on index {geneIndex}. The chromosome genes length is {chromosome.Length}.");
                }

                if (_random.NextFloat() <= probability)
                {
                    chromosome[geneIndex] = chromosome.GenerateGene();
                }
            }
        }
    }
}