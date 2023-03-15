namespace EvoSharp.Domain.Termination;

/// <summary>
/// Fitness Threshold Termination
/// <remarks>
/// The genetic algorithm will be terminate when the best chromosome reach the expected fitness.
/// </remarks>
/// </summary>
public class FitnessThresholdTermination : TerminationBase
{
    public required double ExpectedFitness { get; set; }

    protected override bool PerformHasReached<T>(GeneticAlgorithm<T> geneticAlgorithm) => 
        geneticAlgorithm.Population.BestChromosome.FitnessValue >= ExpectedFitness;
}
