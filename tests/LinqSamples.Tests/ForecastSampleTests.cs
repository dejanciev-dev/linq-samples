namespace LinqSamples.Tests;

public class ForecastSampleTests
{
    [Fact] public void Max_temp() => Assert.Equal(20, ForecastSamples.MaxTemp());
    [Fact] public void Rain_per_city() => Assert.Equal(["Oslo:11", "Rome:5"], ForecastSamples.RainPerCity());
    [Fact] public void Temp_range() => Assert.Equal(["Oslo:2-7", "Rome:17-20"], ForecastSamples.TempRange());
    [Fact] public void Dry_days() => Assert.Equal(3, ForecastSamples.DryDays());
    [Fact] public void Warmest_city() => Assert.Equal("Rome", ForecastSamples.WarmestCity());
    [Fact] public void Rainy_cities() => Assert.Equal(["Oslo"], ForecastSamples.RainyCities());
    [Fact] public void Oslo_change() => Assert.Equal([2, -5], ForecastSamples.OsloChange());
    [Fact] public void Coldest_day() => Assert.Equal("Oslo:3", ForecastSamples.ColdestDay());
    [Fact] public void Total_rain() => Assert.Equal(16, ForecastSamples.TotalRain());
    [Fact] public void Hottest_day_per_city() => Assert.Equal(["Oslo:2", "Rome:2"], ForecastSamples.HottestDayPerCity());
}
