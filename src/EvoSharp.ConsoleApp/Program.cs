using EvoSharp.Domain;
using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Crossover;
using EvoSharp.Domain.Mutation;
using EvoSharp.Domain.Population;
using EvoSharp.Domain.Selection;
using EvoSharp.Domain.Termination;


var chromosome = new IntChromosome(32, 0, 1);
var population = new Population<int>(50, 100, chromosome);
var fitness = new Func<IChromosome<int>, double>(c => c.Genes.Sum());
var selection = new RankSelection<int>();
var crossover = new UniformCrossover(0.5f);
var mutation = new TworsMutation();
var termination = new GenNumberTermination { MaxGenCount = 50 };

var ga = new GeneticAlgorithm<int>(population, fitness, selection, crossover, mutation)
{
    Termination = termination
};

var latestFitness = 0.0;

ga.GenerationRan += (_, _) =>
{
    var bestChromosome = ga.Population.BestChromosome as IntChromosome;
    var bestFitness = bestChromosome.FitnessValue.Value;

    if (bestFitness != latestFitness)
    {
        latestFitness = bestFitness;
        var phenotype = bestChromosome.ToString();

        Console.WriteLine($"Generation {ga.Population.GenerationsNumber}: Fitnes = {bestFitness}, best gene: {ga.Population.BestChromosome}");
    }
};

ga.Start();