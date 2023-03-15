namespace EvoSharp.Domain.Chromosome;

public class IntChromosome : ChromosomeBase<int>
{
    private readonly int _minValue;
    private readonly int _maxValue;

    public IntChromosome(int length, int minValue, int maxValue) : base(length)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        base.CreateGenes();
    }

    public IntChromosome(int minValue, int maxValue) : this(32, minValue, maxValue) { }

    public override IChromosome<int> CreateNew() => 
        new IntChromosome(_length, _minValue, _maxValue);

    public override int GenerateGene() => 
        _random.Next(_minValue, _maxValue + 1);

    public override string ToString() => 
        string.Join(',', Genes);
}

