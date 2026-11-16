namespace LinqSamples.Samples;

/// <summary>Albums: artists, years, tracks and running time.</summary>
[SampleCategory("Music albums", 77)]
public static class MusicSamples
{
    private sealed record Album(string Title, string Artist, int Year, int Tracks, int Minutes);

    private static readonly Album[] Albums =
    [
        new("Abbey Road", "Beatles", 1969, 17, 47), new("Revolver", "Beatles", 1966, 14, 35),
        new("Thriller", "Jackson", 1982, 9, 42), new("Bad", "Jackson", 1987, 11, 48),
        new("Nevermind", "Nirvana", 1991, 13, 43), new("Utero", "Nirvana", 1993, 12, 41),
    ];

    [Sample("Total album minutes", Description = "Sum of running times.")]
    public static int TotalMinutes() => Albums.Sum(a => a.Minutes);

    [Sample("Albums per artist", Description = "Group by artist and count.")]
    public static IEnumerable<string> PerArtist() => Albums.GroupBy(a => a.Artist).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Count()}");

    [Sample("Oldest album", Description = "MinBy year.")]
    public static string Oldest() => Albums.MinBy(a => a.Year)!.Title;

    [Sample("Longest album", Description = "MaxBy minutes.")]
    public static string Longest() => Albums.MaxBy(a => a.Minutes)!.Title;

    [Sample("Tracks per artist", Description = "Sum tracks per group.")]
    public static IEnumerable<string> TracksPerArtist() => Albums.GroupBy(a => a.Artist).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(a => a.Tracks)}");

    [Sample("Albums per decade", Description = "Group by a computed decade.")]
    public static IEnumerable<string> Decades() => Albums.GroupBy(a => a.Year / 10 * 10).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Count()}");

    [Sample("Long albums", Description = "Filter by running time and sort titles.")]
    public static IEnumerable<string> LongAlbums() => Albums.Where(a => a.Minutes >= 43).Select(a => a.Title).Order();

    [Sample("Average album length", Description = "Average rounded to one decimal.")]
    public static double AverageMinutes() => Math.Round(Albums.Average(a => a.Minutes), 1);

    [Sample("Shortest titles first", Description = "OrderBy length, ThenBy alphabetical.")]
    public static IEnumerable<string> ByTitleLength() => Albums.Select(a => a.Title).OrderBy(t => t.Length).ThenBy(t => t, StringComparer.Ordinal);

    [Sample("Artists with a long album", Description = "Keep groups where any album passes 45 minutes.")]
    public static IEnumerable<string> ArtistsWithLongAlbum() => Albums.GroupBy(a => a.Artist).Where(g => g.Any(a => a.Minutes > 45)).Select(g => g.Key).Order();
}
