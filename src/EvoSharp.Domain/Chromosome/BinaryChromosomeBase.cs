namespace EvoSharp.Domain.Chromosome;

public class BinaryChromosome : ChromosomeBase<bool>
{
    public BinaryChromosome(int length) : 
        base(length, false, false)
    {
    }

    public BinaryChromosome(bool[] genes) : base(genes)
    {
    }

    public override IChromosome<bool> CreateNew() => 
        new BinaryChromosome(_length);

    public virtual void FlipGene(int index) =>
        this[index] = !this[index];

    public override bool GenerateGene() =>
        new Random().Next(2) == 1;

    public override string ToString() =>
        string.Join(',', Genes);
}

