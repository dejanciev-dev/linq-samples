namespace LinqSamples.Samples;

/// <summary>A small film catalogue with scores out of 100.</summary>
[SampleCategory("Movies", 50)]
public static class MovieSamples
{
    private sealed record Movie(string Title, string Genre, int Year, int Score, int Minutes);

    private static readonly Movie[] Movies =
    [
        new("Alien", "scifi", 1979, 85, 117), new("Heat", "crime", 1995, 83, 170), new("Fargo", "crime", 1996, 81, 98),
        new("Arrival", "scifi", 2016, 79, 116), new("Up", "family", 2009, 82, 96), new("Coco", "family", 2017, 84, 105),
        new("Se7en", "crime", 1995, 86, 127),
    ];

    [Sample("Top rated movie", Description = "MaxBy score.")]
    public static string TopRated() => Movies.MaxBy(m => m.Score)!.Title;

    [Sample("Movies per genre", Description = "CountBy genre.")]
    public static IEnumerable<string> PerGenre() => Movies.CountBy(m => m.Genre).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Best score per genre", Description = "GroupBy genre and take the max score.")]
    public static IEnumerable<string> BestPerGenre() =>
        Movies.GroupBy(m => m.Genre).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Max(m => m.Score)}");

    [Sample("Short movies", Description = "Under 100 minutes, sorted by title.")]
    public static IEnumerable<string> Short() => Movies.Where(m => m.Minutes < 100).Select(m => m.Title).Order();

    [Sample("Total minutes", Description = "Sum of the running times.")]
    public static int TotalMinutes() => Movies.Sum(m => m.Minutes);

    [Sample("Longest movie", Description = "MaxBy minutes.")]
    public static string Longest() => Movies.MaxBy(m => m.Minutes)!.Title;

    [Sample("Movies from 1995", Description = "Filter by year and sort the titles.")]
    public static IEnumerable<string> From1995() => Movies.Where(m => m.Year == 1995).Select(m => m.Title).Order();

    [Sample("Newest per genre", Description = "GroupBy genre then MaxBy year.")]
    public static IEnumerable<string> NewestPerGenre() =>
        Movies.GroupBy(m => m.Genre).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MaxBy(m => m.Year)!.Title}");

    [Sample("Scores above 82", Description = "Filter, then order by score descending.")]
    public static IEnumerable<string> HighScores() => Movies.Where(m => m.Score > 82).OrderByDescending(m => m.Score).Select(m => m.Title);

    [Sample("Movies per decade", Description = "CountBy the year rounded down to a decade.")]
    public static IEnumerable<string> PerDecade() => Movies.CountBy(m => m.Year / 10 * 10).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");
}
