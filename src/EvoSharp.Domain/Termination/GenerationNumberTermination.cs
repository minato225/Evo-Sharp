namespace EvoSharp.Domain.Termination;

/// <summary>
/// Generation number termination.
/// <remarks>
/// The genetic algorithm will be terminate when reach the expected generation number.
/// </remarks>
/// </summary>
public class GenerationNumberTermination : TerminationBase
{
    public required int ExpectedGenerationNumber { get; set; }
    protected override bool PerformHasReached<T>(GeneticAlgorithm<T> geneticAlgorithm) =>
        geneticAlgorithm.Population.GenerationsNumber >= ExpectedGenerationNumber;
}