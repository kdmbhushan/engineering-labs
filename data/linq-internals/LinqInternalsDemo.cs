namespace EngineeringLabs.LinqInternals;

public static class LinqInternalsDemo
{
    // Demonstrates compiler-generated iterator state machine behavior
    public static IEnumerable<int> GenerateStreamingNumbers(int count, Action<int>? onStep = null)
    {
        for (var i = 1; i <= count; i++)
        {
            onStep?.Invoke(i);
            yield return i * 2;
        }
    }

    // Demonstrates avoided allocation vs closure variable capture
    public static IEnumerable<int> FilterWithoutClosure(IEnumerable<int> source, int threshold)
    {
        // Pure static or argument-based filter to prevent heap allocated closure class
        return source.Where(x => x > threshold);
    }
}
