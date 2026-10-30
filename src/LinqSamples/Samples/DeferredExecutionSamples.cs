namespace LinqSamples.Samples;

/// <summary>
/// Most LINQ operators are lazy: they describe a query but run only when enumerated.
/// Understanding this avoids the most common LINQ bugs and performance traps.
/// </summary>
[SampleCategory("Deferred execution", 12)]
public static class DeferredExecutionSamples
{
    [Sample("Queries see later changes", Description = "The query runs at enumeration time, not at definition time.")]
    public static IEnumerable<string> SeesLaterChanges()
    {
        List<int> source = [1, 2, 3];
        var query = source.Where(n => n > 1);
        source.Add(4);
        return [$"Count: {query.Count()}"]; // 3, not 2
    }

    [Sample("ToList snapshots the result", Description = "Materialize once to freeze the result.")]
    public static IEnumerable<string> Snapshot()
    {
        List<int> source = [1, 2, 3];
        var snapshot = source.Where(n => n > 1).ToList();
        source.Add(4);
        return [$"Count: {snapshot.Count}"]; // 2
    }

    [Sample("Captured variables are read late", Description = "A lambda captures the variable, not its value at definition time.")]
    public static IEnumerable<string> CapturedVariables()
    {
        var threshold = 5;
        var query = Numbers.Where(n => n > threshold);
        threshold = 8;
        return [$"Count: {query.Count()}"]; // 2 (9, 9), not 5
    }

    [Sample("Exceptions surface at enumeration", Description = "A faulty query fails where it is consumed, not where it is written.")]
    public static IEnumerable<string> DeferredExceptions()
    {
        int[] divisors = [1, 0];
        var query = divisors.Select(d => 10 / d);
        string Enumerate()
        {
            try
            {
                return string.Join(",", query);
            }
            catch (DivideByZeroException)
            {
                return "throws DivideByZeroException";
            }
        }

        return ["Defined:    no exception yet", $"Enumerated: {Enumerate()}"];
    }

    [Sample("Multiple enumeration", Description = "Every enumeration re-runs the whole pipeline.")]
    public static IEnumerable<string> MultipleEnumeration()
    {
        var evaluations = 0;
        var query = Numbers.Select(n => { evaluations++; return n * 2; });

        _ = query.Count();
        _ = query.Sum();
        var lazy = evaluations;

        evaluations = 0;
        var list = query.ToList();
        _ = list.Count;
        _ = list.Sum();

        return [$"Lazy: {lazy} evaluations", $"Materialized: {evaluations} evaluations"];
    }

    [Sample("Short-circuiting", Description = "First/Any/Take stop pulling items as soon as they can.")]
    public static IEnumerable<string> ShortCircuit()
    {
        List<int> visited = [];
        var firstBig = Numbers.Where(n => { visited.Add(n); return n > 6; }).First();
        return [$"First > 6: {firstBig}", $"Visited: {string.Join(",", visited)}"];
    }

    [Sample("Infinite sequences", Description = "Laziness makes infinite generators usable with Take.")]
    public static IEnumerable<long> Fibonacci() => FibonacciNumbers().Take(10);

    private static IEnumerable<long> FibonacciNumbers()
    {
        (long a, long b) = (0, 1);
        while (true)
        {
            yield return a;
            (a, b) = (b, a + b);
        }
    }
}
