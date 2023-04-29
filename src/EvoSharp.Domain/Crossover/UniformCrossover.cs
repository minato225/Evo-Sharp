using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Utils;
using System;
using System.Collections.Generic;

namespace EvoSharp.Domain.Crossover
{
    public class UniformCrossover : CrossoverBase
    {
        public UniformCrossover(float mixProbability = .5f) : base(2, 2) =>
            Probability = mixProbability;
        public float Probability { get; set; }

        protected override IList<IChromosome<T>> PerformCross<T>(IList<IChromosome<T>> parents)
        {
            var (firstParent, secondParent) = (parents[0], parents[1]);
            var (firstChild, secondChild) = (firstParent.CreateNew(), secondParent.CreateNew());

            var rnd = new Random();

            for (int i = 0; i < firstParent.Length; i++)
            {
                (firstChild[i], secondChild[i]) = rnd.NextFloat() < Probability ?
                    (firstParent[i], secondParent[i]) :
                    (secondParent[i], firstParent[i]);
            }

            return new List<IChromosome<T>> { firstChild, secondChild };
        }
    }
}