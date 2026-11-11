namespace LinqSamples.Samples;

/// <summary>Train trips: passengers, routes, travel time and fares.</summary>
[SampleCategory("Train trips", 72)]
public static class TrainSamples
{
    private sealed record Trip(string Passenger, string From, string To, int Minutes, int Fare);

    private static readonly Trip[] Trips =
    [
        new("Ann", "Oslo", "Bergen", 420, 90), new("Bob", "Oslo", "Trondheim", 400, 80),
        new("Ann", "Bergen", "Oslo", 420, 90), new("Cid", "Oslo", "Bergen", 420, 70),
        new("Dee", "Trondheim", "Oslo", 400, 85), new("Bob", "Oslo", "Bergen", 420, 75),
    ];

    [Sample("Total fares", Description = "Sum of all fares.")]
    public static int TotalFare() => Trips.Sum(t => t.Fare);

    [Sample("Departures per city", Description = "CountBy departure city.")]
    public static IEnumerable<string> DeparturesPerCity() => Trips.CountBy(t => t.From).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Most popular route", Description = "CountBy a composite route key, then MaxBy.")]
    public static string PopularRoute() => Trips.CountBy(t => $"{t.From}-{t.To}").MaxBy(c => c.Value).Key;

    [Sample("Cities", Description = "Union of departure and arrival cities.")]
    public static IEnumerable<string> Cities() => Trips.Select(t => t.From).Union(Trips.Select(t => t.To)).Order();

    [Sample("Round trippers", Description = "Self-join to find passengers who travelled A to B and back.")]
    public static IEnumerable<string> RoundTrippers() =>
        Trips.Join(Trips, a => (a.Passenger, a.From, a.To), b => (b.Passenger, b.To, b.From), (a, _) => a.Passenger).Distinct().Order();

    [Sample("Cheapest Oslo-Bergen", Description = "Min fare on one route.")]
    public static int CheapestOsloBergen() => Trips.Where(t => t is { From: "Oslo", To: "Bergen" }).Min(t => t.Fare);

    [Sample("Fares per passenger", Description = "Group by passenger and sum fares.")]
    public static IEnumerable<string> FarePerPassenger() =>
        Trips.GroupBy(t => t.Passenger).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(t => t.Fare)}");

    [Sample("Total travel time", Description = "Sum of minutes on board.")]
    public static int TotalMinutes() => Trips.Sum(t => t.Minutes);

    [Sample("Arrivals in Oslo", Description = "Passengers whose trip ended in Oslo.")]
    public static IEnumerable<string> ArrivalsInOslo() => Trips.Where(t => t.To == "Oslo").Select(t => t.Passenger).Distinct().Order();

    [Sample("Fare bands", Description = "Group by a computed band in first-seen order.")]
    public static IEnumerable<string> FareBands() => Trips.GroupBy(t => t.Fare >= 80 ? "high" : "low").Select(g => $"{g.Key}:{g.Count()}");
}
