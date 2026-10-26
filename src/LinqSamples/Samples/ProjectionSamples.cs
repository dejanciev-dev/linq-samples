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
}
