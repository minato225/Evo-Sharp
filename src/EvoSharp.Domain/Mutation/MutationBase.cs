using EvoSharp.Domain.Chromosome;
using System;

namespace EvoSharp.Domain.Mutation
{
    public abstract class MutationBase : IMutation
    {
        protected readonly Random _random = new Random();

        public void Mutate<T>(IChromosome<T> chromosome, float probability)
        {
            if (chromosome == null)
            {
                throw new ArgumentNullException(nameof(chromosome));
            }

            PerformMutate(chromosome, probability);
        }

        protected abstract void PerformMutate<T>(IChromosome<T> chromosome, float probability);
    }
}