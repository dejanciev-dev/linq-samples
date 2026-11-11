namespace LinqSamples.Samples;

/// <summary>Bike rentals: riders, stations, ride minutes and e-bikes.</summary>
[SampleCategory("Bike rentals", 65)]
public static class BikeSamples
{
    private sealed record Ride(string Rider, string Station, int Minutes, bool Electric);

    private static readonly Ride[] Rides =
    [
        new("Ann", "North", 30, true), new("Bob", "South", 15, false), new("Ann", "South", 45, false),
        new("Cid", "North", 20, true), new("Bob", "North", 60, true), new("Dee", "East", 10, false),
    ];

    [Sample("Total minutes", Description = "Sum of all ride minutes.")]
    public static int TotalMinutes() => Rides.Sum(r => r.Minutes);

    [Sample("Minutes per station", Description = "Group by station and sum minutes.")]
    public static IEnumerable<string> MinutesPerStation() =>
        Rides.GroupBy(r => r.Station).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(r => r.Minutes)}");

    [Sample("E-bike rides", Description = "Count rides on electric bikes.")]
    public static int ElectricRides() => Rides.Count(r => r.Electric);

    [Sample("Longest ride", Description = "MaxBy minutes.")]
    public static string LongestRide() => Rides.MaxBy(r => r.Minutes)!.Rider;

    [Sample("Riders", Description = "Distinct rider names.")]
    public static IEnumerable<string> Riders() => Rides.Select(r => r.Rider).Distinct().Order();

    [Sample("Average ride", Description = "Average ride length in minutes.")]
    public static double AverageMinutes() => Rides.Average(r => r.Minutes);

    [Sample("Rides per rider", Description = "CountBy rider.")]
    public static IEnumerable<string> RidesPerRider() => Rides.CountBy(r => r.Rider).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Short rides", Description = "Riders with a ride of 15 minutes or less.")]
    public static IEnumerable<string> ShortRides() => Rides.Where(r => r.Minutes <= 15).Select(r => r.Rider).Distinct().Order();

    [Sample("Electric vs classic", Description = "ToLookup on a bool key to split minutes.")]
    public static IEnumerable<string> ElectricVsClassic()
    {
        var lookup = Rides.ToLookup(r => r.Electric);
        return [$"electric:{lookup[true].Sum(r => r.Minutes)}", $"classic:{lookup[false].Sum(r => r.Minutes)}"];
    }

    [Sample("Busiest station", Description = "CountBy station, then MaxBy count.")]
    public static string BusiestStation() => Rides.CountBy(r => r.Station).MaxBy(c => c.Value).Key;
}
