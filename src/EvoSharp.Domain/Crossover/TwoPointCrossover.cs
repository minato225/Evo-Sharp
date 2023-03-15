using EvoSharp.Domain.Chromosome;

namespace EvoSharp.Domain.Crossover;

public class TwoPointCrossover : CrossoverBase
{
    public TwoPointCrossover(int swapPointOneGeneIndex, int swapPointTwoGeneIndex) : base(2, 2)
    {
        if (swapPointOneGeneIndex >= swapPointTwoGeneIndex)
        {
            throw new ArgumentOutOfRangeException(nameof(swapPointTwoGeneIndex), "The swap point two gene index should be greater than swap point one index.");
        }

        SwapPointOneGeneIndex = swapPointOneGeneIndex;
        SwapPointTwoGeneIndex = swapPointTwoGeneIndex;
        MinChromosomeLength = 3;
    }

    public int SwapPointOneGeneIndex { get; set; }

    public int SwapPointTwoGeneIndex { get; set; }

    protected override IList<IChromosome<T>> PerformCross<T>(IList<IChromosome<T>> parents)
    {
        var firstParent = parents[0];
        var secondParent = parents[1];
        var parentLength = firstParent.Length;
        var swapPointsLength = parentLength - 1;

        if (SwapPointTwoGeneIndex >= swapPointsLength)
        {
            throw new ArgumentOutOfRangeException(nameof(parents),
                "The swap point two index is {0}, but there is only {1} genes. The swap should result at least one gene to each sides.");
        }

        return CreateChildren(firstParent, secondParent);
    }

    protected IList<IChromosome<T>> CreateChildren<T>(IChromosome<T> firstParent, IChromosome<T> secondParent)
    {
        var firstChild = CreateChild(firstParent, secondParent);
        var secondChild = CreateChild(secondParent, firstParent);

        return new List<IChromosome<T>> { firstChild, secondChild };
    }

    protected IChromosome<T> CreateChild<T>(IChromosome<T> leftParent, IChromosome<T> rightParent)
    {
        var firstCutGenesCount = SwapPointOneGeneIndex + 1;
        var secondCutGenesCount = SwapPointTwoGeneIndex + 1;
        var child = leftParent.CreateNew();
        child.ReplaceGenes(0, leftParent.Genes[..firstCutGenesCount]);
        child.ReplaceGenes(firstCutGenesCount, rightParent.Genes[firstCutGenesCount..secondCutGenesCount]);
        child.ReplaceGenes(secondCutGenesCount, leftParent.Genes[secondCutGenesCount..]);

        return child;
    }
}