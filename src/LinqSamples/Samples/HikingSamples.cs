namespace LinqSamples.Samples;

/// <summary>Hiking trails: hikers, trails, distance and climb.</summary>
[SampleCategory("Hiking trails", 70)]
public static class HikingSamples
{
    private sealed record Hike(string Hiker, string Trail, int Km, int Climb);

    private static readonly Hike[] Hikes =
    [
        new("Ann", "Ridge", 12, 800), new("Bob", "Lake", 6, 200), new("Ann", "Lake", 6, 200),
        new("Cid", "Peak", 15, 1200), new("Bob", "Ridge", 12, 800), new("Dee", "Forest", 9, 300),
    ];

    [Sample("Total distance", Description = "Sum of kilometres hiked.")]
    public static int TotalKm() => Hikes.Sum(h => h.Km);

    [Sample("Trails", Description = "Distinct trail names.")]
    public static IEnumerable<string> Trails() => Hikes.Select(h => h.Trail).Distinct().Order();

    [Sample("Distance per hiker", Description = "Group by hiker and sum kilometres.")]
    public static IEnumerable<string> KmPerHiker() =>
        Hikes.GroupBy(h => h.Hiker).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(h => h.Km)}");

    [Sample("Toughest trail", Description = "MaxBy climb.")]
    public static string ToughestTrail() => Hikes.MaxBy(h => h.Climb)!.Trail;

    [Sample("Hikes per trail", Description = "CountBy trail.")]
    public static IEnumerable<string> HikesPerTrail() => Hikes.CountBy(h => h.Trail).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Popular trails", Description = "Trails hiked more than once.")]
    public static IEnumerable<string> PopularTrails() => Hikes.CountBy(h => h.Trail).Where(c => c.Value > 1).Select(c => c.Key).Order();

    [Sample("Average distance", Description = "Average hike length in km.")]
    public static double AverageKm() => Hikes.Average(h => h.Km);

    [Sample("Long hikes", Description = "Count hikes of 10 km or more.")]
    public static int LongHikes() => Hikes.Count(h => h.Km >= 10);

    [Sample("Ann's trail buddies", Description = "Other hikers who walked a trail Ann walked.")]
    public static IEnumerable<string> TrailBuddiesOfAnn()
    {
        var annTrails = Hikes.Where(h => h.Hiker == "Ann").Select(h => h.Trail).ToHashSet();
        return Hikes.Where(h => h.Hiker != "Ann" && annTrails.Contains(h.Trail)).Select(h => h.Hiker).Distinct().Order();
    }

    [Sample("Easy trails", Description = "Distinct trails with less than 500 m of climb.")]
    public static IEnumerable<string> EasyTrails() => Hikes.Where(h => h.Climb < 500).Select(h => h.Trail).Distinct().Order();
}
