namespace EvoSharp.Domain.Chromosome;

public class BinaryChromosome : ChromosomeBase<bool>
{
    protected BinaryChromosome(int length) :
        base(length)
    { }

    public override IChromosome<bool> CreateNew()
    {
        throw new NotImplementedException();
    }

    public virtual void FlipGene(int index) =>
        this[index] = !this[index];

    public override bool GenerateGene() =>
        new Random().Next(2) == 1;

    public override string ToString() =>
        string.Join(',', Genes);
}

