namespace LinqSamples.Samples;

/// <summary>Parking tickets: plates, lots, hours and fees.</summary>
[SampleCategory("Parking tickets", 62)]
public static class ParkingSamples
{
    private sealed record Ticket(string Plate, string Lot, int Hours, int Fee);

    private static readonly Ticket[] Tickets =
    [
        new("A1", "North", 2, 4), new("B2", "North", 5, 10), new("C3", "South", 1, 2),
        new("A1", "South", 3, 6), new("D4", "East", 8, 16), new("B2", "East", 2, 4),
    ];

    [Sample("Total fees", Description = "Sum of all fees.")]
    public static int TotalFees() => Tickets.Sum(t => t.Fee);

    [Sample("Fees per lot", Description = "Group by lot and sum fees.")]
    public static IEnumerable<string> FeesPerLot() => Tickets.GroupBy(t => t.Lot).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(t => t.Fee)}");

    [Sample("Longest stay", Description = "MaxBy hours.")]
    public static string LongestStay() => Tickets.MaxBy(t => t.Hours)!.Plate;

    [Sample("Repeat plates", Description = "Plates with more than one ticket.")]
    public static IEnumerable<string> RepeatPlates() => Tickets.CountBy(t => t.Plate).Where(c => c.Value > 1).Select(c => c.Key).Order();

    [Sample("Average hours", Description = "Average parked hours.")]
    public static double AverageHours() => Tickets.Average(t => t.Hours);

    [Sample("Lots per plate", Description = "Distinct lots visited by each plate.")]
    public static IEnumerable<string> LotsPerPlate() =>
        Tickets.GroupBy(t => t.Plate).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Select(t => t.Lot).Distinct().Count()}");

    [Sample("Over three hours", Description = "Plates parked more than 3 hours.")]
    public static IEnumerable<string> OverThreeHours() => Tickets.Where(t => t.Hours > 3).Select(t => t.Plate).Order();

    [Sample("Cheapest lot", Description = "MinBy over grouped fees.")]
    public static string CheapestLot() => Tickets.GroupBy(t => t.Lot).MinBy(g => g.Sum(t => t.Fee))!.Key;

    [Sample("Flat rate check", Description = "All fees equal hours times two.")]
    public static bool FlatRate() => Tickets.All(t => t.Fee == t.Hours * 2);

    [Sample("Last two tickets", Description = "TakeLast(2).")]
    public static IEnumerable<string> LastTwo() => Tickets.TakeLast(2).Select(t => t.Plate);
}
