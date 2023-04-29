using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Utils;
using System;
using System.Linq;

namespace EvoSharp.Domain.Mutation
{
    public class ReverseSequenceMutation : MutationBase
    {
        protected override void PerformMutate<T>(IChromosome<T> chromosome, float probability)
        {
            if (chromosome.Length < 3)
            {
                throw new ArgumentException("A chromosome should have, at least, 3 genes.");
            }

            if (_random.NextFloat() > probability) return;

            var (firstIndex, secondIndex) = (_random.Next(chromosome.Length), _random.Next(chromosome.Length));
            (firstIndex, secondIndex) = (Math.Min(firstIndex, secondIndex), Math.Max(firstIndex, secondIndex));

            var mutatedSequence = chromosome.Genes[firstIndex..secondIndex].Reverse().ToArray();

            chromosome.ReplaceGenes(firstIndex, mutatedSequence);
        }
    }
}