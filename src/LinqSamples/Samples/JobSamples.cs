namespace LinqSamples.Samples;

/// <summary>A to-do list with priorities and estimates.</summary>
[SampleCategory("To-do list", 57)]
public static class JobSamples
{
    private sealed record Job(int Id, string Title, int Priority, bool Done, int Hours);

    private static readonly Job[] Jobs =
    [
        new(1, "write", 2, true, 3), new(2, "test", 1, false, 2), new(3, "deploy", 3, false, 1),
        new(4, "review", 2, false, 4), new(5, "plan", 1, true, 2),
    ];

    [Sample("Open jobs by priority", Description = "Filter, then OrderByDescending and ThenBy.")]
    public static IEnumerable<string> OpenByPriority() => Jobs.Where(j => !j.Done).OrderByDescending(j => j.Priority).ThenBy(j => j.Id).Select(j => j.Title);

    [Sample("Hours remaining", Description = "Sum of open estimates.")]
    public static int HoursRemaining() => Jobs.Where(j => !j.Done).Sum(j => j.Hours);

    [Sample("Percent done", Description = "Integer percentage of finished jobs.")]
    public static int PercentDone() => Jobs.Count(j => j.Done) * 100 / Jobs.Length;

    [Sample("Hours per priority", Description = "GroupBy priority and sum hours.")]
    public static IEnumerable<string> HoursPerPriority() => Jobs.GroupBy(j => j.Priority).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(j => j.Hours)}");

    [Sample("Next job", Description = "Highest priority open job.")]
    public static string Next() => Jobs.Where(j => !j.Done).OrderByDescending(j => j.Priority).First().Title;

    [Sample("Urgent open job exists", Description = "Any with two conditions.")]
    public static bool HasUrgent() => Jobs.Any(j => !j.Done && j.Priority == 3);

    [Sample("Long titles", Description = "Titles longer than four letters, original order.")]
    public static IEnumerable<string> LongTitles() => Jobs.Where(j => j.Title.Length > 4).Select(j => j.Title);

    [Sample("Pairs of ids", Description = "Chunk into groups of two.")]
    public static IEnumerable<string> IdPairs() => Jobs.Select(j => j.Id).Chunk(2).Select(c => string.Join(",", c));

    [Sample("Done versus open", Description = "GroupBy the flag and count both sides.")]
    public static string DoneVersusOpen()
    {
        var p = Jobs.GroupBy(j => j.Done).ToDictionary(g => g.Key, g => g.Count());
        return $"done:{p[true]} open:{p[false]}";
    }

    [Sample("Longest title", Description = "Order by length descending, then alphabetically.")]
    public static string LongestTitle() => Jobs.OrderByDescending(j => j.Title.Length).ThenBy(j => j.Title).First().Title;
}
