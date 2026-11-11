namespace LinqSamples.Samples;

/// <summary>Concert tickets: buyers, bands, sections and prices.</summary>
[SampleCategory("Concert tickets", 67)]
public static class ConcertSamples
{
    private sealed record Ticket(string Buyer, string Band, string Section, int Price);

    private static readonly Ticket[] Tickets =
    [
        new("Ann", "Muse", "Floor", 90), new("Bob", "Muse", "Balcony", 50), new("Cid", "Blur", "Floor", 70),
        new("Ann", "Blur", "Balcony", 40), new("Dee", "Muse", "Floor", 90), new("Bob", "Oasis", "Floor", 80),
    ];

    [Sample("Total revenue", Description = "Sum of all ticket prices.")]
    public static int TotalRevenue() => Tickets.Sum(t => t.Price);

    [Sample("Tickets per band", Description = "CountBy band.")]
    public static IEnumerable<string> TicketsPerBand() => Tickets.CountBy(t => t.Band).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Revenue per section", Description = "AggregateBy section summing prices.")]
    public static IEnumerable<string> RevenuePerSection() =>
        Tickets.AggregateBy(t => t.Section, 0, (sum, t) => sum + t.Price).OrderBy(p => p.Key).Select(p => $"{p.Key}:{p.Value}");

    [Sample("Multi-band fans", Description = "Buyers with tickets for more than one band.")]
    public static IEnumerable<string> MultiBandFans() =>
        Tickets.GroupBy(t => t.Buyer).Where(g => g.Select(t => t.Band).Distinct().Count() > 1).Select(g => g.Key).Order();

    [Sample("Priciest ticket", Description = "MaxBy price returns the first of equal maxima.")]
    public static string PriciestTicket() => Tickets.MaxBy(t => t.Price) is { } t ? $"{t.Buyer}:{t.Band}" : "";

    [Sample("Bands", Description = "Distinct band names.")]
    public static IEnumerable<string> Bands() => Tickets.Select(t => t.Band).Distinct().Order();

    [Sample("Top price per band", Description = "Group by band and take the max price.")]
    public static IEnumerable<string> TopPricePerBand() =>
        Tickets.GroupBy(t => t.Band).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Max(t => t.Price)}");

    [Sample("Cheapest band entry", Description = "MinBy price.")]
    public static string CheapestBand() => Tickets.MinBy(t => t.Price)!.Band;

    [Sample("Floor all over 60", Description = "All on a filtered sequence.")]
    public static bool FloorAllOver60() => Tickets.Where(t => t.Section == "Floor").All(t => t.Price > 60);

    [Sample("Spend per buyer", Description = "Order by spend descending, then by name.")]
    public static IEnumerable<string> SpendPerBuyer() =>
        Tickets.GroupBy(t => t.Buyer).Select(g => (Buyer: g.Key, Spend: g.Sum(t => t.Price)))
            .OrderByDescending(x => x.Spend).ThenBy(x => x.Buyer).Select(x => $"{x.Buyer}:{x.Spend}");
}
