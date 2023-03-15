using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Crossover;

public abstract class CrossoverBase : ICrossover
{
    protected CrossoverBase(int parentCount, int childCount) : this(parentCount, childCount, 2) =>
        (ParentCount, ChildCount) = (parentCount, childCount);
    protected CrossoverBase(int parentCount, int childCount, int minLength) =>
        (ParentCount, ChildCount, MinLength) = (parentCount, childCount, minLength);

    public bool IsOrdered { get; protected set; }
    public int ParentCount { get; private set; }
    public int ChildCount { get; private set; }
    public int MinLength { get; protected set; }

    public IList<IChromosome<T>> Cross<T>(IList<IChromosome<T>> parents)
    {
        ArgumentNullException.ThrowIfNull(nameof(parents));

        if (parents.Count != ParentCount)
        {
            throw new ArgumentOutOfRangeException(nameof(parents), "The number of parents should be the same of ParentsNumber.");
        }

        var firstParent = parents[0];

        if (firstParent.Length < MinLength)
        {
            throw new ArgumentNullException($"A chromosome should have, at least, {MinLength} genes.");
        }

        return PerformCross(parents);
    }

    protected abstract IList<IChromosome<T>> PerformCross<T>(IList<IChromosome<T>> parents);
}
