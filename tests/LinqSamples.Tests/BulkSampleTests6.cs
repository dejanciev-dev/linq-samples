namespace LinqSamples.Tests;

public class BikeSampleTests
{
    [Fact] public void Total_minutes() => Assert.Equal(180, BikeSamples.TotalMinutes());
    [Fact] public void Minutes_per_station() => Assert.Equal(["East:10", "North:110", "South:60"], BikeSamples.MinutesPerStation());
    [Fact] public void Electric_rides() => Assert.Equal(3, BikeSamples.ElectricRides());
    [Fact] public void Longest_ride() => Assert.Equal("Bob", BikeSamples.LongestRide());
    [Fact] public void Riders() => Assert.Equal(["Ann", "Bob", "Cid", "Dee"], BikeSamples.Riders());
    [Fact] public void Average_minutes() => Assert.Equal(30.0, BikeSamples.AverageMinutes());
    [Fact] public void Rides_per_rider() => Assert.Equal(["Ann:2", "Bob:2", "Cid:1", "Dee:1"], BikeSamples.RidesPerRider());
    [Fact] public void Short_rides() => Assert.Equal(["Bob", "Dee"], BikeSamples.ShortRides());
    [Fact] public void Electric_vs_classic() => Assert.Equal(["electric:110", "classic:70"], BikeSamples.ElectricVsClassic());
    [Fact] public void Busiest_station() => Assert.Equal("North", BikeSamples.BusiestStation());
}
