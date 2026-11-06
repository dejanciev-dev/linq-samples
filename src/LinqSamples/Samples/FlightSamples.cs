namespace LinqSamples.Samples;

/// <summary>Flight routes, connections and prices.</summary>
[SampleCategory("Flights", 55)]
public static class FlightSamples
{
    private sealed record Flight(string Code, string From, string To, int Minutes, int Price);

    private static readonly Flight[] Flights =
    [
        new("A1", "OSL", "LHR", 120, 150), new("B2", "OSL", "CDG", 140, 180), new("C3", "LHR", "JFK", 450, 500),
        new("D4", "CDG", "JFK", 470, 520), new("E5", "LHR", "OSL", 125, 140),
    ];

    private static IEnumerable<(Flight First, Flight Second)> OslToJfk() =>
        Flights.Where(f => f.From == "OSL").Join(Flights, a => a.To, b => b.From, (a, b) => (a, b)).Where(x => x.b.To == "JFK");

    [Sample("Cheapest flight", Description = "MinBy price.")]
    public static string Cheapest() => Flights.MinBy(f => f.Price)!.Code;

    [Sample("Flights from OSL", Description = "Filter by origin.")]
    public static IEnumerable<string> FromOsl() => Flights.Where(f => f.From == "OSL").Select(f => f.Code);

    [Sample("Departures per airport", Description = "CountBy origin.")]
    public static IEnumerable<string> Departures() => Flights.CountBy(f => f.From).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("All airports", Description = "Union of origins and destinations.")]
    public static IEnumerable<string> Airports() => Flights.Select(f => f.From).Union(Flights.Select(f => f.To)).Order();

    [Sample("Longest flight", Description = "MaxBy minutes.")]
    public static string Longest() => Flights.MaxBy(f => f.Minutes)!.Code;

    [Sample("Total flight hours", Description = "Sum of minutes divided by 60.")]
    public static double TotalHours() => Flights.Sum(f => f.Minutes) / 60.0;

    [Sample("OSL to JFK itineraries", Description = "Join flights on destination = origin.")]
    public static IEnumerable<string> Itineraries() => OslToJfk().Select(x => $"{x.First.Code}+{x.Second.Code}").Order();

    [Sample("Cheapest OSL to JFK price", Description = "Min over the joined itineraries.")]
    public static int CheapestItinerary() => OslToJfk().Min(x => x.First.Price + x.Second.Price);

    [Sample("Busiest destination", Description = "CountBy destination, then MaxBy.")]
    public static string Busiest() => Flights.CountBy(f => f.To).MaxBy(c => c.Value).Key;

    [Sample("Flights with a return", Description = "Any flight that flies the reverse route.")]
    public static IEnumerable<string> WithReturn() =>
        Flights.Where(f => Flights.Any(r => r.From == f.To && r.To == f.From)).Select(f => f.Code).Order();
}
