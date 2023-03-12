using EvoSharp.Domain.Chromosome;
using System.Diagnostics;

namespace EvoSharp.Domain.Population;

public sealed class Generation<T>
{
    public Generation(int number, IList<IChromosome<T>> chromosomes)
    {
        if (number < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(number),
                $"Generation number {number} is invalid. Generation number should be positive and start in 1.");
        }

        if (chromosomes == null || chromosomes.Count < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(chromosomes), "A generation should have at least 2 chromosomes.");
        }

        Number = number;
        Chromosomes = chromosomes;
    }

    public int Number { get; private set; }
    public IList<IChromosome<T>> Chromosomes { get; internal set; }
    public IChromosome<T> BestChromosome { get; internal set; }

    public void End()
    {
        Chromosomes = Chromosomes
            .Where(ValidateChromosome)
            .OrderByDescending(c => c.Fitness.Value)
            .ToList();

        BestChromosome = Chromosomes.First();
    }

    private static bool ValidateChromosome(IChromosome<T> chromosome)
    {
        if (!chromosome.Fitness.HasValue)
        {
            throw new InvalidOperationException("There is unknown problem in current generation, because a chromosome has no fitness value.");
        }

        return true;
    }
}