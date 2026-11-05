namespace LinqSamples.Samples;

/// <summary>Analysing application log lines with LINQ.</summary>
[SampleCategory("Log analysis", 45)]
public static class LogSamples
{
    private static readonly string[] Lines =
    [
        "2025-01-01 10:00:01 INFO  api  Started",
        "2025-01-01 10:00:05 WARN  db   Slow query",
        "2025-01-01 10:01:10 ERROR api  Timeout",
        "2025-01-01 10:01:12 ERROR db   Connection lost",
        "2025-01-01 10:02:00 INFO  api  Recovered",
        "2025-01-01 10:02:30 ERROR api  Timeout",
    ];

    private static IEnumerable<(DateTime Time, string Level, string Source, string Message)> Parsed() =>
        Lines.Select(l => l.Split(' ', 5, StringSplitOptions.RemoveEmptyEntries))
             .Select(p => (DateTime.Parse($"{p[0]} {p[1]}"), p[2], p[3], p[4]));

    [Sample("Count by level", Description = "CountBy on the parsed level.")]
    public static IEnumerable<string> ByLevel() =>
        Parsed().CountBy(e => e.Level).OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}:{kv.Value}");

    [Sample("Errors only", Description = "Where on level, projected to messages.")]
    public static IEnumerable<string> Errors() =>
        Parsed().Where(e => e.Level == "ERROR").Select(e => e.Message);

    [Sample("Noisiest source", Description = "GroupBy source, MaxBy on count.")]
    public static string NoisiestSource() =>
        Parsed().GroupBy(e => e.Source).MaxBy(g => g.Count())!.Key;

    [Sample("Most frequent error", Description = "Group error messages and order by count.")]
    public static string FrequentError() =>
        Parsed().Where(e => e.Level == "ERROR").GroupBy(e => e.Message)
                .OrderByDescending(g => g.Count()).First().Key;

    [Sample("Errors per minute", Description = "GroupBy a truncated timestamp.")]
    public static IEnumerable<string> PerMinute() =>
        Parsed().Where(e => e.Level == "ERROR")
                .GroupBy(e => e.Time.ToString("HH:mm"))
                .Select(g => $"{g.Key}={g.Count()}");

    [Sample("Gaps between events", Description = "Zip with Skip(1) to get time deltas in seconds.")]
    public static IEnumerable<double> Gaps()
    {
        var times = Parsed().Select(e => e.Time).ToList();
        return times.Zip(times.Skip(1), (a, b) => (b - a).TotalSeconds);
    }

    [Sample("Longest quiet period", Description = "Max of the gaps between events.")]
    public static double LongestQuiet() => Gaps().Max();

    [Sample("Recovery after first error", Description = "SkipWhile to the first error, then first INFO.")]
    public static string Recovery() =>
        Parsed().SkipWhile(e => e.Level != "ERROR").First(e => e.Level == "INFO").Message;

    [Sample("Sources with errors", Description = "Distinct sources of error events, sorted.")]
    public static IEnumerable<string> ErrorSources() =>
        Parsed().Where(e => e.Level == "ERROR").Select(e => e.Source).Distinct().Order();

    [Sample("Error rate", Description = "Share of events that are errors, as a percentage.")]
    public static double ErrorRate() => Parsed().Average(e => e.Level == "ERROR" ? 100.0 : 0.0);

    [Sample("Levels per source", Description = "GroupBy source with the distinct levels seen, sorted.")]
    public static IEnumerable<string> LevelsPerSource() =>
        Parsed().GroupBy(e => e.Source).OrderBy(g => g.Key)
                .Select(g => $"{g.Key}:{string.Join("/", g.Select(e => e.Level).Distinct().Order())}");
}
