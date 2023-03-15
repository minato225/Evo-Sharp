using EvoSharp.Domain.Chromosome;
using System.Collections.Concurrent;

namespace EvoSharp.Domain.Population;

public class TplPopulation<T> : Population<T>
{
    public TplPopulation(int minSize, int maxSize, IChromosome<T> adamChromosome) : base(minSize, maxSize, adamChromosome)
    {
    }

    public override void InitGeneration()
    {
        Generations = new List<Generation<T>>();
        GenerationsNumber = 0;

        var chromosomes = new ConcurrentBag<IChromosome<T>>();
        Parallel.For(0, MinSize, i => chromosomes.Add(FirstChromosome.CreateNew()));

        CreateNewGeneration(chromosomes.ToList());
    }
}