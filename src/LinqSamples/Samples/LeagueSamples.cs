namespace LinqSamples.Samples;

/// <summary>A tiny football league table built from match results.</summary>
[SampleCategory("League table", 54)]
public static class LeagueSamples
{
    private sealed record Game(string Home, string Away, int HomeGoals, int AwayGoals);

    private static readonly Game[] Games =
    [
        new("Reds", "Blues", 2, 1), new("Greens", "Reds", 0, 0), new("Blues", "Greens", 3, 1),
        new("Reds", "Greens", 1, 0), new("Blues", "Reds", 1, 1),
    ];

    private static IEnumerable<(string Team, int For, int Against)> Rows() =>
        Games.SelectMany(g => new[] { (g.Home, g.HomeGoals, g.AwayGoals), (g.Away, g.AwayGoals, g.HomeGoals) });

    private static int Points(int f, int a) => f > a ? 3 : f == a ? 1 : 0;

    private static IEnumerable<(string Team, int Points)> Table() =>
        Rows().GroupBy(r => r.Team).Select(g => (g.Key, g.Sum(r => Points(r.For, r.Against))))
              .OrderByDescending(t => t.Item2).ThenBy(t => t.Key);

    [Sample("Points table", Description = "Three points for a win, one for a draw.")]
    public static IEnumerable<string> PointsTable() => Table().Select(t => $"{t.Team}:{t.Points}");

    [Sample("League leader", Description = "First row of the table.")]
    public static string Leader() => Table().First().Team;

    [Sample("Total goals", Description = "Sum of both sides in every game.")]
    public static int TotalGoals() => Games.Sum(g => g.HomeGoals + g.AwayGoals);

    [Sample("Draw count", Description = "Equal goals.")]
    public static int Draws() => Games.Count(g => g.HomeGoals == g.AwayGoals);

    [Sample("Biggest win", Description = "MaxBy the absolute goal difference.")]
    public static string BiggestWin()
    {
        var g = Games.MaxBy(x => Math.Abs(x.HomeGoals - x.AwayGoals))!;
        return $"{g.Home} {g.HomeGoals}-{g.AwayGoals} {g.Away}";
    }

    [Sample("Goals scored", Description = "SelectMany both sides, then group.")]
    public static IEnumerable<string> GoalsScored() => Rows().GroupBy(r => r.Team).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(r => r.For)}");

    [Sample("Goal difference", Description = "Scored minus conceded per team.")]
    public static IEnumerable<string> GoalDifference() => Rows().GroupBy(r => r.Team).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(r => r.For - r.Against)}");

    [Sample("Unbeaten teams", Description = "No row where the team lost.")]
    public static IEnumerable<string> Unbeaten() => Rows().GroupBy(r => r.Team).Where(g => g.All(r => r.For >= r.Against)).Select(g => g.Key).Order();

    [Sample("Home wins", Description = "Count games the home side won.")]
    public static int HomeWins() => Games.Count(g => g.HomeGoals > g.AwayGoals);

    [Sample("Teams with a clean sheet", Description = "Rows where nothing was conceded.")]
    public static IEnumerable<string> CleanSheets() => Rows().Where(r => r.Against == 0).Select(r => r.Team).Distinct().Order();
}
