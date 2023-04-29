using System;
using System.Linq;

namespace EvoSharp.Domain.Utils
{
    public static class RandomUtils
    {
        public static int[] GetUniqueInts(int length, int min, int max)
        {
            var random = new Random();
            var diff = max - min;

            if (diff < length)
            {
                throw new ArgumentOutOfRangeException(nameof(length),
                    $"The length is {length}, but the possible unique values between {min} (inclusive) and {max} (exclusive) are {diff}.");
            }

            var orderedValues = Enumerable.Range(min, diff).ToList();
            var ints = new int[length];

            for (int i = 0; i < length; i++)
            {
                var removeIndex = random.Next(0, orderedValues.Count);
                ints[i] = orderedValues[removeIndex];
                orderedValues.RemoveAt(removeIndex);
            }

            return ints;
        }

        public static float NextFloat(this Random rand, float minValue = 0.0f, float maxValue = 1.0f)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentException("minValue must be less than maxValue");
            }

            double range = (double)maxValue - (double)minValue;
            double scaled = rand.NextDouble() * range;
            float shifted = (float)(scaled + minValue);
            return shifted;
        }
    }
}