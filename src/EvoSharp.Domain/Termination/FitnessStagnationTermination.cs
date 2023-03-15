namespace EvoSharp.Domain.Termination;

/// <summary>
/// Fitness Stagnation Termination.    
/// <remarks>
/// The genetic algorithm will be terminate when the best chromosome's fitness has no change in the last generations specified.
/// </remarks>
/// </summary>
public class FitnessStagnationTermination : TerminationBase
{
    private double _lastFitness;
    private int _stagnantCount;

    public FitnessStagnationTermination(int expectedStagnantGenerationsNumber) => ExpectedStagnantCount = expectedStagnantGenerationsNumber;

    public int ExpectedStagnantCount { get; set; }

    protected override bool PerformHasReached<T>(GeneticAlgorithm<T> ga)
    {
        var bestFitness = ga.Population.BestChromosome.FitnessValue.Value;

        _stagnantCount = _lastFitness == bestFitness ? _stagnantCount + 1 : 1;

        _lastFitness = bestFitness;

        return _stagnantCount >= ExpectedStagnantCount;
    }
}
