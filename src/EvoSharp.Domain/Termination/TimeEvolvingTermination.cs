using System;

namespace EvoSharp.Domain.Termination
{
    public class TimeEvolvingTermination : TerminationBase
    {
        public TimeSpan MaxTime { get; set; }

        protected override bool PerformHasReached<T>(GeneticAlgorithm<T> geneticAlgorithm) =>
            geneticAlgorithm.TotalTime >= MaxTime;
    }
}