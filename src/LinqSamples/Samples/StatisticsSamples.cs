namespace LinqSamples.Samples;

/// <summary>
/// Descriptive statistics with plain LINQ. Results are formatted culture-invariantly so the
/// decimal separator is always a dot.
/// </summary>
[SampleCategory("Statistics", 32)]
public static class StatisticsSamples
{
    private static readonly int[] Scores = [72, 85, 90, 66, 85, 78, 95, 85, 60, 88];

    [Sample("Mean, min, max, range", Description = "The basic summary of a data set.")]
    public static IEnumerable<string> Summary() =>
    [
        FormattableString.Invariant($"Mean:  {Scores.Average()}"),
        $"Min:   {Scores.Min()}",
        $"Max:   {Scores.Max()}",
        $"Range: {Scores.Max() - Scores.Min()}",
    ];

    [Sample("Median", Description = "Sort, then take the middle value (or the average of the two middle values).")]
    public static double Median()
    {
        var sorted = Scores.Order().ToArray();
        var mid = sorted.Length / 2;
        return sorted.Length % 2 == 0 ? (sorted[mid - 1] + sorted[mid]) / 2.0 : sorted[mid];
    }

    [Sample("Mode", Description = "The most frequent values - there can be more than one.")]
    public static IEnumerable<int> Mode()
    {
        var counts = Scores.CountBy(s => s).ToList();
        var top = counts.Max(kv => kv.Value);
        return counts.Where(kv => kv.Value == top).Select(kv => kv.Key);
    }

    [Sample("Standard deviation", Description = "Population (divide by n) vs sample (divide by n - 1).")]
    public static IEnumerable<string> StandardDeviation()
    {
        var mean = Scores.Average();
        var squares = Scores.Sum(s => (s - mean) * (s - mean));
        return
        [
            FormattableString.Invariant($"Population SD: {Math.Sqrt(squares / Scores.Length):0.00}"),
            FormattableString.Invariant($"Sample SD:     {Math.Sqrt(squares / (Scores.Length - 1)):0.00}"),
        ];
    }

    [Sample("Percentiles", Description = "Nearest-rank method: the value below which p% of the data falls.")]
    public static IEnumerable<string> Percentiles()
    {
        var sorted = Scores.Order().ToArray();
        return new[] { 25, 50, 90 }.Select(p => $"P{p}: {Percentile(sorted, p)}");
    }

    [Sample("Outliers (IQR rule)", Description = "Values beyond 1.5 x IQR outside the quartiles - here a slow response time.")]
    public static IEnumerable<string> Outliers()
    {
        int[] responseTimes = [120, 135, 128, 140, 122, 900, 131, 125];
        var sorted = responseTimes.Order().ToArray();
        var (q1, q3) = (Percentile(sorted, 25), Percentile(sorted, 75));
        var iqr = q3 - q1;
        var outliers = responseTimes.Where(t => t < q1 - 1.5 * iqr || t > q3 + 1.5 * iqr);
        return [$"Q1: {q1}, Q3: {q3}, IQR: {iqr}", $"Outliers: {string.Join(", ", outliers)}"];
    }

    [Sample("Z-scores", Description = "How many standard deviations each value is from the mean - shown for the extremes.")]
    public static IEnumerable<string> ZScores()
    {
        var mean = Scores.Average();
        var sd = Math.Sqrt(Scores.Average(s => (s - mean) * (s - mean)));
        return new[] { Scores.Min(), Scores.Max() }
            .Select(s => FormattableString.Invariant($"{s}: {(s - mean) / sd:0.00}"));
    }

    [Sample("Min-max normalization", Description = "Scale values into the range 0..1.")]
    public static IEnumerable<double> Normalize()
    {
        int[] values = [10, 20, 15, 30];
        var (min, max) = (values.Min(), values.Max());
        return values.Select(v => (double)(v - min) / (max - min));
    }

    [Sample("Correlation", Description = "Pearson's r between hours studied and score, using Zip over the deviations.")]
    public static string Correlation()
    {
        var (hours, scores) = StudyData();
        var (mx, my) = (hours.Average(), scores.Average());
        var covariance = hours.Zip(scores, (x, y) => (x - mx) * (y - my)).Sum();
        var r = covariance / Math.Sqrt(hours.Sum(x => (x - mx) * (x - mx)) * scores.Sum(y => (y - my) * (y - my)));
        return FormattableString.Invariant($"r = {r:0.000}");
    }

    [Sample("Linear regression", Description = "Least-squares line through the points, then a prediction.")]
    public static IEnumerable<string> LinearRegression()
    {
        var (hours, scores) = StudyData();
        var (mx, my) = (hours.Average(), scores.Average());
        var slope = hours.Zip(scores, (x, y) => (x - mx) * (y - my)).Sum() / hours.Sum(x => (x - mx) * (x - mx));
        var intercept = my - slope * mx;
        return
        [
            FormattableString.Invariant($"y = {slope:0.0}x + {intercept:0.0}"),
            FormattableString.Invariant($"Prediction for 6 hours: {slope * 6 + intercept:0.0}"),
        ];
    }

    [Sample("Weighted average", Description = "Grades weighted by course credits.")]
    public static string WeightedAverage()
    {
        (int Grade, int Credits)[] courses = [(90, 3), (80, 4), (70, 2)];
        var average = (double)courses.Sum(c => c.Grade * c.Credits) / courses.Sum(c => c.Credits);
        return FormattableString.Invariant($"{average:0.00}");
    }

    [Sample("Histogram", Description = "Bucket into ranges of 10 and draw bars with new string('#', count).")]
    public static IEnumerable<string> Histogram() =>
        Scores.GroupBy(s => s / 10 * 10)
              .OrderBy(g => g.Key)
              .Select(g => $"{g.Key}-{g.Key + 9} | {new string('#', g.Count())}");

    [Sample("Cumulative distribution", Description = "The share of values less than or equal to each value.")]
    public static IEnumerable<string> CumulativeDistribution() =>
        Scores.Distinct()
              .Order()
              .Select(v => FormattableString.Invariant($"{v}: {100.0 * Scores.Count(s => s <= v) / Scores.Length:0}%"));

    [Sample("Geometric mean", Description = "The average growth rate: exp of the average log.")]
    public static string GeometricMean()
    {
        double[] growthFactors = [1.10, 0.95, 1.20];
        var mean = Math.Exp(growthFactors.Average(Math.Log));
        return FormattableString.Invariant($"Average growth: {(mean - 1) * 100:0.00}%");
    }

    [Sample("Root mean square error", Description = "Square, average, square root - a common measure of prediction error.")]
    public static string RootMeanSquareError()
    {
        int[] errors = [3, -4, 5, -2];
        return FormattableString.Invariant($"RMSE: {Math.Sqrt(errors.Average(e => (double)e * e)):0.00}");
    }

    private static int Percentile(int[] sorted, double percent) =>
        sorted[(int)Math.Ceiling(percent / 100 * sorted.Length) - 1];

    private static (double[] Hours, double[] Scores) StudyData() =>
        ([1, 2, 3, 4, 5], [52, 60, 68, 74, 86]);
}
