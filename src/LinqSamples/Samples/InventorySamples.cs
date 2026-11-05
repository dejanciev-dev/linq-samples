namespace LinqSamples.Samples;

/// <summary>Stock levels, reorders and valuation with LINQ.</summary>
[SampleCategory("Inventory & stock", 44)]
public static class InventorySamples
{
    private static readonly (string Name, int Qty, decimal Price)[] Stock =
        [("bolt", 120, 0.10m), ("nut", 80, 0.05m), ("gear", 15, 2.50m), ("axle", 4, 12.00m)];

    [Sample("Total stock value", Description = "Sum of quantity times price.")]
    public static decimal TotalValue() => Stock.Sum(s => s.Qty * s.Price);

    [Sample("Low stock items", Description = "Where on quantity below a threshold.")]
    public static IEnumerable<string> LowStock() => Stock.Where(s => s.Qty < 20).Select(s => s.Name);

    [Sample("Most valuable line", Description = "MaxBy on quantity times price.")]
    public static string MostValuable() => Stock.MaxBy(s => s.Qty * s.Price).Name;

    [Sample("Reorder quantities", Description = "Top up low items to a target level of 20.")]
    public static IEnumerable<string> Reorder() => Stock.Where(s => s.Qty < 20).Select(s => $"{s.Name}:{20 - s.Qty}");

    [Sample("Items by value", Description = "OrderByDescending on line value.")]
    public static IEnumerable<string> ByValue() => Stock.OrderByDescending(s => s.Qty * s.Price).Select(s => s.Name);

    [Sample("Anything out of stock?", Description = "Any with a zero-quantity predicate.")]
    public static bool OutOfStock() => Stock.Any(s => s.Qty == 0);

    [Sample("Stock level buckets", Description = "GroupBy a computed label.")]
    public static IEnumerable<string> Buckets() =>
        Stock.GroupBy(s => s.Qty >= 100 ? "high" : s.Qty >= 20 ? "medium" : "low")
             .Select(g => $"{g.Key}:{string.Join(",", g.Select(s => s.Name))}");
}
