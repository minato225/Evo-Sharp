namespace EvoSharp.Domain.Chromosome;

public abstract class ChromosomeBase<T> : IChromosome<T>
{
    private T[] m_genes;
    protected int m_length;
    protected readonly Random _random = new();

    protected ChromosomeBase(int length)
    {
        ValidateLength(length);

        m_length = length;
        m_genes = new T[length];
    }

    public T this[int i]
    {
        get => m_genes[i];
        set
        {
            if (i < 0 || i >= m_length)
                throw new ArgumentOutOfRangeException(nameof(i), $"There is no Gene on index {i} to be replaced.");
            m_genes[i] = value;
            Fitness = null;
        }
    }

    public double? Fitness { get; set; }
    public int Length => m_length;
    public T[] Genes => m_genes;

    public static bool operator ==(ChromosomeBase<T> first, ChromosomeBase<T> second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        return first.CompareTo(second) == 0;
    }

    public static bool operator !=(ChromosomeBase<T> first, ChromosomeBase<T> second) => !(first == second);

    public static bool operator <(ChromosomeBase<T> first, ChromosomeBase<T> second)
    {
        if (ReferenceEquals(first, second))
        {
            return false;
        }

        if (first is null)
        {
            return true;
        }

        if (second is null)
        {
            return false;
        }

        return first.CompareTo(second) < 0;
    }

    public static bool operator >(ChromosomeBase<T> first, ChromosomeBase<T> second) => !(first == second) && !(first < second);

    public abstract T GenerateGene();

    public abstract IChromosome<T> CreateNew();

    public virtual IChromosome<T> Clone()
    {
        var clone = CreateNew();
        clone.ReplaceGenes(0, Genes);
        clone.Fitness = Fitness;

        return clone;
    }

    public void ReplaceGenes(int startIndex, T[] genes)
    {
        ArgumentNullException.ThrowIfNull(genes);

        if (genes.Length == 0) return;

        if (startIndex < 0 || startIndex >= m_length)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex), $"There is no Gene on index {startIndex} to be replaced.");
        }

        Array.Copy(genes, 0, m_genes, startIndex, Math.Min(genes.Length, m_length - startIndex));

        Fitness = null;
    }

    public void Resize(int newLength)
    {
        ValidateLength(newLength);

        Array.Resize(ref m_genes, newLength);
        m_length = newLength;
    }

    public int CompareTo(IChromosome<T> other)
    {
        if (other == null)
        {
            return -1;
        }

        var otherFitness = other.Fitness;

        if (Fitness == otherFitness)
        {
            return 0;
        }

        return Fitness > otherFitness ? 1 : -1;
    }

    public override bool Equals(object obj) => obj is IChromosome<T> other && CompareTo(other) == 0;

    public override int GetHashCode() => Fitness.GetHashCode();

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