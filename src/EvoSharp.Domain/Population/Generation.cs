using EvoSharp.Domain.Chromosome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoSharp.Domain.Population
{
    public sealed class Generation<T>
    {
        public Generation(int generationNumber, IList<IChromosome<T>> chromosomes)
        {
            if (generationNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(generationNumber),
                    $"Generation number {generationNumber} is invalid. Generation number should be positive and start in 1.");
            }

            if (chromosomes is null || chromosomes.Count < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(chromosomes), "A generation should have at least 2 chromosomes.");
            }

            GenerationNumber = generationNumber;
            Chromosomes = chromosomes;
        }

        public int GenerationNumber { get; private set; }
        public IList<IChromosome<T>> Chromosomes { get; internal set; }
        public IChromosome<T> BestChromosome { get; internal set; }

        public void EndGeneration()
        {
            Chromosomes = Chromosomes
                .Where(c => c.FitnessValue.HasValue)
                .OrderByDescending(c => c.FitnessValue.Value)
                .ToList();

            BestChromosome = Chromosomes.First();
        }
    }
}