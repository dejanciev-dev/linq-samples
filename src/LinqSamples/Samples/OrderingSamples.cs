namespace LinqSamples.Samples;

[SampleCategory("Ordering", 3)]
public static class OrderingSamples
{
    [Sample("OrderBy / OrderByDescending")]
    public static IEnumerable<string> OrderByPrice() =>
        Products.OrderByDescending(p => p.Price).Select(p => $"{p.Name} {p.Price}");

    [Sample("ThenBy", Description = "Secondary sort keys are applied only to ties of the previous key.")]
    public static IEnumerable<string> ThenBy() =>
        Products.OrderBy(p => p.Category)
                .ThenByDescending(p => p.Price)
                .Select(p => $"{p.Category} / {p.Name}");

    [Sample("Order (.NET 7+)", Description = "Sort by the element itself without a key selector.")]
    public static IEnumerable<int> Order() =>
        Numbers.Order();

    [Sample("Case-insensitive ordering", Description = "Pass a comparer to control how keys are compared.")]
    public static IEnumerable<string> CaseInsensitive() =>
        Words.Order(StringComparer.OrdinalIgnoreCase);

    [Sample("Order by specific values", Description = "Sort by a custom, business-defined sequence instead of alphabetically.")]
    public static IEnumerable<string> OrderBySpecificValues()
    {
        string[] preferences = ["Awaiting Payment", "Pending", "Declined", "Cancelled", "Shipped", "Completed"];
        return Orders.OrderBy(o => Array.IndexOf(preferences, o.Status))
                     .ThenBy(o => o.Id)
                     .Select(o => $"#{o.Id} {o.Status}");
    }

    [Sample("Order by specific values, unknown last", Description = "IndexOf returns -1 for unknown values; map that to the end.")]
    public static IEnumerable<string> OrderBySpecificValuesUnknownLast()
    {
        string[] preferences = ["Shipped", "Completed"];
        return Orders.OrderBy(o => Array.IndexOf(preferences, o.Status) is var i and >= 0 ? i : int.MaxValue)
                     .ThenBy(o => o.Id)
                     .Select(o => $"#{o.Id} {o.Status}");
    }

    [Sample("Reverse")]
    public static IEnumerable<int> Reverse() =>
        Numbers.Take(5).Reverse();

    [Sample("Order by a computed key", Description = "The three largest orders by line total.")]
    public static IEnumerable<string> OrderByComputedKey() =>
        Orders.OrderByDescending(o => o.Quantity * Products.First(p => p.Id == o.ProductId).Price)
              .ThenBy(o => o.Id)
              .Take(3)
              .Select(o => $"#{o.Id}: {o.Quantity * Products.First(p => p.Id == o.ProductId).Price}");

    [Sample("OrderBy is stable", Description = "Elements with equal keys keep their original relative order.")]
    public static IEnumerable<string> StableSort() =>
        Words.OrderBy(w => w.Length);

    [Sample("Nulls last", Description = "Sort on 'is null' first: false comes before true.")]
    public static IEnumerable<string> NullsLast() =>
        Employees.OrderBy(e => e.ManagerId is null)
                 .ThenBy(e => e.ManagerId)
                 .ThenBy(e => e.Name)
                 .Select(e => e.Name);

    [Sample("Natural sort", Description = "Order by length, then text, so file2 comes before file10 (for names sharing a prefix).")]
    public static IEnumerable<string> NaturalSort()
    {
        string[] files = ["file10.txt", "file2.txt", "file1.txt", "file20.txt"];
        return
        [
            $"Ordinal: {string.Join(", ", files.Order(StringComparer.Ordinal))}",
            $"Natural: {string.Join(", ", files.OrderBy(f => f.Length).ThenBy(f => f, StringComparer.Ordinal))}",
        ];
    }
}
