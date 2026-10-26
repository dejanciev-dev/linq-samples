namespace LinqSamples.Benchmarks;

/// <summary>
/// Finding the most expensive product. OrderBy(...).First() is optimized by the runtime
/// into a single pass, but materializing the sort first (ToList) pays for a full O(n log n) sort.
/// </summary>
[MemoryDiagnoser]
public class TopOneBenchmarks
{
    public sealed record Item(int Id, double Price);

    private Item[] _items = [];

    [Params(1_000, 100_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _items = Enumerable.Range(0, N).Select(i => new Item(i, random.NextDouble() * 1_000)).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int MaxBy() => _items.MaxBy(i => i.Price)!.Id;

    [Benchmark]
    public int OrderByDescending_First() => _items.OrderByDescending(i => i.Price).First().Id;

    [Benchmark]
    public int OrderByDescending_ToList_First() => _items.OrderByDescending(i => i.Price).ToList()[0].Id;

    [Benchmark]
    public int Loop()
    {
        var best = _items[0];
        foreach (var item in _items)
        {
            if (item.Price > best.Price)
            {
                best = item;
            }
        }

        return best.Id;
    }
}
