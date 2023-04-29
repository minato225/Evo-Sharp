namespace EvoSharp.Domain.Termination
{
    public interface ITermination
    {
        bool HasReached<T>(GeneticAlgorithm<T> geneticAlgorithm);
    }
}