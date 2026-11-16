namespace LinqSamples.Samples;

/// <summary>Library loans: members, titles, genres and loan length.</summary>
[SampleCategory("Library loans", 74)]
public static class LibrarySamples
{
    private sealed record Loan(string Member, string Title, string Genre, int Days);

    private static readonly Loan[] Loans =
    [
        new("Ann", "Dune", "SciFi", 14), new("Bob", "Emma", "Classic", 7),
        new("Ann", "Emma", "Classic", 10), new("Cid", "Dune", "SciFi", 21),
        new("Dee", "Ulysses", "Classic", 30), new("Bob", "Neuromancer", "SciFi", 5),
    ];

    [Sample("Total loan days", Description = "Sum of all loan lengths.")]
    public static int TotalDays() => Loans.Sum(l => l.Days);

    [Sample("Loans per member", Description = "Group by member and count.")]
    public static IEnumerable<string> LoansPerMember() => Loans.GroupBy(l => l.Member).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Count()}");

    [Sample("Titles", Description = "Distinct titles, sorted.")]
    public static IEnumerable<string> Titles() => Loans.Select(l => l.Title).Distinct().Order();

    [Sample("Days per genre", Description = "Sum days grouped by genre.")]
    public static IEnumerable<string> DaysPerGenre() => Loans.GroupBy(l => l.Genre).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(l => l.Days)}");

    [Sample("Longest loan", Description = "MaxBy days, returning the member.")]
    public static string LongestLoan() => Loans.MaxBy(l => l.Days)!.Member;

    [Sample("Popular title", Description = "CountBy title, then MaxBy (first wins a tie).")]
    public static string PopularTitle() => Loans.CountBy(l => l.Title).MaxBy(c => c.Value).Key;

    [Sample("Sci-fi readers", Description = "Distinct members who borrowed sci-fi.")]
    public static IEnumerable<string> SciFiReaders() => Loans.Where(l => l.Genre == "SciFi").Select(l => l.Member).Distinct().Order();

    [Sample("Long loans", Description = "Count loans over two weeks.")]
    public static int LongLoans() => Loans.Count(l => l.Days > 14);

    [Sample("Average loan", Description = "Average days per loan.")]
    public static double AverageDays() => Loans.Average(l => l.Days);

    [Sample("Genre explorers", Description = "Members who borrowed from every genre.")]
    public static IEnumerable<string> GenreExplorers() =>
        Loans.GroupBy(l => l.Member).Where(g => g.Select(l => l.Genre).Distinct().Count() == 2).Select(g => g.Key).Order();
}
