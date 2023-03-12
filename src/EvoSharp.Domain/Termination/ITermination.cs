namespace EvoSharp.Domain.Termination;

public interface ITermination
{
    bool HasReached<T>(IGeneticAlgorithm<T> geneticAlgorithm);
}
