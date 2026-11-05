namespace LinqSamples.Samples;

/// <summary>Shopping cart totals, discounts and promotions with LINQ.</summary>
[SampleCategory("Shopping cart", 46)]
public static class CartSamples
{
    private sealed record Line(string Sku, string Category, decimal Price, int Qty);

    private static readonly Line[] Cart =
    [
        new("pen", "office", 1.50m, 10),
        new("book", "media", 12.00m, 2),
        new("lamp", "home", 30.00m, 1),
        new("pad", "office", 4.00m, 5),
        new("disc", "media", 9.00m, 3),
    ];

    [Sample("Cart subtotal", Description = "Sum of price times quantity.")]
    public static decimal Subtotal() => Cart.Sum(l => l.Price * l.Qty);

    [Sample("Item count", Description = "Sum of quantities, not number of lines.")]
    public static int ItemCount() => Cart.Sum(l => l.Qty);

    [Sample("Subtotal per category", Description = "GroupBy category and sum line totals.")]
    public static IEnumerable<string> PerCategory() =>
        Cart.GroupBy(l => l.Category).OrderBy(g => g.Key)
            .Select(g => $"{g.Key}:{g.Sum(l => l.Price * l.Qty)}");

    [Sample("Most expensive line", Description = "MaxBy on the line total.")]
    public static string BiggestLine() => Cart.MaxBy(l => l.Price * l.Qty)!.Sku;

    [Sample("Discount on one category", Description = "10% off media lines only, via a conditional in Sum.")]
    public static decimal MediaDiscount() =>
        Cart.Sum(l => l.Category == "media" ? l.Price * l.Qty * 0.10m : 0m);

    [Sample("Buy 3 get 1 free", Description = "Free units are Qty / 4 per line; subtract their value.")]
    public static decimal BuyThreeGetOne() =>
        Cart.Sum(l => l.Price * (l.Qty - l.Qty / 4));

    [Sample("Free shipping check", Description = "All with a threshold on the subtotal.")]
    public static bool FreeShipping() => Cart.Sum(l => l.Price * l.Qty) >= 100m;

    [Sample("Cheapest unit per category", Description = "GroupBy then MinBy on price.")]
    public static IEnumerable<string> CheapestPerCategory() =>
        Cart.GroupBy(l => l.Category).OrderBy(g => g.Key).Select(g => g.MinBy(l => l.Price)!.Sku);

    [Sample("Bulk lines", Description = "Where Qty >= 5, ordered by SKU.")]
    public static IEnumerable<string> BulkLines() =>
        Cart.Where(l => l.Qty >= 5).Select(l => l.Sku).Order();

    [Sample("Expand to units", Description = "SelectMany with Repeat turns quantities into single units.")]
    public static int UnitCount() => Cart.SelectMany(l => Enumerable.Repeat(l.Sku, l.Qty)).Count();
}
