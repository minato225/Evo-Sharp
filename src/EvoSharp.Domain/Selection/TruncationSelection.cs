using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;

public sealed class TruncationSelection : SelectionBase
{
    public TruncationSelection() : base(2)
    {
    }

    protected override IList<IChromosome<T>> PerformSelectChromosomes<T>(int number, Generation<T> generation) =>
        generation.Chromosomes.OrderByDescending(c => c.Fitness).Take(number).ToList();
}