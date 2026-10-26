namespace LinqSamples.Benchmarks;

/// <summary>
/// Filtering by "is this id in that list?" - List.Contains inside Where is O(n * m),
/// a HashSet lookup makes it O(n + m).
/// </summary>
[MemoryDiagnoser]
public class ContainsBenchmarks
{
    private int[] _orderIds = [];
    private List<int> _wantedList = [];
    private HashSet<int> _wantedSet = [];

    [Params(1_000, 10_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _orderIds = Enumerable.Range(0, N).Select(_ => random.Next(N * 2)).ToArray();
        _wantedList = Enumerable.Range(0, N / 2).Select(_ => random.Next(N * 2)).ToList();
        _wantedSet = [.. _wantedList];
    }

    [Benchmark(Baseline = true)]
    public int Where_ListContains() => _orderIds.Where(id => _wantedList.Contains(id)).Count();

    [Benchmark]
    public int Where_HashSetContains() => _orderIds.Where(id => _wantedSet.Contains(id)).Count();

    [Benchmark]
    public int Where_ToHashSetInline() // building the set is included in the measurement
    {
        var set = _wantedList.ToHashSet();
        return _orderIds.Where(id => set.Contains(id)).Count();
    }
}
