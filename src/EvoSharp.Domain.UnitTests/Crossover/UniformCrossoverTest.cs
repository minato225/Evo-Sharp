using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Crossover;

namespace EvoSharp.Domain.UnitTests.Crossover;

[TestFixture]
[Category("Crossover")]
public class UniformCrossoverTest
{
    [Test]
    public void Cross_ParentsWithLessThanMinLength_ThrowsArgumentNullException()
    {
        // Arrange
        var crossover = new UniformCrossover();
        var parents = new List<IChromosome<int>>
        {
            new IntChromosome(new[] {1}),
            new IntChromosome(new[] {2}),
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => crossover.Cross(parents));
    }

    [Test]
    public void Cross_ValidInput_ReturnsTwoChildren()
    {
        // Arrange
        var crossover = new UniformCrossover();
        var parents = new List<IChromosome<int>>
        {
            new IntChromosome(new[] {1, 2, 3, 4, 5}),
            new IntChromosome(new[] {6, 7, 8, 9, 10}),
        };

        // Act
        var children = crossover.Cross(parents);

        // Assert
        Assert.That(children, Has.Count.EqualTo(2));
    }
}
