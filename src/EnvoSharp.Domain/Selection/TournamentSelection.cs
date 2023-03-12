using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;

namespace EvoSharp.Domain.Selection;

public class TournamentSelection : SelectionBase
{
    public TournamentSelection(int size) : base(2) => TrourSize = size;

    public int TrourSize { get; set; }

    protected override IList<IChromosome<T>> PerformSelectChromosomes<T>(int number, Generation<T> generation)
    {
        if (TrourSize > generation.Chromosomes.Count)
        {
            throw new ArgumentException("The tournament size is greater than available chromosomes.");
        }

        var candidates = generation.Chromosomes.ToList();
        var selected = new List<IChromosome<T>>();

        while (selected.Count < number)
        {
            var randomIndexes = GetUniqueInts(TrourSize, 0, candidates.Count);
            var tournamentWinner = candidates.Where((c, i) => randomIndexes.Contains(i)).OrderByDescending(c => c.Fitness).First();

            selected.Add(tournamentWinner.Clone());
        }

        return selected;
    }

    private int[] GetUniqueInts(int length, int min, int max)
    {
        var diff = max - min;

        if (diff < length)
        {
            throw new ArgumentOutOfRangeException(nameof(length),
                $"The length is {length}, but the possible unique values between {min} (inclusive) and {max} (exclusive) are {diff}.");
        }

        var orderedValues = Enumerable.Range(min, diff).ToList();
        var ints = new int[length];

        for (int i = 0; i < length; i++)
        {
            var removeIndex = _random.Next(0, orderedValues.Count);
            ints[i] = orderedValues[removeIndex];
            orderedValues.RemoveAt(removeIndex);
        }

        return ints;
    }
}
