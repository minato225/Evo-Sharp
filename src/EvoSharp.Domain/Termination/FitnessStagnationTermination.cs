namespace EvoSharp.Domain.Termination;

/// <summary>
/// Fitness Stagnation Termination.    
/// <remarks>
/// The genetic algorithm will be terminate when the best chromosome's fitness has no change in the last generations specified.
/// </remarks>
/// </summary>
public class FitnessStagnationTermination : TerminationBase
{
    private double m_lastFitness;
    private int m_stagnantGenerationsCount;

    public FitnessStagnationTermination(int expectedStagnantGenerationsNumber) => ExpectedStagnantGenerationsNumber = expectedStagnantGenerationsNumber;

    public int ExpectedStagnantGenerationsNumber { get; set; }

    protected override bool PerformHasReached<T>(GeneticAlgorithm<T> geneticAlgorithm)
    {
        var bestFitness = geneticAlgorithm.Population.BestChromosome.Fitness.Value;

        m_stagnantGenerationsCount = m_lastFitness == bestFitness ? m_stagnantGenerationsCount + 1 : 1;

        m_lastFitness = bestFitness;

        return m_stagnantGenerationsCount >= ExpectedStagnantGenerationsNumber;
    }
}
