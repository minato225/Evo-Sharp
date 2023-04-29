using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Utils;
using System;

namespace EvoSharp.Domain.Mutation
{
    public class FlipBitMutation : MutationBase
    {
        protected override void PerformMutate<T>(IChromosome<T> chromosome, float probability)
        {
            if (!(chromosome is BinaryChromosome binaryChromosome))
            {
                throw new ArgumentException("Needs a binary chromosome that implements IBinaryChromosome.");
            }

            if (_random.NextFloat() <= probability)
            {
                var index = _random.Next(0, chromosome.Length);
                binaryChromosome.FlipGene(index);
            }
        }
    }
}