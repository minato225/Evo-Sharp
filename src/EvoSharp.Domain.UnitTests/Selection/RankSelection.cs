using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Selection;

namespace EvoSharp.Domain.UnitTests.Selection;

[TestFixture]
[Category("Selection")]
public class RankSelectionTests
{
    [Test]
    public void TestRankSelection()
    {
        // Arrange
        var chromosomes = new List<IChromosome<int>>()
        {
            new IntChromosome(new [] { 1, 2, 3 }),
            new IntChromosome(new [] { 2, 3, 4 }),
            new IntChromosome(new [] { 3, 4, 5 }),
            new IntChromosome(new [] { 4, 5, 6 }),
            new IntChromosome(new [] { 5, 6, 7 }),
            new IntChromosome(new [] { 6, 7, 8 }),
            new IntChromosome(new [] { 7, 8, 9 }),
            new IntChromosome(new [] { 8, 9, 10 })
        };

        foreach (var chromosome in chromosomes)
        {
            chromosome.FitnessValue = chromosome.Genes.Sum();
        }

        var rankSelection = new RankSelection<int>();

        // Act
        var selectedChromosomes = rankSelection.Selection(4, chromosomes);

        // Assert
        Assert.That(selectedChromosomes, Has.Count.EqualTo(4));

        // Ensure that the selected chromosomes have a higher fitness value on average
        var averageFitnessValueOriginal = chromosomes.Average(c => c.FitnessValue);
        var averageFitnessValueSelected = selectedChromosomes.Average(c => c.FitnessValue);
        Assert.That(averageFitnessValueSelected, Is.GreaterThan(averageFitnessValueOriginal));
    }

    [Test]
    public void TestRankSelection_ThrowsArgumentException()
    {
        // Arrange
        var chromosomes = new List<IChromosome<int>>()
        {
            new IntChromosome(new [] { 1, 2, 3 }),
            new IntChromosome(new [] { 2, 3, 4 }),
            new IntChromosome(new [] { 3, 4, 5 }),
        };

        var rankSelection = new RankSelection<int>();

        // Act and Assert
        Assert.Throws<ArgumentException>(() => rankSelection.Selection(4, chromosomes));
    }
}