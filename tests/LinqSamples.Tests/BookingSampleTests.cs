namespace LinqSamples.Tests;

public class BookingSampleTests
{
    [Fact] public void Conflicts() => Assert.Equal(["R1:9-11/10-12"], BookingSamples.Conflicts());
    [Fact] public void Hours_per_room() => Assert.Equal(["R1:5", "R2:4"], BookingSamples.HoursPerRoom());
    [Fact] public void Merged_intervals() => Assert.Equal(["R1:9-12,13-14", "R2:9-13"], BookingSamples.MergedIntervals());
    [Fact] public void Free_hours() => Assert.Equal(["R1:12", "R2:13"], BookingSamples.FreeHours());
    [Fact] public void Peak_hour() => Assert.Equal("10:3", BookingSamples.PeakHour());
    [Fact] public void Back_to_back() => Assert.Equal(["R2:9-10>10-11", "R2:10-11>11-13"], BookingSamples.BackToBack());
    [Fact] public void Free_at_noon() => Assert.Equal(["R1"], BookingSamples.FreeAtNoon());
    [Fact] public void Busy_hours() => Assert.Equal(5, BookingSamples.BusyHours());
    [Fact] public void Start_hours() => Assert.Equal(["9:2", "10:2", "11:1", "13:1"], BookingSamples.StartHours());
    [Fact] public void Longest_per_room() => Assert.Equal(["R1:9-11", "R2:11-13"], BookingSamples.LongestPerRoom());
}
