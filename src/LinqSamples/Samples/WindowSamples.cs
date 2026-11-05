namespace LinqSamples.Samples;

/// <summary>Streaks, run-length encoding and sliding windows with LINQ.</summary>
[SampleCategory("Streaks & sliding windows", 37)]
public static class WindowSamples
{
    private static readonly int[] Values = [3, 5, 5, 2, 2, 2, 8, 1, 1, 9];

    [Sample("Sliding window sums", Description = "Range of start indexes, then Skip/Take/Sum for each window of 3.")]
    public static IEnumerable<int> WindowSums() =>
        Enumerable.Range(0, Values.Length - 2).Select(i => Values.Skip(i).Take(3).Sum());

    [Sample("Moving average", Description = "Same windowing, averaged and rounded.")]
    public static IEnumerable<double> MovingAverage() =>
        Enumerable.Range(0, Values.Length - 2).Select(i => Math.Round(Values.Skip(i).Take(3).Average(), 2));

    [Sample("Pairwise differences", Description = "Zip a sequence with itself shifted by one.")]
    public static IEnumerable<int> Differences() => Values.Zip(Values.Skip(1), (a, b) => b - a);

    [Sample("Run-length encoding", Description = "Aggregate over the values, extending or starting runs.")]
    public static IEnumerable<string> RunLengthEncode() =>
        Values.Aggregate(new List<(int Value, int Count)>(), (acc, v) =>
        {
            if (acc.Count > 0 && acc[^1].Value == v) acc[^1] = (v, acc[^1].Count + 1);
            else acc.Add((v, 1));
            return acc;
        }).Select(r => $"{r.Value}x{r.Count}");

    [Sample("Run-length decoding", Description = "SelectMany with Enumerable.Repeat expands each pair.")]
    public static IEnumerable<int> RunLengthDecode() =>
        new[] { (7, 2), (4, 3) }.SelectMany(p => Enumerable.Repeat(p.Item1, p.Item2));

    [Sample("Longest streak", Description = "Count each run of equal values and take the maximum.")]
    public static int LongestStreak() =>
        Values.Aggregate((Run: 0, Max: 0, Prev: (int?)null), (s, v) =>
        {
            var run = s.Prev == v ? s.Run + 1 : 1;
            return (run, Math.Max(s.Max, run), v);
        }).Max;

    [Sample("Remove consecutive duplicates", Description = "Keep a value only when it differs from its predecessor.")]
    public static IEnumerable<int> Dedupe() =>
        Values.Where((v, i) => i == 0 || Values[i - 1] != v);

    [Sample("Increasing streak positions", Description = "Indexes where a value is greater than the one before it.")]
    public static IEnumerable<int> RisingIndexes() =>
        Values.Select((v, i) => (v, i)).Where(x => x.i > 0 && x.v > Values[x.i - 1]).Select(x => x.i);

    [Sample("Split on separator", Description = "Group by the number of separators seen so far to split a sequence.")]
    public static IEnumerable<string> SplitOnZero()
    {
        int[] data = [1, 2, 0, 3, 0, 4, 5, 6];
        var seen = 0;
        return data.GroupBy(n => n == 0 ? seen++ : seen)
            .Select(g => string.Join(",", g.Where(n => n != 0)));
    }

    [Sample("Best window", Description = "Pick the start index of the window with the largest sum.")]
    public static int BestWindowStart() =>
        Enumerable.Range(0, Values.Length - 2).MaxBy(i => Values.Skip(i).Take(3).Sum());
}
