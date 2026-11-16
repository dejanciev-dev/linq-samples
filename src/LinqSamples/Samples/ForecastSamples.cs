namespace LinqSamples.Samples;

/// <summary>Weather readings: temperature and rainfall per city and day.</summary>
[SampleCategory("City forecasts", 75)]
public static class ForecastSamples
{
    private sealed record Reading(string City, int Day, int Temp, int Rain);

    private static readonly Reading[] Readings =
    [
        new("Oslo", 1, 5, 0), new("Oslo", 2, 7, 3), new("Rome", 1, 18, 0),
        new("Rome", 2, 20, 0), new("Oslo", 3, 2, 8), new("Rome", 3, 17, 5),
    ];

    [Sample("Max temperature", Description = "Highest reading overall.")]
    public static int MaxTemp() => Readings.Max(r => r.Temp);

    [Sample("Rain per city", Description = "Sum rainfall grouped by city.")]
    public static IEnumerable<string> RainPerCity() => Readings.GroupBy(r => r.City).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(r => r.Rain)}");

    [Sample("Temperature range", Description = "Min and max per city.")]
    public static IEnumerable<string> TempRange() => Readings.GroupBy(r => r.City).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Min(r => r.Temp)}-{g.Max(r => r.Temp)}");

    [Sample("Dry days", Description = "Count readings with no rain.")]
    public static int DryDays() => Readings.Count(r => r.Rain == 0);

    [Sample("Warmest city", Description = "MaxBy temperature.")]
    public static string WarmestCity() => Readings.MaxBy(r => r.Temp)!.City;

    [Sample("Rainy cities", Description = "Distinct cities with heavy rain.")]
    public static IEnumerable<string> RainyCities() => Readings.Where(r => r.Rain > 5).Select(r => r.City).Distinct();

    [Sample("Day-over-day change", Description = "Pair neighbouring Oslo readings with Zip and Skip.")]
    public static IEnumerable<int> OsloChange()
    {
        var temps = Readings.Where(r => r.City == "Oslo").OrderBy(r => r.Day).Select(r => r.Temp).ToArray();
        return temps.Zip(temps.Skip(1), (a, b) => b - a);
    }

    [Sample("Coldest day", Description = "MinBy temperature, formatted.")]
    public static string ColdestDay() { var r = Readings.MinBy(r => r.Temp)!; return $"{r.City}:{r.Day}"; }

    [Sample("Total rain", Description = "Sum of all rainfall.")]
    public static int TotalRain() => Readings.Sum(r => r.Rain);

    [Sample("Hottest day per city", Description = "MaxBy inside each group.")]
    public static IEnumerable<string> HottestDayPerCity() =>
        Readings.GroupBy(r => r.City).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MaxBy(r => r.Temp)!.Day}");
}
