using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Selection;

namespace EvoSharp.Domain.UnitTests.Selection;

[TestFixture]
[Category("Selection")]
public class TournamentSelectionTests
{
    [Test]
    public void PerformSelection_TournamentSizeGreaterThanChromosomeCount_ThrowsArgumentException()
    {
        // Arrange
        var chromosomes = new List<IChromosome<int>>
        {
            new IntChromosome(1, 5),
            new IntChromosome(2, 6)
        };
        var selection = new TournamentSelection<int>(3);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => selection.Selection(2, chromosomes));
    }

    [Test]
    public void PerformSelection_SelectsChromosomesWithHighestFitnessValues()
    {
        // Arrange
        var chromosomes = new List<IChromosome<int>>
        {
            new IntChromosome(new[] { 1, 2, 3 }){FitnessValue = 10},
            new IntChromosome(new[] { 4, 5, 6 }){FitnessValue = 20},
            new IntChromosome(new[] { 7, 8, 9 }){FitnessValue = 15},
            new IntChromosome(new[] { 10, 11, 12 }) { FitnessValue = 5 },
            new IntChromosome(new[] { 13, 14, 15 }) { FitnessValue = 25 }
        };

        var selection = new TournamentSelection<int>(3);

        // Act
        var selectedChromosomes = selection.Selection(2, chromosomes);

        // Assert
        Assert.That(selectedChromosomes, Has.Count.EqualTo(2));
    }
}