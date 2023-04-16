using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Mutation;

namespace EvoSharp.Domain.UnitTests.Mutation;

[TestFixture]
[Category("Mutation")]
public class UniformMutationTests
{
    private UniformMutation _uniformMutation;

    [SetUp]
    public void SetUp()
    {
        _uniformMutation = new UniformMutation(0, 10);
    }

    [Test]
    public void Mutate_NullChromosome_ThrowsArgumentNullException()
    {
        Assert.That(() => _uniformMutation.Mutate<int>(null, 0.5f), Throws.ArgumentNullException);
    }

    [Test]
    public void PerformMutate_ThrowsExceptionForInvalidGeneIndex()
    {
        // Arrange
        var chromosome = new IntChromosome(0, 10);
        var mutation = new UniformMutation(-1, 3, 11);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => mutation.Mutate(chromosome, 1.0f));
    }

    [Test]
    public void PerformMutate_MutatesGenesWithProbabilityLessThanOne()
    {
        // Arrange
        var mutableGenesIndexes = new int[] { 1, 3, 5 };
        var chromosome = new IntChromosome(0, 10);
        var mutation = new UniformMutation(mutableGenesIndexes);
        var originalGenes = chromosome.Genes.ToArray();

        // Act
        mutation.Mutate(chromosome, 0.5f);
        var mutatedGenes = chromosome.Genes.ToArray();

        // Assert
        for (int i = 0; i < originalGenes.Length; i++)
            if (!mutableGenesIndexes.Contains(i))
                Assert.That(mutatedGenes[i], Is.EqualTo(originalGenes[i]));
    }
}
