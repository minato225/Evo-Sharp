namespace EvoSharp.Domain.Chromosome;

public class IntegerChromosome : ChromosomeBase<int>
{
    private readonly int _minValue;
    private readonly int _maxValue;

    public IntegerChromosome(int minValue, int maxValue) : base(32)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        base.CreateGenes();
    }

    public override IChromosome<int> CreateNew() => new IntegerChromosome(_minValue, _maxValue);

    public override int GenerateGene() => _random.Next(_minValue, _maxValue);

    public override string ToString() => string.Join(',', Genes);
}

