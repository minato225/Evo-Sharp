using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoSharp.Domain.Selection
{
    public class TournamentSelection<T> : SelectionBase<T>
    {
        public TournamentSelection(int size) : base(2) =>
            TrourSize = size;

        public int TrourSize { get; set; }

        protected override IList<IChromosome<T>> PerformSelection(int number, IList<IChromosome<T>> chromosomes)
        {
            if (TrourSize > chromosomes.Count)
            {
                throw new ArgumentException("The tournament size is greater than available chromosomes.");
            }

            var candidates = chromosomes.ToList();
            var selected = new List<IChromosome<T>>();

            while (selected.Count < number)
            {
                var randomIndexes = RandomUtils.GetUniqueInts(TrourSize, 0, candidates.Count);
                var tournamentWinner = candidates.Where((c, i) => randomIndexes.Contains(i)).OrderByDescending(c => c.FitnessValue).First();

                selected.Add(tournamentWinner.Clone());
                candidates.Remove(tournamentWinner);
            }

            return selected;
        }
    }
}