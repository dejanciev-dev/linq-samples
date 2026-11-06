namespace LinqSamples.Samples;

/// <summary>Used-car listings: brands, mileage and colours.</summary>
[SampleCategory("Used cars", 58)]
public static class CarSamples
{
    private sealed record Car(string Plate, string Brand, int Year, int Km, string Color);

    private static readonly Car[] Cars =
    [
        new("A1", "vw", 2015, 120000, "red"), new("B2", "bmw", 2020, 40000, "blue"), new("C3", "vw", 2018, 80000, "blue"),
        new("D4", "audi", 2012, 200000, "black"), new("E5", "bmw", 2019, 60000, "red"),
    ];

    [Sample("Cars per brand", Description = "CountBy brand.")]
    public static IEnumerable<string> PerBrand() => Cars.CountBy(c => c.Brand).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Lowest mileage", Description = "MinBy km.")]
    public static string LowestMileage() => Cars.MinBy(c => c.Km)!.Plate;

    [Sample("Average mileage", Description = "Average of km.")]
    public static double AverageKm() => Cars.Average(c => c.Km);

    [Sample("Oldest car", Description = "MinBy year.")]
    public static string Oldest() => Cars.MinBy(c => c.Year)!.Plate;

    [Sample("Red cars", Description = "Filter by colour.")]
    public static IEnumerable<string> Red() => Cars.Where(c => c.Color == "red").Select(c => c.Plate);

    [Sample("Colour count", Description = "Distinct then Count.")]
    public static int ColorCount() => Cars.Select(c => c.Color).Distinct().Count();

    [Sample("Average mileage per brand", Description = "GroupBy brand and average km.")]
    public static IEnumerable<string> KmPerBrand() => Cars.GroupBy(c => c.Brand).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Average(c => c.Km)}");

    [Sample("Total age in 2026", Description = "Sum of 2026 minus the year.")]
    public static int TotalAge() => Cars.Sum(c => 2026 - c.Year);

    [Sample("Over 100000 km", Description = "High mileage plates, sorted.")]
    public static IEnumerable<string> HighMileage() => Cars.Where(c => c.Km > 100000).Select(c => c.Plate).Order();

    [Sample("Newest per colour", Description = "GroupBy colour then MaxBy year.")]
    public static IEnumerable<string> NewestPerColor() => Cars.GroupBy(c => c.Color).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MaxBy(c => c.Year)!.Plate}");
}
