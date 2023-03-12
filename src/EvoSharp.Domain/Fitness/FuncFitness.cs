using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Fitness;

public class FuncFitness<T> : IFitness<T>
{
    private readonly Func<IChromosome<T>, double> m_func;

    public FuncFitness(Func<IChromosome<T>, double> func) => m_func = func;

    public double Evaluate(IChromosome<T> chromosome) => m_func(chromosome);
}