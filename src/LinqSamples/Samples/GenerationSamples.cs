namespace LinqSamples.Samples;

[SampleCategory("Generation & conversion", 10)]
public static class GenerationSamples
{
    [Sample("Range / Repeat / Empty")]
    public static IEnumerable<string> RangeRepeatEmpty() =>
    [
        string.Join(",", Enumerable.Range(1, 5)),
        string.Join(",", Enumerable.Repeat("ha", 3)),
        $"Empty count: {Enumerable.Empty<int>().Count()}",
    ];

    [Sample("Sequence (.NET 10+)", Description = "Generate an inclusive range with a custom step.")]
    public static IEnumerable<int> Sequence() =>
        Enumerable.Sequence(0, 20, 5);

    [Sample("Index (.NET 9+)", Description = "Enumerate with (Index, Item) tuples.")]
    public static IEnumerable<string> Index() =>
        Customers.Index().Select(t => $"{t.Index}: {t.Item.Name}");

    [Sample("Append / Prepend / Concat")]
    public static IEnumerable<int> AppendPrepend() =>
        Enumerable.Range(2, 3).Prepend(1).Append(5).Concat([6, 7]);

    [Sample("ToDictionary")]
    public static IEnumerable<KeyValuePair<int, string>> ToDictionary() =>
        Products.Where(p => p.Category == "Furniture").ToDictionary(p => p.Id, p => p.Name);

    [Sample("ToHashSet")]
    public static string ToHashSet()
    {
        var cities = Customers.Select(c => c.City).ToHashSet();
        return $"{cities.Count} unique cities, contains Vienna: {cities.Contains("Vienna")}";
    }
}
