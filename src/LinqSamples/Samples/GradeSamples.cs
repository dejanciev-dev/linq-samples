namespace LinqSamples.Samples;

/// <summary>Student scores, averages, letter grades and rankings with LINQ.</summary>
[SampleCategory("Student grades", 47)]
public static class GradeSamples
{
    private sealed record Score(string Student, string Subject, int Points);

    private static readonly Score[] Scores =
    [
        new("Ann", "math", 90), new("Ann", "art", 70),
        new("Bob", "math", 60), new("Bob", "art", 80),
        new("Cy", "math", 75), new("Cy", "art", 95),
        new("Di", "math", 50),
    ];

    private static string Letter(int p) => p switch { >= 90 => "A", >= 80 => "B", >= 70 => "C", _ => "F" };

    private static IEnumerable<(string Student, double Avg)> Averages() =>
        Scores.GroupBy(s => s.Student).Select(g => (g.Key, g.Average(s => s.Points)));

    [Sample("Average per student", Description = "GroupBy student and average the points.")]
    public static IEnumerable<string> AveragePerStudent() => Averages().OrderBy(a => a.Student).Select(a => $"{a.Student}:{a.Avg}");

    [Sample("Top student", Description = "MaxBy on the average.")]
    public static string TopStudent() => Averages().MaxBy(a => a.Avg).Student;

    [Sample("Passing students", Description = "Average of at least 60, sorted by name.")]
    public static IEnumerable<string> Passing() => Averages().Where(a => a.Avg >= 60).Select(a => a.Student).Order();

    [Sample("Best score per subject", Description = "GroupBy subject then MaxBy points.")]
    public static IEnumerable<string> BestPerSubject() =>
        Scores.GroupBy(s => s.Subject).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MaxBy(s => s.Points)!.Student}");

    [Sample("Letter grade counts", Description = "Map points to letters, then CountBy.")]
    public static IEnumerable<string> LetterCounts() =>
        Scores.CountBy(s => Letter(s.Points)).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Honor roll", Description = "All scores at least 70.")]
    public static IEnumerable<string> HonorRoll() =>
        Scores.GroupBy(s => s.Student).Where(g => g.All(s => s.Points >= 70)).Select(g => g.Key).Order();

    [Sample("Students with both subjects", Description = "Group size equals the number of subjects.")]
    public static IEnumerable<string> BothSubjects() =>
        Scores.GroupBy(s => s.Student).Where(g => g.Select(s => s.Subject).Distinct().Count() == 2).Select(g => g.Key).Order();

    [Sample("Better at art than math", Description = "Compare two scores inside each group.")]
    public static IEnumerable<string> BetterAtArt() =>
        Scores.GroupBy(s => s.Student)
            .Where(g => g.Any(s => s.Subject == "art") && g.Any(s => s.Subject == "math"))
            .Where(g => g.First(s => s.Subject == "art").Points > g.First(s => s.Subject == "math").Points)
            .Select(g => g.Key).Order();

    [Sample("Ranking", Description = "Order by average descending, then name.")]
    public static IEnumerable<string> Ranking() =>
        Averages().OrderByDescending(a => a.Avg).ThenBy(a => a.Student).Select(a => a.Student);

    [Sample("Total points", Description = "Sum of every score.")]
    public static int TotalPoints() => Scores.Sum(s => s.Points);

    [Sample("Median score", Description = "Order the points and take the middle one.")]
    public static int MedianScore() => Scores.Select(s => s.Points).Order().ElementAt(Scores.Length / 2);

    [Sample("Score spread per subject", Description = "Max minus min inside each subject group.")]
    public static IEnumerable<string> SpreadPerSubject() =>
        Scores.GroupBy(s => s.Subject).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Max(s => s.Points) - g.Min(s => s.Points)}");

    [Sample("Lowest score", Description = "MinBy points returns the student with the weakest result.")]
    public static string LowestScore() => Scores.MinBy(s => s.Points)!.Student;
}
