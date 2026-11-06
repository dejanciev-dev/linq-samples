namespace LinqSamples.Samples;

/// <summary>A week of temperatures and rainfall.</summary>
[SampleCategory("Weather week", 49)]
public static class WeatherSamples
{
    private sealed record Day(string Name, int High, int Low, int Rain);

    private static readonly Day[] Week =
    [
        new("Mon", 20, 10, 0), new("Tue", 22, 12, 5), new("Wed", 18, 9, 12), new("Thu", 25, 15, 0),
        new("Fri", 19, 11, 3), new("Sat", 28, 17, 0), new("Sun", 21, 13, 8),
    ];

    [Sample("Hottest day", Description = "MaxBy the daily high.")]
    public static string Hottest() => Week.MaxBy(d => d.High)!.Name;

    [Sample("Coldest night", Description = "MinBy the daily low.")]
    public static string Coldest() => Week.MinBy(d => d.Low)!.Name;

    [Sample("Average rain", Description = "Average of the rainfall values.")]
    public static double AverageRain() => Week.Average(d => d.Rain);

    [Sample("Dry days", Description = "Days without rain.")]
    public static IEnumerable<string> DryDays() => Week.Where(d => d.Rain == 0).Select(d => d.Name);

    [Sample("Rainy day count", Description = "Count with a predicate.")]
    public static int RainyDays() => Week.Count(d => d.Rain > 0);

    [Sample("Narrowest range", Description = "MinBy high minus low.")]
    public static string NarrowestRange() => Week.MinBy(d => d.High - d.Low)!.Name;

    [Sample("Warmer than yesterday", Description = "Zip each day with the previous one.")]
    public static IEnumerable<string> WarmerThanYesterday() =>
        Week.Skip(1).Zip(Week, (today, yesterday) => (today, yesterday)).Where(p => p.today.High > p.yesterday.High).Select(p => p.today.Name);

    [Sample("Total rain", Description = "Sum of the rainfall.")]
    public static int TotalRain() => Week.Sum(d => d.Rain);

    [Sample("Above average highs", Description = "Compare each high with the average high.")]
    public static IEnumerable<string> AboveAverage()
    {
        var avg = Week.Average(d => d.High);
        return Week.Where(d => d.High > avg).Select(d => d.Name);
    }

    [Sample("Weekend average high", Description = "Skip five days and average the rest.")]
    public static double WeekendHigh() => Week.Skip(5).Average(d => d.High);
}
