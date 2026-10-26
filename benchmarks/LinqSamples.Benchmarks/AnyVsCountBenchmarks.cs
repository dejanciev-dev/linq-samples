namespace LinqSamples.Benchmarks;

/// <summary>
/// "Is there at least one match?" - Any() stops at the first match,
/// Count() has to walk the entire sequence.
/// </summary>
[MemoryDiagnoser]
public class AnyVsCountBenchmarks
{
    private int[] _data = [];

    [Params(1_000, 100_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup() => _data = Enumerable.Range(0, N).ToArray();

    // The match sits near the start, like most "does any exist?" checks in practice.
    [Benchmark(Baseline = true)]
    public bool Any_WithPredicate() => _data.Any(x => x == 10);

    [Benchmark]
    public bool Count_WithPredicate_GreaterThanZero() => _data.Count(x => x == 10) > 0;

    [Benchmark]
    public bool Where_Count_GreaterThanZero() => _data.Where(x => x == 10).Count() > 0;
}
