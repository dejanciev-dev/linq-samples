namespace LinqSamples.Tests;

public class LogSampleTests
{
    [Fact] public void By_level() => Assert.Equal(["ERROR:3", "INFO:2", "WARN:1"], LogSamples.ByLevel());
    [Fact] public void Errors() => Assert.Equal(["Timeout", "Connection lost", "Timeout"], LogSamples.Errors());
    [Fact] public void Noisiest() => Assert.Equal("api", LogSamples.NoisiestSource());
    [Fact] public void Frequent() => Assert.Equal("Timeout", LogSamples.FrequentError());
    [Fact] public void Per_minute() => Assert.Equal(["10:01=2", "10:02=1"], LogSamples.PerMinute());
    [Fact] public void Gaps() => Assert.Equal([4d, 65, 2, 48, 30], LogSamples.Gaps());
    [Fact] public void Longest_quiet() => Assert.Equal(65d, LogSamples.LongestQuiet());
    [Fact] public void Recovery() => Assert.Equal("Recovered", LogSamples.Recovery());
    [Fact] public void Error_sources() => Assert.Equal(["api", "db"], LogSamples.ErrorSources());
}

public class InventoryMoreTests
{
    [Fact] public void Average_price() => Assert.Equal(3.6625m, InventorySamples.AveragePrice());
    [Fact] public void Total_units() => Assert.Equal(219, InventorySamples.TotalUnits());
    [Fact] public void Value_share() => Assert.Equal(["bolt:12%", "nut:4%", "gear:37%", "axle:47%"], InventorySamples.ValueShare());
}
