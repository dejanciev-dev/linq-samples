namespace LinqSamples.Samples;

/// <summary>Room bookings: overlaps, merged intervals, free slots and peak load.</summary>
[SampleCategory("Room bookings", 81)]
public static class BookingSamples
{
    private sealed record Meeting(string Room, int Start, int End);

    private static readonly Meeting[] Meetings =
    [
        new("R1", 9, 11), new("R1", 10, 12), new("R1", 13, 14),
        new("R2", 9, 10), new("R2", 10, 11), new("R2", 11, 13),
    ];

    // Overlapping or touching meetings in a room are merged into one busy interval.
    private static IEnumerable<(string Room, int Start, int End)> Merged() =>
        Meetings.GroupBy(m => m.Room).OrderBy(g => g.Key, StringComparer.Ordinal)
                .SelectMany(g => g.OrderBy(m => m.Start)
                                  .Aggregate(new List<(int S, int E)>(), (acc, m) =>
                                  {
                                      if (acc.Count > 0 && m.Start <= acc[^1].E)
                                          acc[^1] = (acc[^1].S, Math.Max(acc[^1].E, m.End));
                                      else
                                          acc.Add((m.Start, m.End));
                                      return acc;
                                  })
                                  .Select(x => (g.Key, x.S, x.E)));

    [Sample("Overlapping meetings", Description = "Self-join on index to list each conflicting pair once.")]
    public static IEnumerable<string> Conflicts() =>
        from a in Meetings.Index()
        from b in Meetings.Index()
        where a.Index < b.Index && a.Item.Room == b.Item.Room
              && a.Item.Start < b.Item.End && b.Item.Start < a.Item.End
        select $"{a.Item.Room}:{a.Item.Start}-{a.Item.End}/{b.Item.Start}-{b.Item.End}";

    [Sample("Hours booked per room", Description = "Sum durations per group (overlaps counted twice).")]
    public static IEnumerable<string> HoursPerRoom() =>
        Meetings.GroupBy(m => m.Room).OrderBy(g => g.Key, StringComparer.Ordinal).Select(g => $"{g.Key}:{g.Sum(m => m.End - m.Start)}");

    [Sample("Merged busy intervals", Description = "Sort by start, then merge with Aggregate.")]
    public static IEnumerable<string> MergedIntervals() =>
        Merged().GroupBy(r => r.Room).Select(g => $"{g.Key}:{string.Join(",", g.Select(r => $"{r.Start}-{r.End}"))}");

    [Sample("Free hours per room", Description = "Hours between 9 and 14 not covered by a merged interval.")]
    public static IEnumerable<string> FreeHours() =>
        Merged().GroupBy(r => r.Room)
                .Select(g => $"{g.Key}:{string.Join(",", Enumerable.Range(9, 5).Where(h => !g.Any(r => h >= r.Start && h < r.End)))}");

    [Sample("Peak hour", Description = "Count meetings covering each hour, take the maximum.")]
    public static string PeakHour()
    {
        var peak = Enumerable.Range(9, 5)
                             .Select(h => (Hour: h, Count: Meetings.Count(m => m.Start <= h && h < m.End)))
                             .MaxBy(x => x.Count);
        return $"{peak.Hour}:{peak.Count}";
    }

    [Sample("Back-to-back meetings", Description = "Join on a composite key: same room, end equals start.")]
    public static IEnumerable<string> BackToBack() =>
        from a in Meetings
        join b in Meetings on (a.Room, a.End) equals (b.Room, b.Start)
        select $"{a.Room}:{a.Start}-{a.End}>{b.Start}-{b.End}";

    [Sample("Rooms free at noon", Description = "All over each group.")]
    public static IEnumerable<string> FreeAtNoon() =>
        Meetings.GroupBy(m => m.Room).Where(g => g.All(m => !(m.Start <= 12 && 12 < m.End))).Select(g => g.Key).Order();

    [Sample("Distinct busy hours", Description = "Expand each meeting into hours with SelectMany, then Distinct.")]
    public static int BusyHours() =>
        Meetings.SelectMany(m => Enumerable.Range(m.Start, m.End - m.Start)).Distinct().Count();

    [Sample("Meetings per start hour", Description = "CountBy a computed key, then sort.")]
    public static IEnumerable<string> StartHours() =>
        Meetings.CountBy(m => m.Start).OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}:{kv.Value}");

    [Sample("Longest meeting per room", Description = "Order by duration, break ties by earliest start.")]
    public static IEnumerable<string> LongestPerRoom() =>
        Meetings.GroupBy(m => m.Room).OrderBy(g => g.Key, StringComparer.Ordinal)
                .Select(g => g.OrderByDescending(m => m.End - m.Start).ThenBy(m => m.Start).First())
                .Select(m => $"{m.Room}:{m.Start}-{m.End}");
}
