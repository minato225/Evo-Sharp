using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Mutation;

namespace EvoSharp.Domain.UnitTests.Mutation;

[TestFixture]
[Category("Mutation")]
public class FlipBitMutationTests
{
    [Test]
    public void Mutate_GivenNonBinaryChromosome_ThrowsArgumentException()
    {
        // Arrange
        var mutation = new FlipBitMutation();
        var chromosome = new IntChromosome(0, 10);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => mutation.Mutate(chromosome, 0.5f));
    }

    [Test]
    public void Mutate_GivenBinaryChromosome_DoesNotFlipGeneIfProbabilityIsNotMet()
    {
        // Arrange
        var mutation = new FlipBitMutation();
        var chromosome = new BinaryChromosome(new bool[] {true, true, true });

        // Act
        mutation.Mutate(chromosome, 0.0f);

        // Assert
        Assert.That(chromosome.Genes.All(gene => gene), Is.True);
    }
}