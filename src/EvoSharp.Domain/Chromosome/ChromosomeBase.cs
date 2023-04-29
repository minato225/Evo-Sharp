using System;

namespace EvoSharp.Domain.Chromosome
{
    public abstract class ChromosomeBase<T> : IChromosome<T>
    {
        private T[] _genes;
        protected int _length;
        protected readonly T _minValue = default;
        protected readonly T _maxValue = default;
        protected readonly Random _random = new Random();

        public ChromosomeBase(int length, T minValue, T maxValue)
        {
            ValidateLength(length);

            _minValue = minValue;
            _maxValue = maxValue;
            _length = length;
            _genes = CreateGenes(length);
        }

        public ChromosomeBase(T[] genes)
        {
            _genes = genes ?? throw new ArgumentNullException();
            _length = _genes.Length;
        }

        public T this[int i]
        {
            get => _genes[i];
            set
            {
                if (i < 0 || i >= _length)
                    throw new ArgumentOutOfRangeException(nameof(i), $"There is no Gene on index {i} to be replaced.");
                _genes[i] = value;
                FitnessValue = null;
            }
        }

        public double? FitnessValue { get; set; }
        public int Length => _length;
        public T[] Genes => _genes;

        public abstract T GenerateGene();

        public abstract IChromosome<T> CreateNew();

        public virtual IChromosome<T> Clone()
        {
            var clone = CreateNew();
            clone.ReplaceGenes(0, Genes);
            clone.FitnessValue = FitnessValue;

            return clone;
        }

        public void ReplaceGenes(int startIndex, T[] genes)
        {
            if (genes == null)
            {
                throw new ArgumentNullException(nameof(genes));
            }

            if (genes.Length == 0) return;

            if (startIndex < 0 || startIndex >= _length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), $"There is no Gene on index {startIndex} to be replaced.");
            }

            Array.Copy(genes, 0, _genes, startIndex, Math.Min(genes.Length, _length - startIndex));

            FitnessValue = null;
        }

        public void Resize(int newLength)
        {
            ValidateLength(newLength);

            Array.Resize(ref _genes, newLength);
            _length = newLength;
        }

        public int CompareTo(IChromosome<T> other)
        {
            if (other == null)
            {
                return -1;
            }

            var otherFitness = other.FitnessValue;

            if (FitnessValue == otherFitness)
            {
                return 0;
            }

            return FitnessValue > otherFitness ? 1 : -1;
        }

        public override bool Equals(object obj) => obj is IChromosome<T> other && CompareTo(other) == 0;

        public override int GetHashCode() => FitnessValue.GetHashCode();

        private T[] CreateGenes(int length)
        {
            var result = new T[length];

            for (int i = 0; i < length; i++)
                result[i] = GenerateGene();

            return result;
        }

        private static void ValidateLength(int length)
        {
            if (length < 2)
            {
                throw new ArgumentException("The minimum length for a chromosome is 2 genes.", nameof(length));
            }
        }
    }
}