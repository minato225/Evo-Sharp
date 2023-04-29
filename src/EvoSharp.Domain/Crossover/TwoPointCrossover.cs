using EvoSharp.Domain.Chromosome;
using System;
using System.Collections.Generic;

namespace EvoSharp.Domain.Crossover
{
    public class TwoPointCrossover : CrossoverBase
    {
        public TwoPointCrossover(int firstSwapIndex, int secondSwapIndex) : base(2, 2)
        {
            if (firstSwapIndex >= secondSwapIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(secondSwapIndex), "The swap point two gene index should be greater than swap point one index.");
            }

            FirstSwapIndex = firstSwapIndex;
            SecondSwapIndex = secondSwapIndex;
        }

        public int FirstSwapIndex { get; set; }
        public int SecondSwapIndex { get; set; }

        protected override IList<IChromosome<T>> PerformCross<T>(IList<IChromosome<T>> parents)
        {
            var (firstParent, secondParent) = (parents[0], parents[1]);

            if (SecondSwapIndex >= firstParent.Length - 1)
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
            var firstCutGenesCount = FirstSwapIndex + 1;
            var secondCutGenesCount = SecondSwapIndex + 1;
            var child = leftParent.CreateNew();
            child.ReplaceGenes(0, leftParent.Genes[..firstCutGenesCount]);
            child.ReplaceGenes(firstCutGenesCount, rightParent.Genes[firstCutGenesCount..secondCutGenesCount]);
            child.ReplaceGenes(secondCutGenesCount, leftParent.Genes[secondCutGenesCount..]);

            return child;
        }
    }
}