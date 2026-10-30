namespace LinqSamples.Tests;

/// <summary>Pins the results of the pitfall, date/time-series and statistics samples.</summary>
public class PitfallDateStatisticsTests
{
    // Pitfalls

    [Fact]
    public void Any_stops_early() =>
        Assert.Equal(["Count() > 0 visited 10 items", "Any()       visited 1 item"], PitfallSamples.CountVsAny());

    [Fact]
    public void Single_reveals_duplicates() =>
        Assert.Equal(["First:  Bob", "Single: InvalidOperationException - more than one match"], PitfallSamples.FirstVsSingle());

    [Fact]
    public void Second_OrderBy_replaces_the_first() =>
        Assert.Equal(
            ["OrderBy.OrderBy: Pen, Notebook, Mouse", "OrderBy.ThenBy:  Mouse, Monitor, Laptop"],
            PitfallSamples.OrderByTwice());

    [Fact]
    public void Unenumerated_select_does_nothing() =>
        Assert.Equal(["Unenumerated Select processed: 0", "foreach processed:            10"], PitfallSamples.SelectForSideEffects());

    [Fact]
    public void For_loop_closures_share_the_variable() =>
        Assert.Equal(["Captured loop variable: 3, 3, 3", "Copied to a local:      0, 1, 2"], PitfallSamples.ClosuresInForLoop());

    [Fact]
    public void HashSet_lookups_are_constant() =>
        Assert.Equal(["List.Contains:    500500 comparisons", "HashSet.Contains: 1000 comparisons"], PitfallSamples.ContainsInLoop());

    [Fact]
    public void Distinct_on_classes_uses_reference_equality() =>
        Assert.Equal(["Class:      2 distinct", "Record:     1 distinct", "DistinctBy: 1 distinct"], PitfallSamples.DistinctOnClasses());

    [Fact]
    public void Modifying_while_enumerating_throws() =>
        Assert.Equal(["Remove inside foreach: InvalidOperationException", "RemoveAll:             7 left"], PitfallSamples.ModifyWhileEnumerating());

    [Fact]
    public void FirstOrDefault_on_value_types_is_ambiguous() =>
        Assert.Equal(
            ["FirstOrDefault:            0", "Cast to int? first:        null", "FirstOrDefault(pred, -1):  -1"],
            PitfallSamples.FirstOrDefaultValueTypes());

    [Fact]
    public void Sum_overflow() =>
        Assert.Equal(["Sum():               OverflowException", "Sum(n => (long)n):   2147483648"], PitfallSamples.SumOverflow());

    [Fact]
    public void Integer_division_truncates() =>
        Assert.Equal(["Sum() / Length: 5", "Average():      5.1"], PitfallSamples.IntegerDivision());

    [Fact]
    public void Reverse_is_not_in_place() =>
        Assert.Equal(
            ["numbers.Reverse():      1, 2, 3  (result discarded)", "Array.Reverse(numbers): 3, 2, 1"],
            PitfallSamples.ReverseIsNotInPlace());

    [Fact]
    public void Filter_before_project() =>
        Assert.Equal(["Select then Where: 7 projections", "Where then Select: 3 projections"], PitfallSamples.FilterBeforeProject());

    [Fact]
    public void Floating_point_keys() =>
        Assert.Equal(["Distinct:                2 values", "Distinct after rounding: 1 value"], PitfallSamples.FloatingPointKeys());

    [Fact]
    public void Disposed_resource() =>
        Assert.Equal(
            ["Enumerated after Dispose: ObjectDisposedException", "ToList() inside using:    alpha, beta"],
            PitfallSamples.DisposedResource());

    // Dates & time series

    [Fact]
    public void Group_by_iso_week() =>
        Assert.Equal(["W10: 650", "W11: 500"], DateSamples.GroupByWeek());

    [Fact]
    public void Fill_missing_days()
    {
        var days = DateSamples.FillMissingDays().ToList();
        Assert.Equal(13, days.Count);
        Assert.Equal(["2026-03-05: 0", "2026-03-08: 0", "2026-03-11: 0", "2026-03-13: 0"], days.Where(d => d.EndsWith(": 0", StringComparison.Ordinal)));
    }

    [Fact]
    public void Rolling_total()
    {
        var totals = DateSamples.RollingTotal().ToList();
        Assert.Equal(11, totals.Count);
        Assert.Equal(["2026-03-04: 300", "2026-03-05: 180", "2026-03-06: 250"], totals.Take(3));
    }

    [Fact]
    public void Day_over_day() =>
        Assert.Equal(
            ["2026-03-03: -40", "2026-03-04: +20", "2026-03-06: +50", "2026-03-07: +50", "2026-03-09: -110", "2026-03-10: +20", "2026-03-12: +20", "2026-03-14: +40"],
            DateSamples.DayOverDay());

    [Fact]
    public void Weekday_vs_weekend() =>
        Assert.Equal(["Weekday: 780", "Weekend: 370"], DateSamples.WeekdayVsWeekend());

    [Fact]
    public void Business_days_in_march() =>
        Assert.Equal(22, DateSamples.BusinessDays());

    [Fact]
    public void Last_friday() =>
        Assert.Equal(["2026-01-30", "2026-02-27", "2026-03-27"], DateSamples.LastFriday());

    [Fact]
    public void Missing_dates() =>
        Assert.Equal(["2026-03-05", "2026-03-08", "2026-03-11", "2026-03-13"], DateSamples.MissingDates());

    [Fact]
    public void Streaks() =>
        Assert.Equal(
            ["2026-03-02..2026-03-04 (3 days)", "2026-03-06..2026-03-07 (2 days)", "2026-03-09..2026-03-10 (2 days)", "2026-03-12 (1 day)", "2026-03-14 (1 day)"],
            DateSamples.Streaks());

    [Fact]
    public void Running_total_ends_at_the_sum() =>
        Assert.Equal("2026-03-14: 1150", DateSamples.RunningTotal().Last());

    [Fact]
    public void Best_day_per_week() =>
        Assert.Equal(["W10: 2026-03-07 (200)", "W11: 2026-03-14 (170)"], DateSamples.BestDayPerWeek());

    [Fact]
    public void Week_over_week() =>
        Assert.Equal(["W11 vs W10: -150 (-23.1%)"], DateSamples.WeekOverWeek());

    [Fact]
    public void Hour_buckets() =>
        Assert.Equal(["08:00: 2", "09:00: 1", "11:00: 2"], DateSamples.HourBuckets());

    [Fact]
    public void Overlapping_ranges() =>
        Assert.Equal(["09:00-10:00 overlaps 09:30-11:00"], DateSamples.OverlappingRanges());

    [Fact]
    public void Merge_ranges() =>
        Assert.Equal(["09:00-12:00", "13:00-14:00"], DateSamples.MergeRanges());

    // Statistics

    [Fact]
    public void Summary() =>
        Assert.Equal(["Mean:  80.4", "Min:   60", "Max:   95", "Range: 35"], StatisticsSamples.Summary());

    [Fact]
    public void Median() =>
        Assert.Equal(85.0, StatisticsSamples.Median());

    [Fact]
    public void Mode() =>
        Assert.Equal([85], StatisticsSamples.Mode());

    [Fact]
    public void Standard_deviation() =>
        Assert.Equal(["Population SD: 10.61", "Sample SD:     11.19"], StatisticsSamples.StandardDeviation());

    [Fact]
    public void Percentiles() =>
        Assert.Equal(["P25: 72", "P50: 85", "P90: 90"], StatisticsSamples.Percentiles());

    [Fact]
    public void Outliers() =>
        Assert.Equal(["Q1: 122, Q3: 135, IQR: 13", "Outliers: 900"], StatisticsSamples.Outliers());

    [Fact]
    public void Z_scores() =>
        Assert.Equal(["60: -1.92", "95: 1.38"], StatisticsSamples.ZScores());

    [Fact]
    public void Normalize() =>
        Assert.Equal([0.0, 0.5, 0.25, 1.0], StatisticsSamples.Normalize());

    [Fact]
    public void Correlation() =>
        Assert.Equal("r = 0.994", StatisticsSamples.Correlation());

    [Fact]
    public void Linear_regression() =>
        Assert.Equal(["y = 8.2x + 43.4", "Prediction for 6 hours: 92.6"], StatisticsSamples.LinearRegression());

    [Fact]
    public void Weighted_average() =>
        Assert.Equal("81.11", StatisticsSamples.WeightedAverage());

    [Fact]
    public void Histogram() =>
        Assert.Equal(["60-69 | ##", "70-79 | ##", "80-89 | ####", "90-99 | ##"], StatisticsSamples.Histogram());

    [Fact]
    public void Cumulative_distribution() =>
        Assert.Equal(
            ["60: 10%", "66: 20%", "72: 30%", "78: 40%", "85: 70%", "88: 80%", "90: 90%", "95: 100%"],
            StatisticsSamples.CumulativeDistribution());

    [Fact]
    public void Geometric_mean() =>
        Assert.Equal("Average growth: 7.84%", StatisticsSamples.GeometricMean());

    [Fact]
    public void Root_mean_square_error() =>
        Assert.Equal("RMSE: 3.67", StatisticsSamples.RootMeanSquareError());
}
