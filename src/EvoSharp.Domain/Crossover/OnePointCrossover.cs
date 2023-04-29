using EvoSharp.Domain.Chromosome;
using System;
using System.Collections.Generic;

namespace EvoSharp.Domain.Crossover
{
    public class OnePointCrossover : CrossoverBase
    {
        public OnePointCrossover(int swapPointIndex) : base(2, 2) =>
            SwapPointIndex = swapPointIndex;
        public int SwapPointIndex { get; set; }

        protected override IList<IChromosome<T>> PerformCross<T>(IList<IChromosome<T>> parents)
        {
            var (firstParent, secondParent) = (parents[0], parents[1]);

            if (SwapPointIndex >= firstParent.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(parents),
                    $"The swap point index is {SwapPointIndex}, but there is only {firstParent.Length} genes. The swap should result at least one gene to each side.");
            }

            return CreateChildren(firstParent, secondParent);
        }

        protected IList<IChromosome<T>> CreateChildren<T>(IChromosome<T> firstParent, IChromosome<T> secondParent)
        {
            var firstChild = CreateChild(firstParent, secondParent);
            var secondChild = CreateChild(secondParent, firstParent);

            return new List<IChromosome<T>> { firstChild, secondChild };
        }

        protected virtual IChromosome<T> CreateChild<T>(IChromosome<T> leftParent, IChromosome<T> rightParent)
        {
            var cutGenesCount = SwapPointIndex + 1;
            var child = leftParent.CreateNew();
            child.ReplaceGenes(0, leftParent.Genes[..cutGenesCount]);
            child.ReplaceGenes(cutGenesCount, rightParent.Genes[cutGenesCount..]);

            return child;
        }
    }
}