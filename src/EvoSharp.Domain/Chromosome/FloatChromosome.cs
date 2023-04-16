namespace EvoSharp.Domain.Chromosome;

public class FloatChromosome : ChromosomeBase<float>
{
    public FloatChromosome(int length, float minValue, float maxValue) :
        base(length, minValue, maxValue)
    {
    }

    public FloatChromosome(float[] genes) : base(genes)
    {
    }

    public override IChromosome<float> CreateNew() =>
        new FloatChromosome(_length, _minValue, _maxValue);

    public override float GenerateGene() =>
        RandomFloat(_minValue, _maxValue);

    public override string ToString() =>
        string.Join(',', Genes);

    private float RandomFloat(float min, float max) =>
        (float)(min + (max - min) * _random.NextDouble());
}