using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Fitness;
public interface IFitness<T>
{
    double Evaluate(IChromosome<T> chromosome);
}
