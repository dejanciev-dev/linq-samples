namespace LinqSamples.Samples;

/// <summary>Warehouse stock movements: balances, running totals and cross-group questions.</summary>
[SampleCategory("Stock movements", 80)]
public static class StockSamples
{
    private sealed record Move(string Sku, string Warehouse, int Qty, int Day);

    private static readonly Move[] Moves =
    [
        new("A", "W1", 10, 1), new("A", "W1", -3, 2), new("A", "W2", 5, 2),
        new("B", "W1", 7, 1), new("B", "W2", 4, 3), new("B", "W1", -2, 3),
        new("C", "W2", 9, 1), new("C", "W2", -9, 4), new("A", "W1", 6, 5),
    ];

    private static List<int> RunningTotal(IEnumerable<int> values) =>
        values.Aggregate(new List<int>(), (acc, v) =>
        {
            acc.Add((acc.Count > 0 ? acc[^1] : 0) + v);
            return acc;
        });

    [Sample("Stock per SKU", Description = "Signed quantities summed per group.")]
    public static IEnumerable<string> StockPerSku() =>
        Moves.GroupBy(m => m.Sku).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(m => m.Qty)}");

    [Sample("Stock per SKU and warehouse", Description = "Group by a tuple key.")]
    public static IEnumerable<string> StockPerSkuWarehouse() =>
        Moves.GroupBy(m => (m.Sku, m.Warehouse))
             .OrderBy(g => g.Key.Sku, StringComparer.Ordinal).ThenBy(g => g.Key.Warehouse, StringComparer.Ordinal)
             .Select(g => $"{g.Key.Sku}/{g.Key.Warehouse}:{g.Sum(m => m.Qty)}");

    [Sample("Sold-out SKUs", Description = "Keep groups whose balance is zero.")]
    public static IEnumerable<string> SoldOut() =>
        Moves.GroupBy(m => m.Sku).Where(g => g.Sum(m => m.Qty) == 0).Select(g => g.Key);

    [Sample("Running balance", Description = "Aggregate with a list accumulator builds a running total.")]
    public static IEnumerable<int> RunningBalance() =>
        RunningTotal(Moves.Where(m => m.Sku == "A" && m.Warehouse == "W1").OrderBy(m => m.Day).Select(m => m.Qty));

    [Sample("Busiest day", Description = "CountBy then MaxBy.")]
    public static int BusiestDay() => Moves.CountBy(m => m.Day).MaxBy(kv => kv.Value).Key;

    [Sample("Largest outflow per SKU", Description = "Filter negatives, group, take the minimum.")]
    public static IEnumerable<string> LargestOutflow() =>
        Moves.Where(m => m.Qty < 0).GroupBy(m => m.Sku).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Min(m => m.Qty)}");

    [Sample("SKUs in both warehouses", Description = "Distinct count inside each group.")]
    public static IEnumerable<string> InBothWarehouses() =>
        Moves.GroupBy(m => m.Sku).Where(g => g.Select(m => m.Warehouse).Distinct().Count() == 2).Select(g => g.Key).Order();

    [Sample("SKUs per warehouse", Description = "ToLookup with an element selector.")]
    public static IEnumerable<string> SkusPerWarehouse() =>
        Moves.ToLookup(m => m.Warehouse, m => m.Sku)
             .OrderBy(l => l.Key, StringComparer.Ordinal)
             .Select(l => $"{l.Key}:{string.Join(",", l.Distinct().Order())}");

    [Sample("Cumulative net per day", Description = "Group by day, sum, then running total.")]
    public static IEnumerable<int> CumulativeNetPerDay() =>
        RunningTotal(Moves.GroupBy(m => m.Day).OrderBy(g => g.Key).Select(g => g.Sum(m => m.Qty)));

    [Sample("Deltas between movements", Description = "Zip a sequence with itself shifted by one.")]
    public static IEnumerable<int> Deltas()
    {
        var qty = Moves.Where(m => m.Sku == "A").OrderBy(m => m.Day).Select(m => m.Qty).ToArray();
        return qty.Zip(qty.Skip(1), (a, b) => b - a);
    }
}
