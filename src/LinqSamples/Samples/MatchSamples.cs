namespace LinqSamples.Samples;

/// <summary>Football results: goals, wins, draws and points.</summary>
[SampleCategory("Match results", 79)]
public static class MatchSamples
{
    private sealed record Match(string Home, string Away, int HomeGoals, int AwayGoals);

    private static readonly Match[] Matches =
    [
        new("Reds", "Blues", 2, 1), new("Greens", "Reds", 0, 0), new("Blues", "Greens", 3, 1),
        new("Reds", "Greens", 1, 2), new("Blues", "Reds", 1, 1),
    ];

    // One row per team per match: (team, goals for, goals against).
    private static IEnumerable<(string Team, int For, int Against)> TeamRows() =>
        Matches.SelectMany(m => new[] { (m.Home, m.HomeGoals, m.AwayGoals), (m.Away, m.AwayGoals, m.HomeGoals) });

    private static int Points((string Team, int For, int Against) r) => r.For > r.Against ? 3 : r.For == r.Against ? 1 : 0;

    [Sample("Total goals", Description = "Sum goals from both sides.")]
    public static int TotalGoals() => Matches.Sum(m => m.HomeGoals + m.AwayGoals);

    [Sample("Number of draws", Description = "Count with a predicate.")]
    public static int Draws() => Matches.Count(m => m.HomeGoals == m.AwayGoals);

    [Sample("Biggest win", Description = "MaxBy goal difference.")]
    public static string BiggestWin()
    {
        var m = Matches.MaxBy(x => Math.Abs(x.HomeGoals - x.AwayGoals))!;
        return $"{m.Home}-{m.Away}";
    }

    [Sample("Teams that won at home", Description = "Filter, project and sort.")]
    public static IEnumerable<string> HomeWinners() => Matches.Where(m => m.HomeGoals > m.AwayGoals).Select(m => m.Home).Order();

    [Sample("Goals scored per team", Description = "SelectMany both sides, then group.")]
    public static IEnumerable<string> GoalsPerTeam() => TeamRows().GroupBy(r => r.Team).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(r => r.For)}");

    [Sample("Matches per team", Description = "Count rows per team.")]
    public static IEnumerable<string> MatchesPerTeam() => TeamRows().CountBy(r => r.Team).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Points per team", Description = "3 for a win, 1 for a draw.")]
    public static IEnumerable<string> PointsPerTeam() => TeamRows().GroupBy(r => r.Team).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(Points)}");

    [Sample("Table leader", Description = "MaxBy total points.")]
    public static string Leader() => TeamRows().GroupBy(r => r.Team).MaxBy(g => g.Sum(Points))!.Key;

    [Sample("All teams", Description = "Distinct names from both columns.")]
    public static IEnumerable<string> Teams() => Matches.Select(m => m.Home).Concat(Matches.Select(m => m.Away)).Distinct().Order();

    [Sample("High scoring matches", Description = "Three or more goals in total.")]
    public static IEnumerable<string> HighScoring() => Matches.Where(m => m.HomeGoals + m.AwayGoals >= 3).Select(m => $"{m.Home}-{m.Away}");
}
