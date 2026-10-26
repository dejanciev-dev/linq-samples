namespace LinqSamples.Benchmarks;

/// <summary>
/// .NET 10's LeftJoin against the classic GroupJoin + SelectMany + DefaultIfEmpty pattern.
/// </summary>
[MemoryDiagnoser]
public class JoinBenchmarks
{
    public sealed record Customer(int Id, string Name);

    public sealed record Order(int Id, int CustomerId, decimal Total);

    private Customer[] _customers = [];
    private Order[] _orders = [];

    [Params(1_000, 10_000)]
    public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _customers = Enumerable.Range(0, N).Select(i => new Customer(i, $"Customer {i}")).ToArray();
        // About a third of customers get no orders, so the "left" side matters.
        _orders = Enumerable.Range(0, N * 2)
                            .Select(i => new Order(i, random.Next(N * 2 / 3), random.Next(1, 500)))
                            .ToArray();
    }

    [Benchmark(Baseline = true)]
    public int GroupJoin_SelectMany() =>
        _customers.GroupJoin(_orders, c => c.Id, o => o.CustomerId, (c, orders) => (c, orders))
                  .SelectMany(x => x.orders.DefaultIfEmpty(), (x, o) => (x.c.Name, Total: o?.Total ?? 0))
                  .Count();

    [Benchmark]
    public int LeftJoin() =>
        _customers.LeftJoin(_orders, c => c.Id, o => o.CustomerId, (c, o) => (c.Name, Total: o?.Total ?? 0))
                  .Count();
}
