namespace EvoSharp.Domain.Chromosome;

public class FloatChromosome : ChromosomeBase<float>
{
    private readonly float _minValue;
    private readonly float _maxValue;

    public FloatChromosome(float minValue, float maxValue) : base(32)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        base.CreateGenes();
    }

    public override IChromosome<float> CreateNew() =>
        new FloatChromosome(_minValue, _maxValue);

    public override float GenerateGene() =>
        RandomFloat(_minValue, _maxValue);

    public override string ToString() =>
        string.Join(',', Genes);

    private float RandomFloat(float min, float max) =>
        (float)(min + (max - min) * _random.NextDouble());
}