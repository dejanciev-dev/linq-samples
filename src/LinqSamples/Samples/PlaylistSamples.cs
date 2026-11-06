namespace LinqSamples.Samples;

/// <summary>Playlist durations and play counts.</summary>
[SampleCategory("Playlist", 52)]
public static class PlaylistSamples
{
    private sealed record Track(string Title, string Artist, int Seconds, int Plays);

    private static readonly Track[] Tracks =
    [
        new("Sun", "Mia", 200, 50), new("Moon", "Mia", 180, 30), new("Star", "Leo", 240, 80),
        new("Sky", "Leo", 210, 10), new("Sea", "Zed", 190, 60),
    ];

    [Sample("Total seconds", Description = "Sum of all durations.")]
    public static int TotalSeconds() => Tracks.Sum(t => t.Seconds);

    [Sample("Most played", Description = "MaxBy plays.")]
    public static string MostPlayed() => Tracks.MaxBy(t => t.Plays)!.Title;

    [Sample("Plays per artist", Description = "GroupBy artist and sum plays.")]
    public static IEnumerable<string> PlaysPerArtist() =>
        Tracks.GroupBy(t => t.Artist).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(t => t.Plays)}");

    [Sample("Longest track per artist", Description = "GroupBy artist then MaxBy seconds.")]
    public static IEnumerable<string> LongestPerArtist() =>
        Tracks.GroupBy(t => t.Artist).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MaxBy(t => t.Seconds)!.Title}");

    [Sample("Under 200 seconds", Description = "Short tracks, sorted.")]
    public static IEnumerable<string> Short() => Tracks.Where(t => t.Seconds < 200).Select(t => t.Title).Order();

    [Sample("Top two by plays", Description = "OrderByDescending then Take(2).")]
    public static IEnumerable<string> TopTwo() => Tracks.OrderByDescending(t => t.Plays).Take(2).Select(t => t.Title);

    [Sample("Total listening seconds", Description = "Sum of seconds times plays.")]
    public static int ListeningSeconds() => Tracks.Sum(t => t.Seconds * t.Plays);

    [Sample("Artists with several tracks", Description = "Groups with more than one item.")]
    public static IEnumerable<string> MultiTrackArtists() => Tracks.GroupBy(t => t.Artist).Where(g => g.Count() > 1).Select(g => g.Key).Order();

    [Sample("Playlist length", Description = "Format the total as minutes:seconds.")]
    public static string Length()
    {
        var t = TimeSpan.FromSeconds(Tracks.Sum(x => x.Seconds));
        return $"{(int)t.TotalMinutes}:{t.Seconds:00}";
    }

    [Sample("Titles A to Z", Description = "Order the titles.")]
    public static IEnumerable<string> Titles() => Tracks.Select(t => t.Title).Order();
}
