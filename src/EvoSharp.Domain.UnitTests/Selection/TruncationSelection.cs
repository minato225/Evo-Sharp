using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Selection;

namespace EvoSharp.Domain.UnitTests.Selection;

[TestFixture]
[Category("Selection")]
public class TruncationSelectionTests
{
    private TruncationSelection<int> _selection;

    [SetUp]
    public void Setup()
    {
        _selection = new TruncationSelection<int>();
    }

    [Test]
    public void Selection_ThrowsArgumentOutOfRangeException_WhenMinSizeIsLessThanTwo()
    {
        // Arrange
        var chromosomes = new List<IChromosome<int>>()
        {
            new IntChromosome(1,2),
            new IntChromosome(1,2),
            new IntChromosome(1,2)
        };
        var minSize = 1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _selection.Selection(minSize, chromosomes));
    }

    [Test]
    public void Selection_ThrowsArgumentException_WhenAnyChromosomeHasNullFitnessValue()
    {
        // Arrange
        var chromosomes = new List<IChromosome<int>>()
        {
            new IntChromosome(1, 2){FitnessValue = 1},
            new IntChromosome(1, 2){FitnessValue = null},
            new IntChromosome(1, 2)
        };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _selection.Selection(2, chromosomes));
    }

    [Test]
    public void Selection_ReturnsSelectedChromosomes()
    {
        // Arrange
        var chromosomes = new List<IChromosome<int>>()
        {
            new IntChromosome(new []{1, 2, 3}){FitnessValue = 10},
            new IntChromosome(new []{4, 5, 6}){FitnessValue = 5},
            new IntChromosome(new []{7, 8, 9}){FitnessValue = 8},
            new IntChromosome(new []{10, 11, 12}){FitnessValue = 6}
        };

        // Act
        var selectedChromosomes = _selection.Selection(3, chromosomes);

        // Assert
        Assert.That(selectedChromosomes, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(selectedChromosomes.Contains(chromosomes[0]), Is.True);
            Assert.That(selectedChromosomes.Contains(chromosomes[2]), Is.True);
            Assert.That(selectedChromosomes.Contains(chromosomes[3]), Is.True);
        });
    }
}