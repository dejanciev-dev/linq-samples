namespace LinqSamples.Samples;

/// <summary>Movies: directors, years, ratings and running time.</summary>
[SampleCategory("Film directors", 76)]
public static class FilmSamples
{
    private sealed record Movie(string Title, string Director, int Year, int Rating, int Minutes);

    private static readonly Movie[] Movies =
    [
        new("Alien", "Scott", 1979, 8, 117), new("Blade Runner", "Scott", 1982, 8, 117),
        new("Jaws", "Spielberg", 1975, 8, 124), new("ET", "Spielberg", 1982, 7, 115),
        new("Heat", "Mann", 1995, 8, 170), new("Thief", "Mann", 1981, 7, 124),
    ];

    [Sample("Total minutes", Description = "Sum of running times.")]
    public static int TotalMinutes() => Movies.Sum(m => m.Minutes);

    [Sample("Movies per director", Description = "Group by director and count.")]
    public static IEnumerable<string> PerDirector() => Movies.GroupBy(m => m.Director).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Count()}");

    [Sample("Oldest movie", Description = "MinBy year.")]
    public static string Oldest() => Movies.MinBy(m => m.Year)!.Title;

    [Sample("Longest movie", Description = "MaxBy minutes.")]
    public static string Longest() => Movies.MaxBy(m => m.Minutes)!.Title;

    [Sample("Top rated", Description = "Filter by rating and sort titles.")]
    public static IEnumerable<string> TopRated() => Movies.Where(m => m.Rating == 8).Select(m => m.Title).Order();

    [Sample("Rating sum per director", Description = "Sum ratings per group.")]
    public static IEnumerable<string> RatingSumPerDirector() => Movies.GroupBy(m => m.Director).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(m => m.Rating)}");

    [Sample("Decades", Description = "Group by a computed decade.")]
    public static IEnumerable<string> Decades() => Movies.GroupBy(m => m.Year / 10 * 10).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Count()}");

    [Sample("Busy years", Description = "Years with more than one release.")]
    public static IEnumerable<int> BusyYears() => Movies.CountBy(m => m.Year).Where(c => c.Value > 1).Select(c => c.Key);

    [Sample("Shortest titles first", Description = "OrderBy length, ThenBy alphabetical.")]
    public static IEnumerable<string> ByTitleLength() => Movies.Select(m => m.Title).OrderBy(t => t.Length).ThenBy(t => t);

    [Sample("Same runtime pairs", Description = "Self-join in query syntax, avoiding mirrored pairs.")]
    public static IEnumerable<string> SameRuntime() =>
        from a in Movies
        join b in Movies on a.Minutes equals b.Minutes
        where string.CompareOrdinal(a.Title, b.Title) < 0
        select $"{a.Title}+{b.Title}";
}
