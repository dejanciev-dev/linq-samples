namespace LinqSamples.Samples;

/// <summary>Money transfers between people: balances, mutual pairs and multi-hop paths.</summary>
[SampleCategory("Money transfers", 82)]
public static class TransferSamples
{
    private sealed record Transfer(string From, string To, int Amount);

    private static readonly Transfer[] Transfers =
    [
        new("Ann", "Bob", 50), new("Bob", "Cy", 30), new("Ann", "Cy", 20),
        new("Cy", "Ann", 10), new("Bob", "Ann", 5), new("Dee", "Bob", 40),
    ];

    private static IEnumerable<(string Name, int Net)> NetBalances() =>
        Transfers.SelectMany(t => new[] { (Name: t.From, Delta: -t.Amount), (Name: t.To, Delta: t.Amount) })
                 .GroupBy(x => x.Name)
                 .OrderBy(g => g.Key, StringComparer.Ordinal)
                 .Select(g => (g.Key, g.Sum(x => x.Delta)));

    [Sample("Net balance per person", Description = "Split each transfer into a debit and a credit with SelectMany.")]
    public static IEnumerable<string> Net() => NetBalances().Select(x => $"{x.Name}:{x.Net}");

    [Sample("Richest and poorest", Description = "MaxBy and MinBy over the net balances.")]
    public static string RichestAndPoorest() => $"{NetBalances().MaxBy(x => x.Net).Name}/{NetBalances().MinBy(x => x.Net).Name}";

    [Sample("Mutual partners", Description = "Join the list with itself on swapped columns.")]
    public static IEnumerable<string> Mutual() =>
        from a in Transfers
        join b in Transfers on (a.From, a.To) equals (b.To, b.From)
        where string.CompareOrdinal(a.From, a.To) < 0
        select $"{a.From}-{a.To}";

    [Sample("Never received money", Description = "Except between two projections.")]
    public static IEnumerable<string> NeverReceived() => Transfers.Select(t => t.From).Except(Transfers.Select(t => t.To));

    [Sample("Total sent per person", Description = "Group by sender and sum.")]
    public static IEnumerable<string> Sent() =>
        Transfers.GroupBy(t => t.From).OrderBy(g => g.Key, StringComparer.Ordinal).Select(g => $"{g.Key}:{g.Sum(t => t.Amount)}");

    [Sample("Largest transfer per sender", Description = "MaxBy inside each group.")]
    public static IEnumerable<string> LargestPerSender() =>
        Transfers.GroupBy(t => t.From).OrderBy(g => g.Key, StringComparer.Ordinal)
                 .Select(g => g.MaxBy(t => t.Amount)!).Select(t => $"{t.From}>{t.To}");

    [Sample("Two-hop paths", Description = "Chain a transfer into the next one, excluding round trips.")]
    public static IEnumerable<string> TwoHop() =>
        from a in Transfers
        from b in Transfers
        where a.To == b.From && a.From != b.To
        select $"{a.From}>{a.To}>{b.To}";

    [Sample("Above-average transfers", Description = "Compare each item with a precomputed average.")]
    public static IEnumerable<string> AboveAverage()
    {
        var average = Transfers.Average(t => t.Amount);
        return Transfers.Where(t => t.Amount > average).Select(t => $"{t.From}>{t.To}:{t.Amount}");
    }

    [Sample("Partners per person", Description = "Flatten both directions, group and distinct.")]
    public static IEnumerable<string> Partners() =>
        Transfers.SelectMany(t => new[] { (Who: t.From, Other: t.To), (Who: t.To, Other: t.From) })
                 .GroupBy(x => x.Who).OrderBy(g => g.Key, StringComparer.Ordinal)
                 .Select(g => $"{g.Key}:{string.Join(",", g.Select(x => x.Other).Distinct().Order())}");

    [Sample("Running total of transfers", Description = "Aggregate with a list accumulator.")]
    public static IEnumerable<int> RunningTotal() =>
        Transfers.Aggregate(new List<int>(), (acc, t) =>
        {
            acc.Add((acc.Count > 0 ? acc[^1] : 0) + t.Amount);
            return acc;
        });
}
