namespace EvoSharp.Domain.Chromosome;

public abstract class BinaryChromosomeBase : ChromosomeBase<bool>, IBinaryChromosome<bool>
{
    protected BinaryChromosomeBase(int length) :
        base(length)
    { }

    public virtual void FlipGene(int index) =>
        this[index] = !this[index];

    public override bool GenerateGene() =>
        new Random().Next(2) == 1;

    public override string ToString() =>
        string.Join(',', Genes);
}

