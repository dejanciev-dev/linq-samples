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

    [Sample("Average price", Description = "Average over all flights.")]
    public static double AveragePrice() => Flights.Average(f => f.Price);

    [Sample("Short-haul flights", Description = "Flights under 200 minutes.")]
    public static IEnumerable<string> ShortHaul() => Flights.Where(f => f.Minutes < 200).Select(f => f.Code);

    [Sample("Arrivals per airport", Description = "CountBy destination.")]
    public static IEnumerable<string> Arrivals() => Flights.CountBy(f => f.To).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Fare total per origin", Description = "Group by origin and sum prices.")]
    public static IEnumerable<string> FareByOrigin() =>
        Flights.GroupBy(f => f.From).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(f => f.Price)}");

    [Sample("Priciest per minute", Description = "MaxBy on a computed price per minute.")]
    public static string PriciestPerMinute() => Flights.MaxBy(f => (double)f.Price / f.Minutes)!.Code;

    [Sample("Dead-end airports", Description = "Destinations that are never an origin (Except).")]
    public static IEnumerable<string> DeadEnds() => Flights.Select(f => f.To).Except(Flights.Select(f => f.From)).Order();

    [Sample("Reachable from OSL in two hops", Description = "Union of direct and connecting destinations.")]
    public static IEnumerable<string> TwoHops()
    {
        var first = Flights.Where(f => f.From == "OSL").Select(f => f.To).ToList();
        return first.Union(Flights.Where(f => first.Contains(f.From)).Select(f => f.To)).Order();
    }

    [Sample("Price bands", Description = "CountBy price rounded down to 200.")]
    public static IEnumerable<string> PriceBands() => Flights.CountBy(f => f.Price / 200 * 200).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Two longest flights", Description = "OrderByDescending then Take.")]
    public static IEnumerable<string> TwoLongest() => Flights.OrderByDescending(f => f.Minutes).Take(2).Select(f => f.Code);

    [Sample("OSL-LHR round trip price", Description = "Join on the reversed route with composite keys.")]
    public static int RoundTrip() =>
        Flights.Where(f => f is { From: "OSL", To: "LHR" })
            .Join(Flights, a => (a.To, a.From), b => (b.From, b.To), (a, b) => a.Price + b.Price).Single();
}
