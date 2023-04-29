using EvoSharp.Domain.Chromosome;
using System.Collections.Generic;
using System.Linq;

namespace EvoSharp.Domain.Selection
{
    public class TruncationSelection<T> : SelectionBase<T>
    {
        public TruncationSelection() : base(2)
        {
        }

        protected override IList<IChromosome<T>> PerformSelection(int number, IList<IChromosome<T>> chromosomes) =>
            chromosomes.OrderByDescending(c => c.FitnessValue).Take(number).ToList();
    }
}