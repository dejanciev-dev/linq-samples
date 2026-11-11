namespace LinqSamples.Samples;

/// <summary>Coffee shop orders: drinks, sizes, prices and hour of day.</summary>
[SampleCategory("Coffee shop", 71)]
public static class CoffeeSamples
{
    private sealed record Order(string Drink, string Size, int Price, int Hour);

    private static readonly Order[] Orders =
    [
        new("Latte", "L", 5, 8), new("Espresso", "S", 2, 8), new("Latte", "M", 4, 9),
        new("Mocha", "L", 6, 12), new("Espresso", "S", 2, 15), new("Tea", "M", 5, 15),
    ];

    [Sample("Total revenue", Description = "Sum of all order prices.")]
    public static int TotalRevenue() => Orders.Sum(o => o.Price);

    [Sample("Orders per drink", Description = "CountBy drink.")]
    public static IEnumerable<string> OrdersPerDrink() => Orders.CountBy(o => o.Drink).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Revenue per hour", Description = "AggregateBy hour summing prices.")]
    public static IEnumerable<string> RevenuePerHour() =>
        Orders.AggregateBy(o => o.Hour, 0, (sum, o) => sum + o.Price).OrderBy(p => p.Key).Select(p => $"{p.Key}:{p.Value}");

    [Sample("Morning orders", Description = "Count orders before noon.")]
    public static int MorningOrders() => Orders.Count(o => o.Hour < 12);

    [Sample("Priciest drink", Description = "MaxBy price.")]
    public static string PriciestDrink() => Orders.MaxBy(o => o.Price)!.Drink;

    [Sample("Sizes", Description = "Distinct cup sizes.")]
    public static IEnumerable<string> Sizes() => Orders.Select(o => o.Size).Distinct().Order();

    [Sample("Average price", Description = "Average order price.")]
    public static double AveragePrice() => Orders.Average(o => o.Price);

    [Sample("Busiest hour", Description = "CountBy hour, then MaxBy count (first wins on ties).")]
    public static int BusiestHour() => Orders.CountBy(o => o.Hour).MaxBy(c => c.Value).Key;

    [Sample("Drinks over 4", Description = "Distinct drinks priced above 4.")]
    public static IEnumerable<string> DrinksOver4() => Orders.Where(o => o.Price > 4).Select(o => o.Drink).Distinct().Order();

    [Sample("Revenue per drink", Description = "Order by revenue descending.")]
    public static IEnumerable<string> RevenuePerDrink() =>
        Orders.GroupBy(o => o.Drink).Select(g => (Drink: g.Key, Total: g.Sum(o => o.Price)))
            .OrderByDescending(x => x.Total).ThenBy(x => x.Drink).Select(x => $"{x.Drink}:{x.Total}");
}
