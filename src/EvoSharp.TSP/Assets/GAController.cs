using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using EvoSharp.Domain;
using EvoSharp.Domain.Chromosome;
using EvoSharp.Domain.Crossover;
using EvoSharp.Domain.Mutation;
using EvoSharp.Domain.Population;
using EvoSharp.Domain.Selection;
using EvoSharp.Domain.Termination;
using UnityEngine;
using UnityEngine.UI;
using UnityObject = UnityEngine.Object;
using UnityRundom = UnityEngine.Random;

public class GAController : MonoBehaviour
{
    private GeneticAlgorithm<int> _ga;
    private LineRenderer _lr;
    private Rect _map;
    private string _text;
    private Thread _gaThread;

    public Text Statistics;
    public UnityObject CityPrefab;
    public int CitiesCounts;

    private IList<TspCity> Cities { get; set; }
    private double Distance { get; set; }

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.positionCount = CitiesCounts + 1;
    }

    private void Start()
    {
        var size = Camera.main.orthographicSize - 1;
        _map = new Rect(-size, -size, size * 2, size * 2);

        Cities = Enumerable.Range(0, CitiesCounts)
            .Select(_ => new TspCity { Position = GetRandomPosition() })
            .ToList();

        var chromosome = new IntChromosome(CitiesCounts, 0, CitiesCounts - 1);
        var crossover = new UniformCrossover();
        var mutation = new ReverseSequenceMutation();
        var selection = new RouletteWheelSelection<int>();
        var population = new Population<int>(50, 100, chromosome);
        var termination = new TimeEvolvingTermination { MaxTime = TimeSpan.FromMinutes(1) };

        _ga = new GeneticAlgorithm<int>(population, Fitness, selection, crossover, mutation)
        {
            Termination = termination
        };

        double? latestFitness = 0.0;

        _ga.GenerationRan += delegate
        {
            var bestFitness = _ga.Population.BestChromosome.FitnessValue;

            if (bestFitness != latestFitness)
                latestFitness = bestFitness;

            _text = $"Generation {_ga.Population.GenerationsNumber}:\t\nFitnes = {bestFitness:0.00000}\t\nDistance = {Distance:0.000} km";
        };

        // Draw cities
        for (var i = 0; i < CitiesCounts; i++)
        {
            var city = Cities[i];
            var gameCity = Instantiate(CityPrefab, city.Position, Quaternion.identity) as GameObject;
            gameCity.name = $"City {i}";
            gameCity.GetComponent<CityController>().Data = city;
        }

        _gaThread = new Thread(() => _ga.Start());
        _gaThread.Start();
    }

    private void Update()
    {
        Statistics.text = _text;

        // draw root
        var bestChromosome = _ga?.Population?.CurrentGeneration?.BestChromosome;

        if (bestChromosome is null) return;

        var genes = bestChromosome.Genes;

        for (int i = 0; i < genes.Length; i++)
            _lr.SetPosition(i, Cities[genes[i]].Position);

        _lr.SetPosition(CitiesCounts, Cities[genes[0]].Position);
    }

    private void OnDestroy() => _gaThread.Abort();

    private double Fitness(IChromosome<int> chromosome)
    {
        try
        {
            var genes = chromosome.Genes;
            var distanceSum = 0.0;
            var lastCityIndex = genes[0];
            var citiesIndexes = new List<int> { lastCityIndex };

            foreach (var gene in genes)
            {
                distanceSum += Vector2.Distance(Cities[gene].Position, Cities[lastCityIndex].Position);
                lastCityIndex = gene;

                citiesIndexes.Add(lastCityIndex);
            }

            distanceSum += Vector2.Distance(Cities[citiesIndexes.Last()].Position, Cities[citiesIndexes.First()].Position);
            Distance = distanceSum;

            var diff = Cities.Count - citiesIndexes.Distinct().Count();
            var fitness = 1.0 - (distanceSum / (Cities.Count * 1000.0));

            if (diff > 0)
                fitness /= diff;

            if (fitness < 0)
                fitness = 0;

            return fitness;
        }
        catch (ArgumentNullException ex)
        {
            Debug.Log(ex);
        }

        return 0;
    }

    private Vector2 GetRandomPosition()
        => new(
            UnityRundom.Range(_map.xMin, _map.xMax),
            UnityRundom.Range(_map.yMin, _map.yMax)
            );
}