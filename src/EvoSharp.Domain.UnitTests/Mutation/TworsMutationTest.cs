using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Mutation;

namespace EvoSharp.Domain.UnitTests.Mutation;

[TestFixture]
[Category("Mutation")]
public class TworsMutationTests
{
    [Test]
    public void Mutate_SwapsTwoRandomGenesWithProbabilityLessThanOne()
    {
        // Arrange
        var chromosome = new IntChromosome(5, 1, 10);
        var mutation = new TworsMutation();
        var originalGenes = chromosome.Genes.ToArray();

        // Act
        mutation.Mutate(chromosome, 0.5f);

        // Assert
        var mutatedGenes = chromosome.Genes.ToArray();
        Assert.That(mutatedGenes, Has.Length.EqualTo(originalGenes.Length));

        int differences = 0;
        for (int i = 0; i < originalGenes.Length; i++)
        {
            if (!EqualityComparer<int>.Default.Equals(originalGenes[i], mutatedGenes[i]))
            {
                differences++;
            }
        }

        Assert.That(differences, Is.EqualTo(0), "Unexpected number of gene swaps.");
    }

    [Test]
    public void Mutate_DoesNotMutateWithProbabilityEqualToZero()
    {
        // Arrange
        var chromosome = new IntChromosome(5, 1, 10);
        var mutation = new TworsMutation();
        var originalGenes = chromosome.Genes.ToArray();

        // Act
        mutation.Mutate(chromosome, 0.0f);

        // Assert
        var mutatedGenes = chromosome.Genes.ToArray();
        CollectionAssert.AreEqual(originalGenes, mutatedGenes, "Genes should not have changed.");
    }

    [Test]
    public void Mutate_ThrowsExceptionForNullChromosome()
    {
        // Arrange
        var mutation = new TworsMutation();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mutation.Mutate<int>(null, 1.0f));
    }
}
