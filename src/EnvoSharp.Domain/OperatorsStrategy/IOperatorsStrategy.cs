using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Crossover;
using EvoSharp.Domain.Mutation;
using EvoSharp.Domain.Population;
using System.Collections.Generic;

namespace EvoSharp.Domain.OperatorsStrategy
{
    /// <summary>
    /// Defines an interface for operators strategy.
    /// </summary>
    public interface IOperatorsStrategy<T>
    {
        /// <summary>
        /// Crosses the specified parents.
        /// </summary>
        /// <param name="crossover">The crossover class.</param>
        /// <param name="crossoverProbability">The crossover probability.</param>
        /// <param name="parents">The parents.</param>
        /// <returns>The result chromosomes.</returns>
        IList<IChromosome<T>> Cross(IPopulation<T> population, ICrossover crossover, float crossoverProbability, IList<IChromosome<T>> parents);

        /// <summary>
        /// Mutate the specified chromosomes.
        /// </summary>
        /// <param name="mutation">The mutation class.</param>
        /// <param name="mutationProbability">The mutation probability.</param>
        /// <param name="chromosomes">The chromosomes.</param>
        void Mutate(IMutation mutation, float mutationProbability, IList<IChromosome<T>> chromosomes);
    }
}
