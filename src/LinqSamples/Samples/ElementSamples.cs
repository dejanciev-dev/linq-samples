namespace LinqSamples.Samples;

[SampleCategory("Elements & quantifiers", 9)]
public static class ElementSamples
{
    [Sample("First / Last")]
    public static IEnumerable<string> FirstLast() =>
        [Products.First().Name, Products.Last(p => p.Category == "Furniture").Name];

    [Sample("FirstOrDefault with a default (.NET 6+)", Description = "Supply your own fallback instead of null.")]
    public static string FirstOrDefaultWithDefault() =>
        Customers.FirstOrDefault(c => c.City == "Paris", new Customer(0, "Nobody", "-")).Name;

    [Sample("Single", Description = "Exactly one match, otherwise an exception.")]
    public static string Single() =>
        Customers.Single(c => c.Id == 3).Name;

    [Sample("ElementAt with Index (.NET 6+)")]
    public static int ElementAtFromEnd() => Numbers.ElementAt(^1);

    [Sample("Any / All / Contains")]
    public static IEnumerable<string> Quantifiers() =>
    [
        $"Any order declined?      {Orders.Any(o => o.Status == "Declined")}",
        $"All products priced > 0? {Products.All(p => p.Price > 0)}",
        $"Numbers contain 7?       {Numbers.Contains(7)}",
    ];

    [Sample("DefaultIfEmpty")]
    public static IEnumerable<int> DefaultIfEmpty() =>
        Numbers.Where(n => n > 100).DefaultIfEmpty(-1);

    [Sample("TryGetNonEnumeratedCount (.NET 6+)", Description = "Get a count cheaply, but only when no enumeration is required.")]
    public static IEnumerable<string> TryGetNonEnumeratedCount()
    {
        var list = Numbers.ToList();
        var lazy = Numbers.Where(n => n > 3);
        return
        [
            $"List:  {(list.TryGetNonEnumeratedCount(out var c1) ? c1.ToString() : "unknown")}",
            $"Where: {(lazy.TryGetNonEnumeratedCount(out var c2) ? c2.ToString() : "unknown")}",
        ];
    }
}
