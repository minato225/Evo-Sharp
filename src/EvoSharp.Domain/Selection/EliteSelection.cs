using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;

public sealed class EliteSelection<T> : SelectionBase<T>
{
    readonly int _previousGenerationChromosomesNumber;
    List<IChromosome<T>> _previousGenerationChromosomes;

    public EliteSelection(int previousGenerationChromosomesNumber)
        : base(2)
    {
        _previousGenerationChromosomesNumber = previousGenerationChromosomesNumber;
    }

    protected override IList<IChromosome<T>> PerformSelectChromosomes(int number, Generation<T> generation)
    {
        if (generation.Number == 1)
            _previousGenerationChromosomes = new List<IChromosome<T>>();

        _previousGenerationChromosomes.AddRange(generation.Chromosomes);

        var ordered = _previousGenerationChromosomes.OrderByDescending(c => c.Fitness);
        var result = ordered.Take(number).ToList();

        _previousGenerationChromosomes = result.Take(_previousGenerationChromosomesNumber).ToList();

        return result;
    }
}