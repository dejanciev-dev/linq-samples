namespace LinqSamples.Samples;

/// <summary>Bakery sales: items, days, quantities and unit prices.</summary>
[SampleCategory("Bakery sales", 66)]
public static class BakerySamples
{
    private sealed record Sale(string Item, string Day, int Qty, int Price);

    private static readonly Sale[] Sales =
    [
        new("Bread", "Mon", 10, 3), new("Croissant", "Mon", 6, 2), new("Bread", "Tue", 8, 3),
        new("Cake", "Tue", 2, 15), new("Croissant", "Wed", 14, 2), new("Muffin", "Wed", 5, 2),
    ];

    [Sample("Total revenue", Description = "Sum of quantity times price.")]
    public static int TotalRevenue() => Sales.Sum(s => s.Qty * s.Price);

    [Sample("Revenue per day", Description = "GroupBy keeps first-seen order of days.")]
    public static IEnumerable<string> RevenuePerDay() => Sales.GroupBy(s => s.Day).Select(g => $"{g.Key}:{g.Sum(s => s.Qty * s.Price)}");

    [Sample("Units per item", Description = "Group by item and sum quantity.")]
    public static IEnumerable<string> UnitsPerItem() =>
        Sales.GroupBy(s => s.Item).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(s => s.Qty)}");

    [Sample("Best seller", Description = "Item with the most units sold.")]
    public static string BestSeller() => Sales.GroupBy(s => s.Item).MaxBy(g => g.Sum(s => s.Qty))!.Key;

    [Sample("Top revenue item", Description = "Item with the highest revenue.")]
    public static string TopRevenueItem() => Sales.GroupBy(s => s.Item).MaxBy(g => g.Sum(s => s.Qty * s.Price))!.Key;

    [Sample("Sold on several days", Description = "Items appearing on more than one day.")]
    public static IEnumerable<string> MultiDayItems() =>
        Sales.GroupBy(s => s.Item).Where(g => g.Select(s => s.Day).Distinct().Count() > 1).Select(g => g.Key).Order();

    [Sample("Average price", Description = "Average unit price across sales.")]
    public static double AveragePrice() => Sales.Average(s => s.Price);

    [Sample("Premium items", Description = "Distinct items priced 10 or more.")]
    public static IEnumerable<string> PremiumItems() => Sales.Where(s => s.Price >= 10).Select(s => s.Item).Distinct();

    [Sample("Big orders", Description = "Count sales of 10 units or more.")]
    public static int BigOrders() => Sales.Count(s => s.Qty >= 10);

    [Sample("Running revenue", Description = "Cumulative revenue day by day.")]
    public static IEnumerable<int> RunningRevenue()
    {
        var daily = Sales.GroupBy(s => s.Day).Select(g => g.Sum(s => s.Qty * s.Price)).ToArray();
        return daily.Select((_, i) => daily.Take(i + 1).Sum());
    }
}
