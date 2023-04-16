using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Mutation;

namespace EvoSharp.Domain.UnitTests.Mutation;

[TestFixture]
[Category("Mutation")]
public class ReverseSequenceMutationTests
{
    [Test]
    public void Mutate_ShouldNotReverseSequence_WhenProbabilityNotMet()
    {
        // Arrange
        var chromosome = new IntChromosome(new[] { 0, 10, 1, 2, 3, 4, 5 });
        var mutation = new ReverseSequenceMutation();
        var probability = 0f; // never perform mutation

        // Act
        mutation.Mutate(chromosome, probability);

        // Assert
        Assert.That(chromosome.Genes, Is.EqualTo(new[] { 0, 10, 1, 2, 3, 4, 5 }));
    }

    [Test]
    public void Mutate_ShouldThrowArgumentException_WhenChromosomeHasLessThan3Genes()
    {
        // Arrange
        var chromosome = new IntChromosome(new[] { 1, 2 });
        var mutation = new ReverseSequenceMutation();
        var probability = 1f; // always perform mutation

        // Act & Assert
        Assert.That(() => mutation.Mutate(chromosome, probability), Throws.ArgumentException);
    }

    [Test]
    public void Mutate_ThrowsExceptionForNullChromosome()
    {
        // Arrange
        var mutation = new ReverseSequenceMutation();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mutation.Mutate<int>(null, 1.0f));
    }
}
