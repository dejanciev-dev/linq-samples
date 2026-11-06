namespace LinqSamples.Samples;

/// <summary>Hotel bookings: nights, rooms and revenue.</summary>
[SampleCategory("Hotel bookings", 59)]
public static class HotelSamples
{
    private sealed record Booking(string Guest, string Room, int Nights, int Rate, string Month);

    private static readonly Booking[] Bookings =
    [
        new("Ann", "101", 3, 100, "Jan"), new("Bob", "102", 2, 120, "Jan"), new("Cy", "101", 5, 100, "Feb"),
        new("Di", "201", 1, 200, "Feb"), new("Ed", "102", 4, 120, "Mar"), new("Ann", "201", 2, 200, "Mar"),
    ];

    [Sample("Total nights", Description = "Sum of nights.")]
    public static int TotalNights() => Bookings.Sum(b => b.Nights);

    [Sample("Total revenue", Description = "Sum of nights times rate.")]
    public static int Revenue() => Bookings.Sum(b => b.Nights * b.Rate);

    [Sample("Longest stay", Description = "MaxBy nights.")]
    public static string LongestStay() => Bookings.MaxBy(b => b.Nights)!.Guest;

    [Sample("Repeat guests", Description = "Groups with more than one booking.")]
    public static IEnumerable<string> RepeatGuests() => Bookings.GroupBy(b => b.Guest).Where(g => g.Count() > 1).Select(g => g.Key);

    [Sample("Nights per room", Description = "Group by room and sum nights.")]
    public static IEnumerable<string> NightsPerRoom() =>
        Bookings.GroupBy(b => b.Room).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(b => b.Nights)}");

    [Sample("Revenue per month", Description = "Group by month, keeping first-seen order.")]
    public static IEnumerable<string> RevenuePerMonth() =>
        Bookings.GroupBy(b => b.Month).Select(g => $"{g.Key}:{g.Sum(b => b.Nights * b.Rate)}");

    [Sample("Best room", Description = "Room with the highest revenue.")]
    public static string BestRoom() =>
        Bookings.GroupBy(b => b.Room).MaxBy(g => g.Sum(b => b.Nights * b.Rate))!.Key;

    [Sample("Average rate", Description = "Average of the nightly rate.")]
    public static double AverageRate() => Bookings.Average(b => b.Rate);

    [Sample("Long stays", Description = "Count bookings of three nights or more.")]
    public static int LongStays() => Bookings.Count(b => b.Nights >= 3);

    [Sample("Guest spend ranking", Description = "Group, sum, then OrderByDescending.")]
    public static IEnumerable<string> GuestSpend() =>
        Bookings.GroupBy(b => b.Guest).Select(g => (Guest: g.Key, Spend: g.Sum(b => b.Nights * b.Rate)))
            .OrderByDescending(x => x.Spend).Select(x => $"{x.Guest}:{x.Spend}");
}
