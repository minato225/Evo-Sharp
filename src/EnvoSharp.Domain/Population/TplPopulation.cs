using EvoSharp.Domain.Chromosome;
using System.Collections.Concurrent;

namespace EvoSharp.Domain.Population;

public class TplPopulation<T> : Population<T>
{
    public TplPopulation(int minSize, int maxSize, IChromosome<T> adamChromosome) : base(minSize, maxSize, adamChromosome)
    {
    }

    public override void CreateInitialGeneration()
    {
        Generations = new List<Generation<T>>();
        GenerationsNumber = 0;

        var chromosomes = new ConcurrentBag<IChromosome<T>>();
        Parallel.For(0, MinSize, i =>
        {
            var c = AdamChromosome.CreateNew();

            if (c == null)
            {
                throw new InvalidOperationException("The Adam chromosome's 'CreateNew' method generated a null chromosome. This is a invalid behavior, please, check your chromosome code.");
            }

            chromosomes.Add(c);
        });

        CreateNewGeneration(chromosomes.ToList());
    }
}