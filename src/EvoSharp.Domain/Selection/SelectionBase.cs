using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;
public abstract class SelectionBase<T> : ISelection<T>
{
    readonly int m_minNumberChromosomes;
    protected readonly Random _random = new();

    protected SelectionBase(int minNumberChromosomes) => m_minNumberChromosomes = minNumberChromosomes;

    public IList<IChromosome<T>> SelectChromosomes(int number, Generation<T> generation)
    {
        if (number < m_minNumberChromosomes)
        {
            throw new ArgumentOutOfRangeException(nameof(number), $"The number of selected chromosomes should be at least {m_minNumberChromosomes}.");
        }

        ArgumentNullException.ThrowIfNull(nameof(generation));

        if (generation.Chromosomes.Any(c => !c.Fitness.HasValue))
        {
            throw new ArgumentException("There are chromosomes with null fitness.");
        }

        return PerformSelectChromosomes(number, generation);
    }

    protected abstract IList<IChromosome<T>> PerformSelectChromosomes(int number, Generation<T> generation);
}