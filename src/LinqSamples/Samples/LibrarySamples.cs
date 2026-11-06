namespace LinqSamples.Samples;

/// <summary>Library catalogue queries: authors, loans, decades and page counts.</summary>
[SampleCategory("Library books", 48)]
public static class LibrarySamples
{
    private sealed record Book(string Title, string Author, int Year, int Pages, bool Loaned);

    private static readonly Book[] Books =
    [
        new("Dune", "Herbert", 1965, 612, true),
        new("Emma", "Austen", 1815, 474, false),
        new("Persuasion", "Austen", 1817, 249, true),
        new("Neuromancer", "Gibson", 1984, 271, false),
        new("Children of Dune", "Herbert", 1976, 444, false),
        new("Count Zero", "Gibson", 1986, 320, true),
    ];

    [Sample("Titles by author", Description = "GroupBy author and join the sorted titles.")]
    public static IEnumerable<string> TitlesByAuthor() =>
        Books.GroupBy(b => b.Author).OrderBy(g => g.Key).Select(g => $"{g.Key}:{string.Join(",", g.Select(b => b.Title).Order())}");

    [Sample("Available titles", Description = "Books that are not on loan, sorted.")]
    public static IEnumerable<string> Available() => Books.Where(b => !b.Loaned).Select(b => b.Title).Order();

    [Sample("Longest book", Description = "MaxBy pages.")]
    public static string Longest() => Books.MaxBy(b => b.Pages)!.Title;

    [Sample("Pages per author", Description = "GroupBy author and sum the pages.")]
    public static IEnumerable<string> PagesPerAuthor() =>
        Books.GroupBy(b => b.Author).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(b => b.Pages)}");

    [Sample("Oldest book", Description = "MinBy publication year.")]
    public static string Oldest() => Books.MinBy(b => b.Year)!.Title;

    [Sample("Books per decade", Description = "CountBy the year rounded down to its decade.")]
    public static IEnumerable<string> PerDecade() =>
        Books.CountBy(b => b.Year / 10 * 10).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Pages on loan", Description = "Filter loaned books, then sum their pages.")]
    public static int PagesOnLoan() => Books.Where(b => b.Loaned).Sum(b => b.Pages);

    [Sample("Average pages", Description = "Average of the page counts.")]
    public static double AveragePages() => Books.Average(b => b.Pages);
}
