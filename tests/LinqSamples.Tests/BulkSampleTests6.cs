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

public class BakerySampleTests
{
    [Fact] public void Total_revenue() => Assert.Equal(134, BakerySamples.TotalRevenue());
    [Fact] public void Revenue_per_day() => Assert.Equal(["Mon:42", "Tue:54", "Wed:38"], BakerySamples.RevenuePerDay());
    [Fact] public void Units_per_item() => Assert.Equal(["Bread:18", "Cake:2", "Croissant:20", "Muffin:5"], BakerySamples.UnitsPerItem());
    [Fact] public void Best_seller() => Assert.Equal("Croissant", BakerySamples.BestSeller());
    [Fact] public void Top_revenue_item() => Assert.Equal("Bread", BakerySamples.TopRevenueItem());
    [Fact] public void Multi_day_items() => Assert.Equal(["Bread", "Croissant"], BakerySamples.MultiDayItems());
    [Fact] public void Average_price() => Assert.Equal(4.5, BakerySamples.AveragePrice());
    [Fact] public void Premium_items() => Assert.Equal(["Cake"], BakerySamples.PremiumItems());
    [Fact] public void Big_orders() => Assert.Equal(2, BakerySamples.BigOrders());
    [Fact] public void Running_revenue() => Assert.Equal([42, 96, 134], BakerySamples.RunningRevenue());
}
