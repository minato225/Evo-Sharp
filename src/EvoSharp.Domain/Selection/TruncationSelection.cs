using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;

public sealed class TruncationSelection<T> : SelectionBase<T>
{
    public TruncationSelection() : base(2)
    {
    }

    protected override IList<IChromosome<T>> PerformSelection(int number, Generation<T> generation) =>
        generation.Chromosomes.OrderByDescending(c => c.FitnessValue).Take(number).ToList();
}