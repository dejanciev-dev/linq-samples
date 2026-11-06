namespace LinqSamples.Samples;

/// <summary>Customer orders, payments and running totals.</summary>
[SampleCategory("Customer orders", 53)]
public static class OrderSamples
{
    private sealed record Order(int Id, string Customer, decimal Total, bool Paid);

    private static readonly Order[] Orders =
    [
        new(1, "ann", 50m, true), new(2, "bob", 20m, false), new(3, "ann", 30m, true),
        new(4, "cy", 70m, true), new(5, "bob", 40m, true), new(6, "cy", 10m, false),
    ];

    [Sample("Paid revenue", Description = "Sum of paid orders.")]
    public static decimal PaidRevenue() => Orders.Where(o => o.Paid).Sum(o => o.Total);

    [Sample("Unpaid order ids", Description = "Filter on the flag.")]
    public static IEnumerable<int> Unpaid() => Orders.Where(o => !o.Paid).Select(o => o.Id);

    [Sample("Total per customer", Description = "GroupBy customer and sum.")]
    public static IEnumerable<string> PerCustomer() =>
        Orders.GroupBy(o => o.Customer).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(o => o.Total)}");

    [Sample("Best paying customer", Description = "Group paid orders, MaxBy the sum.")]
    public static string BestCustomer() =>
        Orders.Where(o => o.Paid).GroupBy(o => o.Customer).MaxBy(g => g.Sum(o => o.Total))!.Key;

    [Sample("Largest order", Description = "MaxBy total.")]
    public static int Largest() => Orders.MaxBy(o => o.Total)!.Id;

    [Sample("Average paid order", Description = "Average over paid orders.")]
    public static decimal AveragePaid() => Orders.Where(o => o.Paid).Average(o => o.Total);

    [Sample("Customers with unpaid orders", Description = "Any inside a group.")]
    public static IEnumerable<string> WithUnpaid() => Orders.GroupBy(o => o.Customer).Where(g => g.Any(o => !o.Paid)).Select(g => g.Key).Order();

    [Sample("Customers who always pay", Description = "All inside a group.")]
    public static IEnumerable<string> AlwaysPaid() => Orders.GroupBy(o => o.Customer).Where(g => g.All(o => o.Paid)).Select(g => g.Key).Order();

    [Sample("Running total", Description = "Sum of the first N orders for each N.")]
    public static IEnumerable<decimal> RunningTotal() => Enumerable.Range(1, Orders.Length).Select(n => Orders.Take(n).Sum(o => o.Total));

    [Sample("Customers over 75", Description = "Filter groups on their total.")]
    public static IEnumerable<string> BigSpenders() => Orders.GroupBy(o => o.Customer).Where(g => g.Sum(o => o.Total) > 75).Select(g => g.Key).Order();
}
