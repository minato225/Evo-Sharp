using System;

namespace EvoSharp.Domain.Termination
{
    public abstract class TerminationBase : ITermination
    {
        private bool _hasReached;

        public bool HasReached<T>(GeneticAlgorithm<T> geneticAlgorithm)
        {
            if (geneticAlgorithm == null)
            {
                throw new ArgumentNullException(nameof(geneticAlgorithm));
            }

            _hasReached = PerformHasReached(geneticAlgorithm);

            return _hasReached;
        }
        public override string ToString() =>
            $"{GetType().Name} (HasReached: {_hasReached})";

        protected abstract bool PerformHasReached<T>(GeneticAlgorithm<T> geneticAlgorithm);
    }
}