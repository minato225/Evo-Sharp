using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Crossover;

public abstract class CrossoverBase : ICrossover
{
    protected CrossoverBase(int parentCount) : this(parentCount, 2) =>
        ParentCount = parentCount;
    protected CrossoverBase(int parentCount, int minLength) =>
        (ParentCount, MinLength) = (parentCount, minLength);

    public int ParentCount { get; private set; }
    public int MinLength { get; protected set; }

    public IList<IChromosome<T>> Cross<T>(IList<IChromosome<T>> parents)
    {
        ArgumentNullException.ThrowIfNull(parents, nameof(parents));

        if (parents.Count != ParentCount)
            throw new ArgumentOutOfRangeException(nameof(parents), "The number of parents should be the same of ParentsNumber.");

        if (parents[0].Length < MinLength)
            throw new ArgumentException($"A chromosome should have, at least, {MinLength} genes.");

        return PerformCross(parents);
    }

    protected abstract IList<IChromosome<T>> PerformCross<T>(IList<IChromosome<T>> parents);
}
