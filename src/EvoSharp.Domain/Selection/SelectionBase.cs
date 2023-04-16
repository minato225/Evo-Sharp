using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Selection;
public abstract class SelectionBase<T> : ISelection<T>
{
    readonly int _minCountChromosomes;
    protected readonly Random _random = new();

    protected SelectionBase(int minNumberChromosomes) => _minCountChromosomes = minNumberChromosomes;

    public IList<IChromosome<T>> Selection(int minSize, IList<IChromosome<T>> chromosomes)
    {
        if (minSize < _minCountChromosomes)
        {
            throw new ArgumentOutOfRangeException(nameof(minSize), $"The number of selected chromosomes should be at least {_minCountChromosomes}.");
        }

        ArgumentNullException.ThrowIfNull(chromosomes, nameof(chromosomes));

        if (chromosomes.Any(c => !c.FitnessValue.HasValue))
        {
            throw new ArgumentException("There are chromosomes with null fitness.");
        }

        return PerformSelection(minSize, chromosomes);
    }

    protected abstract IList<IChromosome<T>> PerformSelection(int number, IList<IChromosome<T>> chromosomes);
}