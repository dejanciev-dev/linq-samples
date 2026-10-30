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

    [Sample("ToDictionary with duplicate keys", Description = "ToDictionary throws on a repeated key; group first when keys are not unique.")]
    public static IEnumerable<string> ToDictionaryDuplicates()
    {
        string Attempt()
        {
            try
            {
                return $"{Customers.ToDictionary(c => c.City).Count} entries";
            }
            catch (ArgumentException)
            {
                return "throws ArgumentException";
            }
        }

        var byCity = Customers.GroupBy(c => c.City)
                              .ToDictionary(g => g.Key, g => string.Join(", ", g.Select(c => c.Name)));
        return [$"ToDictionary(c => c.City): {Attempt()}", .. byCity.Select(kv => $"{kv.Key}: {kv.Value}")];
    }

    [Sample("Generate dates", Description = "Range over day numbers to build calendars - here every Monday in February 2026.")]
    public static IEnumerable<string> GenerateDates() =>
        Enumerable.Range(1, DateTime.DaysInMonth(2026, 2))
                  .Select(day => new DateOnly(2026, 2, day))
                  .Where(d => d.DayOfWeek == DayOfWeek.Monday)
                  .Select(d => $"{d:yyyy-MM-dd}");

    [Sample("Multiplication table", Description = "Nested Range calls generate two-dimensional data.")]
    public static IEnumerable<string> MultiplicationTable() =>
        Enumerable.Range(1, 3)
                  .Select(i => string.Join(" ", Enumerable.Range(1, 3).Select(j => (i * j).ToString().PadLeft(2))));

    [Sample("Shuffle (.NET 10+)", Description = "Random order - here three random products.")]
    public static IEnumerable<string> Shuffle() =>
        Products.Shuffle().Take(3).Select(p => p.Name);
}
