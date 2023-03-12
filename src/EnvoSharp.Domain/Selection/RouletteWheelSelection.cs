using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;

public class RouletteWheelSelection : SelectionBase
{
    public RouletteWheelSelection() : base(2)
    {
    }

    protected static IList<IChromosome<T>> SelectFromWheel<T>(int number, IList<IChromosome<T>> chromosomes, IList<double> rouletteWheel, Func<double> getPointer)
    {
        var selected = new List<IChromosome<T>>();

        for (int i = 0; i < number; i++)
        {
            var chromosome = rouletteWheel
                .Select((value, index) => new { Value = value, Index = index })
                .FirstOrDefault(r => r.Value >= getPointer());

            if (chromosome != null)
                selected.Add(chromosomes[chromosome.Index].Clone());
        }

        return selected;
    }

    /// <summary>
    /// Calculates the cumulative percent.
    /// </summary>
    /// <param name="chromosomes">The chromosomes.</param>
    /// <param name="rouletteWheel">The roulette wheel.</param>
    protected static void CalculateCumulativePercentFitness<T>(IList<IChromosome<T>> chromosomes, IList<double> rouletteWheel)
    {
        var sumFitness = chromosomes.Sum(c => c.Fitness.Value);

        var cumulativePercent = 0.0;

        for (int i = 0; i < chromosomes.Count; i++)
        {
            cumulativePercent += chromosomes[i].Fitness.Value / sumFitness;
            rouletteWheel.Add(cumulativePercent);
        }
    }

    protected override IList<IChromosome<T>> PerformSelectChromosomes<T>(int number, Generation<T> generation)
    {
        var chromosomes = generation.Chromosomes;
        var rouletteWheel = new List<double>();

        CalculateCumulativePercentFitness(chromosomes, rouletteWheel);

        return SelectFromWheel(number, chromosomes, rouletteWheel, () => _random.NextSingle());
    }
}