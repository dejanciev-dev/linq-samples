namespace LinqSamples.Samples;

[SampleCategory("Projection", 2)]
public static class ProjectionSamples
{
    [Sample("Select", Description = "Transform every element.")]
    public static IEnumerable<string> Select() =>
        Products.Select(p => p.Name.ToUpperInvariant());

    [Sample("Select into a tuple", Description = "Shape results without declaring a class.")]
    public static IEnumerable<(string Name, decimal PriceWithVat)> SelectTuple() =>
        Products.Select(p => (p.Name, PriceWithVat: Math.Round(p.Price * 1.18m, 2)));

    [Sample("Select with index", Description = "Use the element position, e.g. for numbering.")]
    public static IEnumerable<string> SelectWithIndex() =>
        Customers.Select((c, i) => $"{i + 1}. {c.Name}");

    [Sample("SelectMany", Description = "Flatten a sequence of sequences.")]
    public static IEnumerable<char> SelectMany() =>
        new[] { "ab", "cd", "ef" }.SelectMany(s => s);

    [Sample("SelectMany with result selector", Description = "Produce a cross product (every pair).")]
    public static IEnumerable<string> CrossProduct()
    {
        string[] sizes = ["S", "M"];
        string[] colors = ["Red", "Blue"];
        return sizes.SelectMany(_ => colors, (size, color) => $"{size}-{color}");
    }

    [Sample("Zip", Description = "Pair up elements from two (or three) sequences by position.")]
    public static IEnumerable<string> Zip()
    {
        string[] names = ["Alice", "Bob", "Carla"];
        int[] scores = [90, 75, 82];
        string[] grades = ["A", "C", "B"];
        return names.Zip(scores, grades).Select(t => $"{t.First}: {t.Second} ({t.Third})");
    }

    [Sample("Cast", Description = "Convert a non-generic IEnumerable into IEnumerable<T>.")]
    public static IEnumerable<int> Cast()
    {
        System.Collections.ArrayList legacy = [1, 2, 3];
        return legacy.Cast<int>().Select(n => n * 10);
    }

    [Sample("Select with a switch expression", Description = "Map values to labels inline with pattern matching.")]
    public static IEnumerable<string> SelectSwitch() =>
        Products.Select(p => $"{p.Name}: {p.Price switch
        {
            >= 500m => "premium",
            >= 100m => "mid-range",
            _ => "budget",
        }}");

    [Sample("Anonymous types", Description = "Carry computed values through a pipeline without declaring a type.")]
    public static IEnumerable<string> AnonymousTypes() =>
        Customers.Select(c => new { c.Name, Length = c.Name.Length })
                 .Where(x => x.Length > 3)
                 .Select(x => x.Name);

    [Sample("SelectMany with index", Description = "The outer element's position is available when flattening.")]
    public static IEnumerable<string> SelectManyWithIndex()
    {
        string[] lines = ["to be", "or not"];
        return lines.SelectMany((line, i) => line.Split(' ').Select(word => $"line {i + 1}: {word}"));
    }

    [Sample("Enrich with a lookup dictionary", Description = "Build a dictionary once instead of searching a list for every element.")]
    public static IEnumerable<string> EnrichWithLookup()
    {
        var productsById = Products.ToDictionary(p => p.Id);
        return Orders.Take(3).Select(o => $"#{o.Id}: {o.Quantity} x {productsById[o.ProductId].Name}");
    }
}
