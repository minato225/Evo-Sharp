using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Crossover;

public abstract class CrossoverBase : ICrossover
{
    protected CrossoverBase(int parentsNumber, int childrenNumber) : this(parentsNumber, childrenNumber, 2) =>
        (ParentsNumber, ChildrenNumber) = (parentsNumber, childrenNumber);
    protected CrossoverBase(int parentsNumber, int childrenNumber, int minChromosomeLength) =>
        (ParentsNumber, ChildrenNumber, MinChromosomeLength) = (parentsNumber, childrenNumber, minChromosomeLength);

    public bool IsOrdered { get; protected set; }
    public int ParentsNumber { get; private set; }
    public int ChildrenNumber { get; private set; }
    public int MinChromosomeLength { get; protected set; }

    public IList<IChromosome<T>> Cross<T>(IList<IChromosome<T>> parents)
    {
        ArgumentNullException.ThrowIfNull(nameof(parents));

        if (parents.Count != ParentsNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(parents), "The number of parents should be the same of ParentsNumber.");
        }

        var firstParent = parents[0];

        if (firstParent.Length < MinChromosomeLength)
        {
            throw new ArgumentNullException($"A chromosome should have, at least, {MinChromosomeLength} genes.");
        }

        return PerformCross(parents);
    }

    protected abstract IList<IChromosome<T>> PerformCross<T>(IList<IChromosome<T>> parents);
}
