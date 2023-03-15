namespace EvoSharp.Domain.Utils;

public class RandomUtils
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
}
