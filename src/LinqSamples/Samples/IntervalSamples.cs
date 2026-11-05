namespace LinqSamples.Samples;

/// <summary>Intervals, ranges and scheduling with LINQ.</summary>
[SampleCategory("Intervals & scheduling", 36)]
public static class IntervalSamples
{
    private static readonly (int Start, int End)[] Meetings = [(9, 11), (10, 12), (13, 14), (14, 16), (15, 17)];

    [Sample("Overlapping pairs", Description = "Self-join on index order; intervals overlap when each starts before the other ends.")]
    public static IEnumerable<string> OverlappingPairs() =>
        from i in Enumerable.Range(0, Meetings.Length)
        from j in Enumerable.Range(i + 1, Meetings.Length - i - 1)
        where Meetings[i].Start < Meetings[j].End && Meetings[j].Start < Meetings[i].End
        select $"{Meetings[i]}-{Meetings[j]}";

    [Sample("Merge intervals", Description = "Sort by start, then Aggregate extends the last merged interval or starts a new one.")]
    public static IEnumerable<(int Start, int End)> Merge() =>
        Meetings.OrderBy(m => m.Start).Aggregate(new List<(int Start, int End)>(), (acc, m) =>
        {
            if (acc.Count > 0 && m.Start <= acc[^1].End) acc[^1] = (acc[^1].Start, Math.Max(acc[^1].End, m.End));
            else acc.Add(m);
            return acc;
        });

    [Sample("Total busy hours", Description = "Merge first, then sum the lengths of the merged intervals.")]
    public static int BusyHours() => Merge().Sum(m => m.End - m.Start);

    [Sample("Free slots", Description = "Zip merged intervals with their successors to find the gaps.")]
    public static IEnumerable<(int Start, int End)> FreeSlots()
    {
        var merged = Merge().ToList();
        return merged.Zip(merged.Skip(1), (a, b) => (a.End, b.Start)).Where(g => g.Start > g.End);
    }

    [Sample("Max concurrent meetings", Description = "Sweep line: +1 at starts, -1 at ends, running sum, take the peak.")]
    public static int MaxConcurrent() =>
        Meetings.SelectMany(m => new[] { (Time: m.Start, Delta: 1), (Time: m.End, Delta: -1) })
            .OrderBy(e => e.Time).ThenBy(e => e.Delta)
            .Select(e => e.Delta)
            .Scan(0, (a, d) => a + d)
            .Max();

    [Sample("Point in intervals", Description = "Count the intervals containing a point with Count.")]
    public static int ContainingPoint() => Meetings.Count(m => m.Start <= 15 && 15 < m.End);

    [Sample("Greedy activity selection", Description = "Sort by end time, keep each interval that starts after the last kept one ends.")]
    public static IEnumerable<(int Start, int End)> MaxNonOverlapping() =>
        Meetings.OrderBy(m => m.End).Aggregate(new List<(int Start, int End)>(), (acc, m) =>
        {
            if (acc.Count == 0 || m.Start >= acc[^1].End) acc.Add(m);
            return acc;
        });

    [Sample("Missing ranges", Description = "Group consecutive numbers by value minus index and show first-last.")]
    public static IEnumerable<string> ConsecutiveRuns() =>
        new[] { 1, 2, 3, 7, 8, 10 }
            .Select((n, i) => (n, key: n - i))
            .GroupBy(x => x.key)
            .Select(g => g.Count() == 1 ? $"{g.First().n}" : $"{g.First().n}-{g.Last().n}");

    [Sample("Bucket into ranges", Description = "Integer division maps each value to a fixed-width bucket.")]
    public static IEnumerable<string> Buckets() =>
        new[] { 3, 7, 12, 18, 21, 25, 29 }
            .GroupBy(n => n / 10 * 10)
            .OrderBy(g => g.Key)
            .Select(g => $"{g.Key}-{g.Key + 9}: {g.Count()}");

    [Sample("Split range into chunks", Description = "Chunk a Range and report each first-last pair.")]
    public static IEnumerable<string> RangeChunks() =>
        Enumerable.Range(1, 10).Chunk(4).Select(c => $"{c[0]}-{c[^1]}");
}
