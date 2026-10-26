namespace LinqSamples.Benchmarks;

/// <summary>
/// The cost of LINQ's abstraction (delegates, iterators) against hand-written loops
/// for a simple pipeline: the sum of squares of even numbers.
/// </summary>
[MemoryDiagnoser]
public class LinqVsLoopBenchmarks
{
    private int[] _data = [];

    [Params(1_000, 100_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _data = Enumerable.Range(0, N).Select(_ => random.Next(1_000)).ToArray();
    }

    [Benchmark(Baseline = true)]
    public long Linq() =>
        _data.Where(x => x % 2 == 0).Select(x => (long)x * x).Sum();

    [Benchmark]
    public long Foreach()
    {
        long sum = 0;
        foreach (var x in _data)
        {
            if (x % 2 == 0)
            {
                sum += (long)x * x;
            }
        }

        return sum;
    }

    [Benchmark]
    public long For()
    {
        long sum = 0;
        for (var i = 0; i < _data.Length; i++)
        {
            var x = _data[i];
            if (x % 2 == 0)
            {
                sum += (long)x * x;
            }
        }

        return sum;
    }
}
