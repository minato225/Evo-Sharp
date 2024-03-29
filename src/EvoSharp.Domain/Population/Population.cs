using EvoSharp.Domain.Chromosome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvoSharp.Domain.Population
{
    public class Population<T> : IPopulation<T>
    {
        public event EventHandler BestChromosomeChanged;

        public Population(int minSize, int maxSize, IChromosome<T> firstChromosome)
        {
            if (minSize < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(minSize), "The minimum size for a population is 2 chromosomes.");
            }

            if (maxSize < minSize)
            {
                throw new ArgumentOutOfRangeException(nameof(maxSize), "The maximum size for a population should be equal or greater than minimum size.");
            }

            MinSize = minSize;
            MaxSize = maxSize;
            FirstChromosome = firstChromosome;
            Generations = new List<Generation<T>>();
        }

        public IList<Generation<T>> Generations { get; protected set; }
        public Generation<T> CurrentGeneration { get; protected set; }
        public int GenerationsNumber { get; protected set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public IChromosome<T> BestChromosome { get; protected set; }
        protected IChromosome<T> FirstChromosome { get; set; }

        public virtual void InitGeneration()
        {
            Generations = new List<Generation<T>>();
            GenerationsNumber = 0;

            var chromosomes = Enumerable.Range(0, MinSize).Select(x => FirstChromosome.CreateNew()).ToList();

            CreateNewGeneration(chromosomes);
        }

        public virtual void CreateNewGeneration(IList<IChromosome<T>> chromosomes)
        {
            if (chromosomes == null)
            {
                throw new ArgumentNullException(nameof(chromosomes));
            }

            CurrentGeneration = new Generation<T>(++GenerationsNumber, chromosomes);
            Generations.Add(CurrentGeneration);
        }

        public virtual void EndCurrentGeneration()
        {
            CurrentGeneration.EndGeneration();

            if (BestChromosome is null || BestChromosome.CompareTo(CurrentGeneration.BestChromosome) != 0)
            {
                BestChromosome = CurrentGeneration.BestChromosome;

                OnBestChromosomeChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnBestChromosomeChanged(EventArgs args) =>
            BestChromosomeChanged?.Invoke(this, args);
    }
}