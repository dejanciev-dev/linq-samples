namespace LinqSamples.Benchmarks;

/// <summary>
/// Counting occurrences per key: .NET 9's CountBy skips building the groups
/// that GroupBy(...).Select(g => g.Count()) has to create.
/// </summary>
[MemoryDiagnoser]
public class CountByBenchmarks
{
    private int[] _data = [];

    [Params(1_000, 100_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _data = Enumerable.Range(0, N).Select(_ => random.Next(100)).ToArray();
    }

    // Each benchmark returns a checksum so the results can be compared in tests.
    [Benchmark(Baseline = true)]
    public long GroupBy_Count() =>
        _data.GroupBy(x => x).Select(g => (long)g.Key * g.Count()).Sum();

    [Benchmark]
    public long CountBy() =>
        _data.CountBy(x => x).Select(kv => (long)kv.Key * kv.Value).Sum();

    [Benchmark]
    public long Dictionary_Loop()
    {
        var counts = new Dictionary<int, int>();
        foreach (var x in _data)
        {
            counts[x] = counts.GetValueOrDefault(x) + 1;
        }

        return counts.Sum(kv => (long)kv.Key * kv.Value);
    }
}
