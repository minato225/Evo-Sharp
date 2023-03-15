namespace EvoSharp.Domain.Chromosome;

public abstract class ChromosomeBase<T> : IChromosome<T>
{
    private T[] _genes;
    protected int _length;
    protected readonly Random _random = new();

    protected ChromosomeBase(int length)
    {
        ValidateLength(length);

        _length = length;
        _genes = new T[length];
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
        ArgumentNullException.ThrowIfNull(genes);

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

    protected virtual void CreateGene(int index) => this[index] = GenerateGene();

    protected virtual void CreateGenes()
    {
        for (int i = 0; i < Length; i++)
            CreateGene(i);
    }

    private static void ValidateLength(int length)
    {
        if (length < 2)
        {
            throw new ArgumentException("The minimum length for a chromosome is 2 genes.", nameof(length));
        }
    }
}