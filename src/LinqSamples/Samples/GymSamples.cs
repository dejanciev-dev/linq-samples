namespace LinqSamples.Samples;

/// <summary>Workout log: minutes and calories per member.</summary>
[SampleCategory("Gym workouts", 61)]
public static class GymSamples
{
    private sealed record Workout(string Member, string Day, int Minutes, int Calories);

    private static readonly Workout[] Log =
    [
        new("Ann", "Mon", 30, 200), new("Ann", "Wed", 45, 300), new("Bob", "Mon", 60, 450), new("Bob", "Tue", 20, 100),
        new("Cy", "Wed", 40, 280), new("Cy", "Thu", 50, 350), new("Cy", "Fri", 30, 210),
    ];

    [Sample("Total minutes", Description = "Sum of all workout minutes.")]
    public static int TotalMinutes() => Log.Sum(w => w.Minutes);

    [Sample("Minutes per member", Description = "Group by member and sum minutes.")]
    public static IEnumerable<string> MinutesPerMember() => Log.GroupBy(w => w.Member).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(w => w.Minutes)}");

    [Sample("Most active member", Description = "MaxBy over grouped minutes.")]
    public static string MostActive() => Log.GroupBy(w => w.Member).MaxBy(g => g.Sum(w => w.Minutes))!.Key;

    [Sample("Busy days", Description = "Days with more than one workout.")]
    public static IEnumerable<string> BusyDays() => Log.CountBy(w => w.Day).Where(c => c.Value > 1).Select(c => c.Key).Order();

    [Sample("Average calories", Description = "Average of calories per workout.")]
    public static double AverageCalories() => Log.Average(w => w.Calories);

    [Sample("Most efficient member", Description = "MaxBy calories per minute.")]
    public static string MostEfficient() => Log.MaxBy(w => (double)w.Calories / w.Minutes)!.Member;

    [Sample("Long sessions", Description = "Count sessions of 45 minutes or more.")]
    public static int LongSessions() => Log.Count(w => w.Minutes >= 45);

    [Sample("Calories ranking", Description = "Total calories per member, highest first.")]
    public static IEnumerable<string> CaloriesRanking() =>
        Log.GroupBy(w => w.Member).Select(g => (g.Key, Total: g.Sum(w => w.Calories))).OrderByDescending(t => t.Total).Select(t => $"{t.Key}:{t.Total}");

    [Sample("Monday members", Description = "Distinct members who trained on Monday.")]
    public static IEnumerable<string> MondayMembers() => Log.Where(w => w.Day == "Mon").Select(w => w.Member).Distinct().Order();

    [Sample("Top two sessions", Description = "OrderByDescending then Take(2).")]
    public static IEnumerable<string> TopTwo() => Log.OrderByDescending(w => w.Calories).Take(2).Select(w => w.Member);
}
