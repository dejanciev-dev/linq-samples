namespace LinqSamples.Samples;

[SampleCategory("Grouping", 5)]
public static class GroupingSamples
{
    [Sample("GroupBy", Description = "Group elements by a key.")]
    public static IEnumerable<string> GroupBy() =>
        Products.GroupBy(p => p.Category)
                .Select(g => $"{g.Key}: {string.Join(", ", g.Select(p => p.Name))}");

    [Sample("GroupBy with aggregates")]
    public static IEnumerable<(string Category, int Count, decimal Average)> GroupByAggregate() =>
        Products.GroupBy(p => p.Category)
                .Select(g => (g.Key, g.Count(), Math.Round(g.Average(p => p.Price), 2)));

    [Sample("GroupBy composite key", Description = "Use a tuple (or anonymous type) for multi-column keys.")]
    public static IEnumerable<string> GroupByCompositeKey() =>
        Orders.GroupBy(o => (o.Date.Year, o.Date.Month))
              .Select(g => $"{g.Key.Year}-{g.Key.Month:00}: {g.Count()} orders");

    [Sample("CountBy (.NET 9+)", Description = "Count occurrences per key without building groups.")]
    public static IEnumerable<KeyValuePair<string, int>> CountBy() =>
        Orders.CountBy(o => o.Status).OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key);

    [Sample("AggregateBy (.NET 9+)", Description = "Fold values per key - here: total quantity per customer.")]
    public static IEnumerable<KeyValuePair<int, int>> AggregateBy() =>
        Orders.AggregateBy(o => o.CustomerId, seed: 0, (total, o) => total + o.Quantity)
              .OrderBy(kv => kv.Key);

    [Sample("ToLookup", Description = "Immediately-evaluated, reusable grouping; a missing key returns an empty sequence.")]
    public static IEnumerable<string> ToLookup()
    {
        var byCity = Customers.ToLookup(c => c.City);
        return byCity["Berlin"].Select(c => c.Name)
                               .Concat(byCity["Paris"].Select(c => c.Name));
    }
}
