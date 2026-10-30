using System.Runtime.CompilerServices;

namespace LinqSamples.Samples;

/// <summary>
/// LINQ over <c>IAsyncEnumerable&lt;T&gt;</c>. Since .NET 10 the operators ship in the framework
/// (System.Linq.AsyncEnumerable), including async overloads that take
/// <c>(item, CancellationToken) =&gt; ValueTask&lt;T&gt;</c> lambdas.
/// </summary>
[SampleCategory("Async LINQ (IAsyncEnumerable)", 19)]
public static class AsyncSamples
{
    [Sample("await foreach", Description = "Consume a stream item by item as it arrives.")]
    public static async Task<List<int>> AwaitForeach()
    {
        List<int> ids = [];
        await foreach (var order in StreamOrders())
        {
            ids.Add(order.Id);
        }

        return ids;
    }

    [Sample("Where / Select on a stream", Description = "The familiar operators, applied lazily to an async source.")]
    public static IAsyncEnumerable<string> WhereSelect() =>
        StreamOrders().Where(o => o.Status == "Completed").Select(o => $"#{o.Id}");

    [Sample("Async predicate", Description = "Filter with an awaited check - orders whose product is in stock.")]
    public static IAsyncEnumerable<int> AsyncPredicate() =>
        StreamOrders().Where(async (o, ct) => await IsInStockAsync(o.ProductId, ct))
                      .Select(o => o.Id);

    [Sample("Async projection", Description = "Select with an awaited lookup per element.")]
    public static IAsyncEnumerable<string> AsyncProjection() =>
        StreamOrders().Take(3)
                      .Select(async (o, ct) => $"#{o.Id} {await GetCustomerNameAsync(o.CustomerId, ct)}");

    [Sample("CountAsync / SumAsync / AnyAsync", Description = "Aggregates end the stream and return a single awaited value.")]
    public static async Task<IEnumerable<string>> Aggregates() =>
    [
        $"Count:          {await StreamOrders().CountAsync()}",
        $"Total quantity: {await StreamOrders().Select(o => o.Quantity).SumAsync()}",
        $"Any declined?   {await StreamOrders().AnyAsync(o => o.Status == "Declined")}",
    ];

    [Sample("FirstAsync / FirstOrDefaultAsync", Description = "Stop at the first match; the OrDefault form returns null when nothing matches.")]
    public static async Task<IEnumerable<string>> FirstAsync() =>
    [
        $"First pending: #{(await StreamOrders().FirstAsync(o => o.Status == "Pending")).Id}",
        $"First lost:    {(await StreamOrders().FirstOrDefaultAsync(o => o.Status == "Lost"))?.Id.ToString() ?? "(none)"}",
    ];

    [Sample("Take stops the stream", Description = "The producer is asked for only as many items as the query needs.")]
    public static async Task<string> TakeStopsTheStream()
    {
        var produced = 0;
        async IAsyncEnumerable<int> Produce()
        {
            for (var i = 1; i <= 100; i++)
            {
                await Task.Yield();
                produced++;
                yield return i;
            }
        }

        var taken = await Produce().Take(2).ToListAsync();
        return $"Took {taken.Count}, produced {produced}";
    }

    [Sample("Cancellation", Description = "WithCancellation flows a token into the producer; cancelling ends the loop with an exception.")]
    public static async Task<IEnumerable<string>> Cancellation()
    {
        using var cts = new CancellationTokenSource();
        var received = 0;
        try
        {
            await foreach (var _ in StreamOrders().WithCancellation(cts.Token))
            {
                if (++received == 3)
                {
                    await cts.CancelAsync();
                }
            }

            return [$"Received: {received}", "Completed without cancellation"];
        }
        catch (OperationCanceledException ex)
        {
            return [$"Received: {received}", $"Stopped:  {ex.GetType().Name}"];
        }
    }

    [Sample("GroupBy on a stream", Description = "Grouping has to read the whole stream before it yields the first group.")]
    public static IAsyncEnumerable<string> GroupBy() =>
        StreamOrders().GroupBy(o => o.Status).Select(g => $"{g.Key}: {g.Count()}");

    [Sample("Chunk a stream", Description = "Batch items for bulk processing as they arrive.")]
    public static IAsyncEnumerable<string> Chunk() =>
        StreamOrders().Select(o => o.Id).Chunk(4).Select(batch => string.Join(",", batch));

    [Sample("ToAsyncEnumerable", Description = "Adapt an in-memory sequence to APIs that expect a stream.")]
    public static IAsyncEnumerable<int> ToAsyncEnumerable() =>
        Numbers.ToAsyncEnumerable().Where(n => n > 5);

    [Sample("AsyncEnumerable.Range", Description = "Generate an async sequence directly.")]
    public static IAsyncEnumerable<int> Range() =>
        AsyncEnumerable.Range(1, 5).Select(n => n * n);

    [Sample("Zip two streams", Description = "Pair items from two async sources by position.")]
    public static IAsyncEnumerable<string> Zip() =>
        AsyncEnumerable.Range(1, 3)
                       .Zip(new[] { "gold", "silver", "bronze" }.ToAsyncEnumerable())
                       .Select(t => $"{t.First}. {t.Second}");

    [Sample("Distinct on a stream", Description = "Set operators work on streams too, in first-seen order.")]
    public static IAsyncEnumerable<string> Distinct() =>
        StreamOrders().Select(o => o.Status).Distinct();

    [Sample("Select + Task.WhenAll", Description = "Start every async call at once, then await them all - results keep the source order.")]
    public static async Task<IEnumerable<string>> WhenAll()
    {
        var counts = await Task.WhenAll(Customers.Select(c => CountOrdersAsync(c.Id)));
        return Customers.Zip(counts, (c, count) => $"{c.Name}: {count}");
    }

    [Sample("Throttle with Chunk", Description = "Limit concurrency: run one batch of async calls at a time.")]
    public static async Task<IEnumerable<string>> Throttle()
    {
        List<string> results = [];
        foreach (var (batch, index) in Customers.Chunk(2).Select((batch, index) => (batch, index)))
        {
            var counts = await Task.WhenAll(batch.Select(c => CountOrdersAsync(c.Id)));
            results.Add($"Batch {index + 1}: {string.Join(", ", batch.Zip(counts, (c, n) => $"{c.Name}={n}"))}");
        }

        return results;
    }

    /// <summary>Simulates a remote source, such as a paged API or a database cursor.</summary>
    private static async IAsyncEnumerable<Order> StreamOrders([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var order in Orders)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
            yield return order;
        }
    }

    private static async ValueTask<bool> IsInStockAsync(int productId, CancellationToken cancellationToken)
    {
        await Task.Yield();
        cancellationToken.ThrowIfCancellationRequested();
        return productId != 1; // laptops are sold out
    }

    private static async ValueTask<string> GetCustomerNameAsync(int customerId, CancellationToken cancellationToken)
    {
        await Task.Yield();
        cancellationToken.ThrowIfCancellationRequested();
        return Customers.FirstOrDefault(c => c.Id == customerId)?.Name ?? "(guest)";
    }

    private static async Task<int> CountOrdersAsync(int customerId)
    {
        await Task.Yield();
        return Orders.Count(o => o.CustomerId == customerId);
    }
}
