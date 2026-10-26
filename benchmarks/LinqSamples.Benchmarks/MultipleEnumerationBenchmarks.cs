namespace LinqSamples.Benchmarks;

/// <summary>
/// Using a lazy query twice runs the whole pipeline twice.
/// Materializing it once with ToList() trades an allocation for half the work.
/// </summary>
[MemoryDiagnoser]
public class MultipleEnumerationBenchmarks
{
    private int[] _data = [];

    [Params(1_000, 100_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup() => _data = Enumerable.Range(0, N).ToArray();

    private IEnumerable<double> Query() =>
        _data.Where(x => x % 3 == 0).Select(x => Math.Sqrt(x));

    [Benchmark(Baseline = true)]
    public double EnumerateTwice()
    {
        var query = Query();
        return query.Count() + query.Sum();
    }

    [Benchmark]
    public double MaterializeOnce()
    {
        var list = Query().ToList();
        return list.Count + list.Sum();
    }
}
