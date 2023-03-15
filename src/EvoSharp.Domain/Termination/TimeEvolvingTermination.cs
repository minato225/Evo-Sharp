namespace EvoSharp.Domain.Termination;

public class TimeEvolvingTermination : TerminationBase
{
    public required TimeSpan MaxTime { get; set; }

    protected override bool PerformHasReached<T>(GeneticAlgorithm<T> geneticAlgorithm) =>
        geneticAlgorithm.TimeEvolving >= MaxTime;
}
