namespace LinqSamples.Samples;

/// <summary>Taxi rides: drivers, distances, fares and ratings.</summary>
[SampleCategory("Taxi rides", 68)]
public static class TaxiSamples
{
    private sealed record Ride(string Driver, int Km, int Fare, int Rating);

    private static readonly Ride[] Rides =
    [
        new("Max", 5, 12, 5), new("Sue", 12, 25, 4), new("Max", 3, 8, 3),
        new("Joe", 20, 40, 5), new("Sue", 7, 15, 5), new("Joe", 2, 6, 2),
    ];

    [Sample("Total distance", Description = "Sum of kilometres driven.")]
    public static int TotalKm() => Rides.Sum(r => r.Km);

    [Sample("Earnings per driver", Description = "Group by driver and sum fares.")]
    public static IEnumerable<string> EarningsPerDriver() =>
        Rides.GroupBy(r => r.Driver).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(r => r.Fare)}");

    [Sample("Top earner", Description = "MaxBy over grouped fares.")]
    public static string TopEarner() => Rides.GroupBy(r => r.Driver).MaxBy(g => g.Sum(r => r.Fare))!.Key;

    [Sample("Longest ride", Description = "MaxBy kilometres.")]
    public static string LongestRide() => Rides.MaxBy(r => r.Km)!.Driver;

    [Sample("Five-star rides", Description = "Count rides rated 5.")]
    public static int FiveStarRides() => Rides.Count(r => r.Rating == 5);

    [Sample("Low-rated drivers", Description = "Drivers with any rating of 3 or less.")]
    public static IEnumerable<string> LowRatedDrivers() => Rides.Where(r => r.Rating <= 3).Select(r => r.Driver).Distinct().Order();

    [Sample("Average rating", Description = "Average of all ratings.")]
    public static double AverageRating() => Rides.Average(r => r.Rating);

    [Sample("Short trips", Description = "Count rides under 5 km.")]
    public static int ShortTrips() => Rides.Count(r => r.Km < 5);

    [Sample("Consistent drivers", Description = "Drivers whose every ride is rated 4 or more.")]
    public static IEnumerable<string> ConsistentDrivers() =>
        Rides.GroupBy(r => r.Driver).Where(g => g.All(r => r.Rating >= 4)).Select(g => g.Key).Order();

    [Sample("Fare range", Description = "Max fare minus min fare.")]
    public static int FareRange() => Rides.Max(r => r.Fare) - Rides.Min(r => r.Fare);
}
