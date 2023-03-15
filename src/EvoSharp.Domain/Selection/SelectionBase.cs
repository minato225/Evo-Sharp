using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;
public abstract class SelectionBase<T> : ISelection<T>
{
    readonly int _minCountChromosomes;
    protected readonly Random _random = new();

    protected SelectionBase(int minNumberChromosomes) => _minCountChromosomes = minNumberChromosomes;

    public IList<IChromosome<T>> Selection(int number, Generation<T> generation)
    {
        if (number < _minCountChromosomes)
        {
            throw new ArgumentOutOfRangeException(nameof(number), $"The number of selected chromosomes should be at least {_minCountChromosomes}.");
        }

        ArgumentNullException.ThrowIfNull(nameof(generation));

        if (generation.Chromosomes.Any(c => !c.FitnessValue.HasValue))
        {
            throw new ArgumentException("There are chromosomes with null fitness.");
        }

        return PerformSelection(number, generation);
    }

    protected abstract IList<IChromosome<T>> PerformSelection(int number, Generation<T> generation);
}