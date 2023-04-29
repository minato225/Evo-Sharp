using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoSharp.Domain.Selection
{
    public class RankSelection<T> : SelectionBase<T>
    {
        public RankSelection() : base(2)
        {
        }

        protected static IList<IChromosome<T>> SelectFromWheel(int number, IList<IChromosome<T>> chromosomes, IList<double> rankWheel, Func<double> getPointer)
        {
            var selected = new List<IChromosome<T>>();

            for (int i = 0; i < number; i++)
            {
                var chromosome = rankWheel
                    .Select((value, index) => new { Value = value, Index = index })
                    .FirstOrDefault(x => x.Value >= getPointer());

                if (chromosome != null)
                    selected.Add(chromosomes[chromosome.Index].Clone());
            }

            return selected;
        }

        /// <summary>
        /// Calculates the cumulative percent.
        /// </summary>
        /// <param name="chromosomes">The chromosomes.</param>
        /// <param name="rankWheel">The rank wheel.</param>
        protected static void CalculateCumulativeFitnessRank(IList<IChromosome<T>> chromosomes, IList<double> rankWheel)
        {
            var totalFitness = chromosomes.Count * (chromosomes.Count + 1) / 2;

            var cumulativeRank = 0.0;

            for (int n = chromosomes.Count; n > 0; n--)
            {
                cumulativeRank += (double)n / totalFitness;
                rankWheel.Add(cumulativeRank);
            }
        }

        protected override IList<IChromosome<T>> PerformSelection(int number, IList<IChromosome<T>> chromosomes)
        {
            chromosomes = chromosomes.OrderByDescending(c => c.FitnessValue).ToList();
            var rankWheel = new List<double>();

            CalculateCumulativeFitnessRank(chromosomes, rankWheel);

            return SelectFromWheel(number, chromosomes, rankWheel, () => _random.NextFloat());
        }
    }
}