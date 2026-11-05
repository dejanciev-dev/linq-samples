namespace LinqSamples.Samples;

/// <summary>Ranking, ties, percentiles and leaderboards with LINQ.</summary>
[SampleCategory("Ranking & leaderboards", 38)]
public static class RankingSamples
{
    private static readonly (string Name, int Score)[] Scores =
        [("Ada", 90), ("Bob", 75), ("Cy", 90), ("Di", 60), ("Ed", 75), ("Flo", 40)];

    [Sample("Leaderboard", Description = "OrderByDescending, then ThenBy for a stable tie-break.")]
    public static IEnumerable<string> Leaderboard() =>
        Scores.OrderByDescending(s => s.Score).ThenBy(s => s.Name).Select(s => $"{s.Name}:{s.Score}");

    [Sample("Standard competition rank (1224)", Description = "Rank = 1 + number of strictly higher scores.")]
    public static IEnumerable<string> CompetitionRank() =>
        Scores.OrderByDescending(s => s.Score).ThenBy(s => s.Name)
            .Select(s => $"{s.Name}={1 + Scores.Count(o => o.Score > s.Score)}");

    [Sample("Dense rank (1223)", Description = "Rank = 1 + number of distinct higher scores.")]
    public static IEnumerable<string> DenseRank() =>
        Scores.OrderByDescending(s => s.Score).ThenBy(s => s.Name)
            .Select(s => $"{s.Name}={1 + Scores.Select(o => o.Score).Distinct().Count(x => x > s.Score)}");

    [Sample("Top 3 including ties", Description = "Keep everyone whose score is among the 3 best distinct scores.")]
    public static IEnumerable<string> TopThreeWithTies()
    {
        var cutoff = Scores.Select(s => s.Score).Distinct().OrderByDescending(x => x).Take(3).Min();
        return Scores.Where(s => s.Score >= cutoff).OrderByDescending(s => s.Score).ThenBy(s => s.Name).Select(s => s.Name);
    }

    [Sample("Group by score tier", Description = "Group then order groups by key descending.")]
    public static IEnumerable<string> ByScore() =>
        Scores.GroupBy(s => s.Score).OrderByDescending(g => g.Key)
            .Select(g => $"{g.Key}: {string.Join(",", g.Select(s => s.Name).Order())}");

    [Sample("Percentile rank", Description = "Share of scores strictly below, as a percentage.")]
    public static IEnumerable<string> PercentileRank() =>
        Scores.OrderBy(s => s.Name)
            .Select(s => $"{s.Name}={Math.Round(100.0 * Scores.Count(o => o.Score < s.Score) / Scores.Length)}");

    [Sample("Median score", Description = "Order, then average the middle one or two.")]
    public static double Median()
    {
        var sorted = Scores.Select(s => s.Score).Order().ToArray();
        return sorted.Skip((sorted.Length - 1) / 2).Take(2 - sorted.Length % 2).Average();
    }

    [Sample("Rank changes between rounds", Description = "Join two rankings by name and subtract positions.")]
    public static IEnumerable<string> RankChanges()
    {
        string[] before = ["Ada", "Bob", "Cy", "Di"];
        string[] after = ["Cy", "Ada", "Di", "Bob"];
        return after.Select((n, i) => (Name: n, Now: i))
            .Join(before.Select((n, i) => (Name: n, Was: i)), a => a.Name, b => b.Name, (a, b) => $"{a.Name}:{b.Was - a.Now:+0;-0;0}");
    }

    [Sample("Worst and best per bracket", Description = "MinBy/MaxBy inside a group projection.")]
    public static IEnumerable<string> BestAndWorst() =>
        Scores.GroupBy(s => s.Score >= 75 ? "high" : "low").OrderBy(g => g.Key)
            .Select(g => $"{g.Key}: best={g.MaxBy(s => s.Score)!.Name} worst={g.MinBy(s => s.Score)!.Name}");

    [Sample("Weighted score", Description = "Sum of value * weight divided by the weights.")]
    public static double WeightedAverage()
    {
        (double Value, double Weight)[] items = [(90, 3), (70, 1), (50, 1)];
        return items.Sum(i => i.Value * i.Weight) / items.Sum(i => i.Weight);
    }
}
