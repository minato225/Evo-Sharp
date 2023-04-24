using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Crossover;

namespace EvoSharp.Domain.UnitTests.Crossover;

[TestFixture]
[Category("Crossover")]
public class TwoPointCrossoverTest
{
    [Test]
    public void Cross_FirstIndexLargeSecond_ThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new TwoPointCrossover(10, 5));
    }


    [Test]
    public void Cross_ValidInput_ReturnsTwoChildren()
    {
        // Arrange
        var crossover = new TwoPointCrossover(2, 3);
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

    [Test]
    public void SmokeTest()
    {
        // Arrange
        var crossover = new TwoPointCrossover(1,3);
        var parents = new List<IChromosome<int>>
        {
            new IntChromosome(new[] {1, 2, 3, 4, 5}),
            new IntChromosome(new[] {6, 7, 8, 9, 10}),
        };

        // Act
        var children = crossover.Cross(parents).ToArray();

        // Assert
        Assert.That(new[] { 1, 2, 8, 9, 5 }, Is.EqualTo(children[0].Genes));
    }
}
