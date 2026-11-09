namespace LinqSamples.Samples;

/// <summary>Garden plants: beds, heights and watering needs.</summary>
[SampleCategory("Garden plants", 64)]
public static class GardenSamples
{
    private sealed record Plant(string Name, string Bed, int Height, int Water);

    private static readonly Plant[] Plants =
    [
        new("Rose", "A", 80, 3), new("Tulip", "A", 30, 2), new("Fern", "B", 50, 5),
        new("Mint", "B", 25, 4), new("Basil", "C", 40, 3), new("Sage", "C", 35, 1),
    ];

    [Sample("Tallest plant", Description = "MaxBy height.")]
    public static string Tallest() => Plants.MaxBy(p => p.Height)!.Name;

    [Sample("Total height", Description = "Sum of all heights.")]
    public static int TotalHeight() => Plants.Sum(p => p.Height);

    [Sample("Beds by water", Description = "Total water per bed, highest first.")]
    public static IEnumerable<string> BedsByWater() =>
        Plants.GroupBy(p => p.Bed).Select(g => (g.Key, W: g.Sum(p => p.Water))).OrderByDescending(t => t.W).Select(t => $"{t.Key}:{t.W}");

    [Sample("Thirsty plants", Description = "Water need of 4 or more.")]
    public static IEnumerable<string> Thirsty() => Plants.Where(p => p.Water >= 4).Select(p => p.Name).Order();

    [Sample("Short plants", Description = "Plants under 40 cm.")]
    public static IEnumerable<string> Short() => Plants.Where(p => p.Height < 40).Select(p => p.Name).Order();

    [Sample("Tallest per bed", Description = "MaxBy inside each group.")]
    public static IEnumerable<string> TallestPerBed() => Plants.GroupBy(p => p.Bed).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MaxBy(p => p.Height)!.Name}");

    [Sample("Any over 100", Description = "Any plant taller than 100.")]
    public static bool AnyOver100() => Plants.Any(p => p.Height > 100);

    [Sample("Three shortest", Description = "OrderBy then Take(3).")]
    public static IEnumerable<string> ThreeShortest() => Plants.OrderBy(p => p.Height).Take(3).Select(p => p.Name);

    [Sample("Lowest water", Description = "MinBy water.")]
    public static string LowestWater() => Plants.MinBy(p => p.Water)!.Name;

    [Sample("Chunk sizes", Description = "Chunk into groups of four.")]
    public static IEnumerable<int> ChunkSizes() => Plants.Chunk(4).Select(c => c.Length);
}
