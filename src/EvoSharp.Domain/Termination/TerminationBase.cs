namespace EvoSharp.Domain.Termination;

public abstract class TerminationBase : ITermination
{
    private bool m_hasReached;

    public bool HasReached<T>(GeneticAlgorithm<T> geneticAlgorithm)
    {
        ArgumentNullException.ThrowIfNull(nameof(geneticAlgorithm));

        m_hasReached = PerformHasReached(geneticAlgorithm);

        return m_hasReached;
    }
    public override string ToString() => $"{GetType().Name} (HasReached: {m_hasReached})";

    protected abstract bool PerformHasReached<T>(GeneticAlgorithm<T> geneticAlgorithm);
}
