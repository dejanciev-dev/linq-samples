namespace LinqSamples.Samples;

/// <summary>Pizza orders: customers, sizes, toppings, prices and delivery.</summary>
[SampleCategory("Pizza orders", 69)]
public static class PizzaSamples
{
    private sealed record Order(string Customer, string Size, int Toppings, int Price, bool Delivered);

    private static readonly Order[] Orders =
    [
        new("Ann", "L", 3, 14, true), new("Bob", "M", 1, 10, false), new("Ann", "S", 0, 7, false),
        new("Cid", "L", 2, 13, true), new("Dee", "M", 2, 11, true), new("Bob", "L", 4, 15, true),
    ];

    [Sample("Total revenue", Description = "Sum of all order prices.")]
    public static int TotalRevenue() => Orders.Sum(o => o.Price);

    [Sample("Orders per size", Description = "CountBy size.")]
    public static IEnumerable<string> OrdersPerSize() => Orders.CountBy(o => o.Size).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Delivered orders", Description = "Count orders that were delivered.")]
    public static int Delivered() => Orders.Count(o => o.Delivered);

    [Sample("Average toppings", Description = "Average number of toppings per pizza.")]
    public static double AverageToppings() => Orders.Average(o => o.Toppings);

    [Sample("Most toppings", Description = "MaxBy toppings.")]
    public static string MostToppings() => Orders.MaxBy(o => o.Toppings)!.Customer;

    [Sample("Plain pizza fans", Description = "Customers who ordered a pizza with no toppings.")]
    public static IEnumerable<string> PlainPizzaCustomers() => Orders.Where(o => o.Toppings == 0).Select(o => o.Customer).Distinct();

    [Sample("Spend per customer", Description = "Order by spend descending.")]
    public static IEnumerable<string> SpendPerCustomer() =>
        Orders.GroupBy(o => o.Customer).Select(g => (Name: g.Key, Spend: g.Sum(o => o.Price)))
            .OrderByDescending(x => x.Spend).ThenBy(x => x.Name).Select(x => $"{x.Name}:{x.Spend}");

    [Sample("Pickup customers", Description = "Customers who collected at least one order.")]
    public static IEnumerable<string> PickupCustomers() => Orders.Where(o => !o.Delivered).Select(o => o.Customer).Distinct().Order();

    [Sample("Any small delivered", Description = "Any with a compound condition.")]
    public static bool AnySmallDelivered() => Orders.Any(o => o.Size == "S" && o.Delivered);

    [Sample("Cheapest large", Description = "Min price among large pizzas.")]
    public static int CheapestLarge() => Orders.Where(o => o.Size == "L").Min(o => o.Price);
}
