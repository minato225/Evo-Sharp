using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Selection;

public class EliteSelection<T> : SelectionBase<T>
{
    private readonly int _previousGenerationChromosomesNumber;

    private List<IChromosome<T>> _previousGenerationChromosomes;

    public EliteSelection(int previousGenerationChromosomesNumber)
        : base(2)
    {
        _previousGenerationChromosomesNumber = previousGenerationChromosomesNumber;
    }

    protected override IList<IChromosome<T>> PerformSelection(int number, IList<IChromosome<T>> chromosomes)
    {
        if (_previousGenerationChromosomesNumber == 0)
            _previousGenerationChromosomes = new List<IChromosome<T>>();

        _previousGenerationChromosomes.AddRange(chromosomes);

        var ordered = _previousGenerationChromosomes.OrderByDescending(c => c.FitnessValue);
        var result = ordered.Take(number).ToList();

        _previousGenerationChromosomes = result.Take(_previousGenerationChromosomesNumber).ToList();

        return result;
    }
}