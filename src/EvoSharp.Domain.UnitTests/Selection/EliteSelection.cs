using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Population;
using EvoSharp.Domain.Selection;

namespace EvoSharp.Domain.UnitTests.Selection;

[TestFixture]
[Category("Selection")]
public class EliteSelectionTests
{
    private EliteSelection<int> _selection;

    [SetUp]
    public void SetUp()
    {
        _selection = new EliteSelection<int>(0);
    }

    [Test]
    public void Selection_ShouldThrowException_WhenMinSizeIsLessThanTwo()
    {
        var chromosomes = new List<IChromosome<int>>
        {
            new IntChromosome(new int[] { 0, 1, 0 }),
            new IntChromosome(new int[] { 1, 0, 0 }),
            new IntChromosome(new int[] { 0, 1, 1 }),
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => _selection.Selection(1, chromosomes));
    }

    [Test]
    public void Selection_ShouldThrowException_WhenChromosomesHasNullFitness()
    {
        var chromosomes = new List<IChromosome<int>>
        {
            new IntChromosome(new int[] { 0, 1, 0 }),
            new IntChromosome(new int[] { 1, 0, 0 }),
            new IntChromosome(new int[] { 0, 1, 1 }),
            new IntChromosome(new int[] { 0, 1, 1 }),
        };

        Assert.Throws<ArgumentException>(() => _selection.Selection(2, chromosomes));
    }

    [Test]
    public void Selection_ShouldReturnTwoChromosomes_WhenTwoAreRequested()
    {
        var chromosomes = new List<IChromosome<int>>
        {
            new IntChromosome(new int[] { 0, 1, 0 }) { FitnessValue = 0.3 },
            new IntChromosome(new int[] { 1, 0, 0 }) { FitnessValue = 0.5 },
            new IntChromosome(new int[] { 0, 1, 1 }) { FitnessValue = 0.7 },
            new IntChromosome(new int[] { 0, 1, 1 }) { FitnessValue = 0.9 }
        };

        var result = _selection.Selection(2, chromosomes);

        Assert.That(result, Has.Count.EqualTo(2));
    }
}