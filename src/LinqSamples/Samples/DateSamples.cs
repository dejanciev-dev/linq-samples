using System.Globalization;

namespace LinqSamples.Samples;

/// <summary>Calendars and time series: grouping by period, filling gaps, rolling windows and time ranges.</summary>
[SampleCategory("Dates & time series", 31)]
public static class DateSamples
{
    /// <summary>Daily sales for two weeks of March 2026, with some days missing.</summary>
    private static readonly (DateOnly Date, int Amount)[] Sales =
    [
        (new(2026, 3, 2), 120), (new(2026, 3, 3), 80), (new(2026, 3, 4), 100), (new(2026, 3, 6), 150),
        (new(2026, 3, 7), 200), (new(2026, 3, 9), 90), (new(2026, 3, 10), 110), (new(2026, 3, 12), 130),
        (new(2026, 3, 14), 170),
    ];

    [Sample("Group by ISO week", Description = "ISOWeek gives the week number used in business calendars.")]
    public static IEnumerable<string> GroupByWeek() =>
        Sales.GroupBy(s => WeekOf(s.Date)).Select(g => $"W{g.Key}: {g.Sum(s => s.Amount)}");

    [Sample("Fill missing days", Description = "Generate every date, then LeftJoin the data - missing days become 0.")]
    public static IEnumerable<string> FillMissingDays() =>
        FilledSeries().Select(d => $"{d.Date:yyyy-MM-dd}: {d.Amount}");

    [Sample("Rolling 3-day total", Description = "Window over the gap-free series - each value covers the day and the two before it.")]
    public static IEnumerable<string> RollingTotal() =>
        FilledSeries().Window(3).Select(w => $"{w[^1].Date:yyyy-MM-dd}: {w.Sum(d => d.Amount)}");

    [Sample("Day-over-day change", Description = "Pairwise gives each day with the previous recorded day.")]
    public static IEnumerable<string> DayOverDay() =>
        Sales.Pairwise().Select(p => $"{p.Current.Date:yyyy-MM-dd}: {p.Current.Amount - p.Previous.Amount:+0;-0;0}");

    [Sample("Weekday vs weekend", Description = "Group by a computed boolean.")]
    public static IEnumerable<string> WeekdayVsWeekend() =>
        Sales.GroupBy(s => s.Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
             .Select(g => $"{(g.Key ? "Weekend" : "Weekday")}: {g.Sum(s => s.Amount)}");

    [Sample("Business days in a month", Description = "Count the days of March 2026 that are not on a weekend.")]
    public static int BusinessDays() =>
        Enumerable.Range(0, DateTime.DaysInMonth(2026, 3))
                  .Select(new DateOnly(2026, 3, 1).AddDays)
                  .Count(d => d.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday));

    [Sample("Last Friday of each month", Description = "Walk back from the month's last day to the first Friday.")]
    public static IEnumerable<string> LastFriday() =>
        Enumerable.Range(1, 3).Select(month =>
        {
            var lastDay = new DateOnly(2026, month, DateTime.DaysInMonth(2026, month));
            var friday = Enumerable.Range(0, 7).Select(i => lastDay.AddDays(-i)).First(d => d.DayOfWeek == DayOfWeek.Friday);
            return $"{friday:yyyy-MM-dd}";
        });

    [Sample("Missing dates", Description = "The full range Except the dates that have data.")]
    public static IEnumerable<string> MissingDates() =>
        AllDays().Except(Sales.Select(s => s.Date)).Select(d => $"{d:yyyy-MM-dd}");

    [Sample("Consecutive-day streaks", Description = "SplitWhen starts a new streak when a day is skipped.")]
    public static IEnumerable<string> Streaks() =>
        Sales.Select(s => s.Date)
             .SplitWhen((previous, current) => current.DayNumber - previous.DayNumber > 1)
             .Select(streak => streak.Count == 1
                 ? $"{streak[0]:yyyy-MM-dd} (1 day)"
                 : $"{streak[0]:yyyy-MM-dd}..{streak[^1]:yyyy-MM-dd} ({streak.Count} days)");

    [Sample("Running total", Description = "Scan accumulates; Zip puts the dates back next to the totals.")]
    public static IEnumerable<string> RunningTotal() =>
        Sales.Select(s => s.Amount)
             .Scan(0, (total, amount) => total + amount)
             .Zip(Sales, (total, s) => $"{s.Date:yyyy-MM-dd}: {total}");

    [Sample("Best day per week", Description = "Group by week, then MaxBy inside each group.")]
    public static IEnumerable<string> BestDayPerWeek() =>
        Sales.GroupBy(s => WeekOf(s.Date))
             .Select(g => g.MaxBy(s => s.Amount))
             .Select(best => $"W{WeekOf(best.Date)}: {best.Date:yyyy-MM-dd} ({best.Amount})");

    [Sample("Week over week", Description = "Totals per week, then Pairwise for the change and its percentage.")]
    public static IEnumerable<string> WeekOverWeek() =>
        Sales.GroupBy(s => WeekOf(s.Date))
             .Select(g => (Week: g.Key, Total: g.Sum(s => s.Amount)))
             .Pairwise()
             .Select(p => FormattableString.Invariant(
                 $"W{p.Current.Week} vs W{p.Previous.Week}: {p.Current.Total - p.Previous.Total:+0;-0;0} ({100.0 * (p.Current.Total - p.Previous.Total) / p.Previous.Total:+0.0;-0.0}%)"));

    [Sample("Bucket times by hour", Description = "Group TimeOnly values by their hour.")]
    public static IEnumerable<string> HourBuckets()
    {
        TimeOnly[] events = [new(8, 15), new(8, 47), new(9, 5), new(11, 30), new(11, 59)];
        return events.GroupBy(t => t.Hour).Select(g => $"{g.Key:00}:00: {g.Count()}");
    }

    [Sample("Overlapping ranges", Description = "Two ranges overlap when each starts before the other ends.")]
    public static IEnumerable<string> OverlappingRanges()
    {
        var meetings = Meetings();
        return from a in meetings.Index()
               from b in meetings.Index()
               where a.Index < b.Index && a.Item.Start < b.Item.End && b.Item.Start < a.Item.End
               select $"{Format(a.Item)} overlaps {Format(b.Item)}";
    }

    [Sample("Merge ranges", Description = "Sort by start, then Aggregate: extend the last range or start a new one.")]
    public static IEnumerable<string> MergeRanges() =>
        Meetings().OrderBy(m => m.Start)
                  .Aggregate(new List<(TimeOnly Start, TimeOnly End)>(), (merged, m) =>
                  {
                      if (merged.Count > 0 && m.Start <= merged[^1].End)
                      {
                          merged[^1] = (merged[^1].Start, m.End > merged[^1].End ? m.End : merged[^1].End);
                      }
                      else
                      {
                          merged.Add(m);
                      }

                      return merged;
                  })
                  .Select(Format);

    private static int WeekOf(DateOnly date) => ISOWeek.GetWeekOfYear(date.ToDateTime(TimeOnly.MinValue));

    private static IEnumerable<DateOnly> AllDays()
    {
        var first = Sales[0].Date;
        return Enumerable.Range(0, Sales[^1].Date.DayNumber - first.DayNumber + 1).Select(first.AddDays);
    }

    private static IEnumerable<(DateOnly Date, int Amount)> FilledSeries() =>
        AllDays().LeftJoin(Sales, day => day, s => s.Date, (day, s) => (day, s.Amount));

    private static (TimeOnly Start, TimeOnly End)[] Meetings() =>
        [(new(9, 0), new(10, 0)), (new(9, 30), new(11, 0)), (new(11, 0), new(12, 0)), (new(13, 0), new(14, 0))];

    private static string Format((TimeOnly Start, TimeOnly End) range) =>
        $"{range.Start.ToString("HH:mm", CultureInfo.InvariantCulture)}-{range.End.ToString("HH:mm", CultureInfo.InvariantCulture)}";
}
