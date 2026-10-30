using System.Collections.Concurrent;

namespace LinqSamples.Samples;

/// <summary>
/// PLINQ: <c>AsParallel()</c> spreads a query across cores. It pays off for CPU-heavy work on
/// large inputs; results come back unordered unless you ask for <c>AsOrdered()</c> or sort.
/// </summary>
[SampleCategory("Parallel LINQ (PLINQ)", 20)]
public static class ParallelSamples
{
    [Sample("AsParallel", Description = "The rest of the query runs on several threads.")]
    public static int AsParallel() =>
        Enumerable.Range(1, 1000).AsParallel().Count(n => n % 7 == 0);

    [Sample("Order is not guaranteed", Description = "Same elements as the sequential query, but possibly in another order.")]
    public static IEnumerable<string> OrderNotGuaranteed()
    {
        var sequential = Enumerable.Range(1, 1000).Select(n => n * 2).ToList();
        var parallel = Enumerable.Range(1, 1000).AsParallel().Select(n => n * 2).ToList();
        return
        [
            $"Same elements: {parallel.Order().SequenceEqual(sequential)}",
            "Same order:    not guaranteed - use AsOrdered() or sort",
        ];
    }

    [Sample("AsOrdered", Description = "Keep the source order, at some cost in speed.")]
    public static IEnumerable<int> AsOrdered() =>
        Enumerable.Range(1, 20).AsParallel().AsOrdered().Select(n => n * n).Take(5);

    [Sample("Parallel Sum", Description = "Each thread sums a partition, then the partial sums are combined.")]
    public static long Sum() =>
        Enumerable.Range(1, 1_000_000).AsParallel().Sum(n => (long)n);

    [Sample("Parallel Aggregate", Description = "Per-thread accumulators plus a combine step - min and max in one pass.")]
    public static string Aggregate() =>
        Enumerable.Range(1, 100).AsParallel().Aggregate(
            seedFactory: () => (Min: int.MaxValue, Max: int.MinValue),
            updateAccumulatorFunc: (acc, n) => (Math.Min(acc.Min, n), Math.Max(acc.Max, n)),
            combineAccumulatorsFunc: (a, b) => (Math.Min(a.Min, b.Min), Math.Max(a.Max, b.Max)),
            resultSelector: acc => $"Min: {acc.Min}, Max: {acc.Max}");

    [Sample("WithDegreeOfParallelism", Description = "Cap how many threads the query may use.")]
    public static string DegreeOfParallelism()
    {
        var threads = Enumerable.Range(1, 200)
                                .AsParallel()
                                .WithDegreeOfParallelism(2)
                                .Select(_ => Environment.CurrentManagedThreadId)
                                .Distinct()
                                .Count();
        return $"At most 2 threads: {threads <= 2}";
    }

    [Sample("ForAll", Description = "Process results in parallel without merging them back to one thread.")]
    public static IEnumerable<int> ForAll()
    {
        var squares = new ConcurrentBag<int>();
        Enumerable.Range(1, 10).AsParallel().ForAll(n => squares.Add(n * n));
        return squares.Order();
    }

    [Sample("Exceptions are aggregated", Description = "Errors from worker threads arrive wrapped in an AggregateException.")]
    public static string Exceptions()
    {
        try
        {
            _ = Enumerable.Range(1, 10)
                          .AsParallel()
                          .Select(n => n == 5 ? throw new InvalidOperationException("bad item 5") : n)
                          .ToList();
            return "no exception";
        }
        catch (AggregateException ex)
        {
            return $"{ex.GetType().Name}: {string.Join(", ", ex.InnerExceptions.Select(e => e.Message).Distinct())}";
        }
    }

    [Sample("WithCancellation", Description = "A cancelled token stops the query with OperationCanceledException.")]
    public static string Cancellation()
    {
        using var cts = new CancellationTokenSource();
        cts.Cancel();
        try
        {
            return $"Count: {Enumerable.Range(1, 1000).AsParallel().WithCancellation(cts.Token).Count()}";
        }
        catch (OperationCanceledException ex)
        {
            return $"Cancelled: {ex.GetType().Name}";
        }
    }

    [Sample("AsSequential", Description = "Run the heavy part in parallel, then switch back for order-sensitive steps.")]
    public static IEnumerable<string> AsSequential() =>
        Enumerable.Range(1, 10)
                  .AsParallel()
                  .AsOrdered()
                  .Where(n => n % 2 == 0)
                  .AsSequential()
                  .Select((n, i) => $"{i}: {n}");

    [Sample("ForceParallelism", Description = "PLINQ may decide to run sequentially; WithExecutionMode overrides that.")]
    public static IEnumerable<int> ForceParallelism() =>
        Enumerable.Range(1, 100)
                  .AsParallel()
                  .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                  .Where(n => n % 10 == 0)
                  .OrderBy(n => n);

    [Sample("Parallel GroupBy", Description = "Group in parallel, then sort for a stable result.")]
    public static IEnumerable<string> GroupBy() =>
        Orders.AsParallel()
              .GroupBy(o => o.Status)
              .Select(g => $"{g.Key}: {g.Count()}")
              .OrderBy(s => s, StringComparer.Ordinal);

    [Sample("WithMergeOptions", Description = "NotBuffered hands each result to the consumer as soon as it is ready.")]
    public static IEnumerable<int> MergeOptions() =>
        Enumerable.Range(1, 10)
                  .AsParallel()
                  .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                  .Select(n => n * 10)
                  .AsSequential()
                  .OrderBy(n => n);

    [Sample("CPU-bound work", Description = "Where PLINQ shines: an expensive test on many items - primes below 100,000.")]
    public static int CountPrimes() =>
        Enumerable.Range(2, 99_999).AsParallel().Count(IsPrime);

    private static bool IsPrime(int n)
    {
        for (var d = 2; d * d <= n; d++)
        {
            if (n % d == 0)
            {
                return false;
            }
        }

        return true;
    }
}
