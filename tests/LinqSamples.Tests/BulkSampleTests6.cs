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

public class ConcertSampleTests
{
    [Fact] public void Total_revenue() => Assert.Equal(420, ConcertSamples.TotalRevenue());
    [Fact] public void Tickets_per_band() => Assert.Equal(["Blur:2", "Muse:3", "Oasis:1"], ConcertSamples.TicketsPerBand());
    [Fact] public void Revenue_per_section() => Assert.Equal(["Balcony:90", "Floor:330"], ConcertSamples.RevenuePerSection());
    [Fact] public void Multi_band_fans() => Assert.Equal(["Ann", "Bob"], ConcertSamples.MultiBandFans());
    [Fact] public void Priciest_ticket() => Assert.Equal("Ann:Muse", ConcertSamples.PriciestTicket());
    [Fact] public void Bands() => Assert.Equal(["Blur", "Muse", "Oasis"], ConcertSamples.Bands());
    [Fact] public void Top_price_per_band() => Assert.Equal(["Blur:70", "Muse:90", "Oasis:80"], ConcertSamples.TopPricePerBand());
    [Fact] public void Cheapest_band() => Assert.Equal("Blur", ConcertSamples.CheapestBand());
    [Fact] public void Floor_all_over_60() => Assert.True(ConcertSamples.FloorAllOver60());
    [Fact] public void Spend_per_buyer() => Assert.Equal(["Ann:130", "Bob:130", "Dee:90", "Cid:70"], ConcertSamples.SpendPerBuyer());
}

public class TaxiSampleTests
{
    [Fact] public void Total_km() => Assert.Equal(49, TaxiSamples.TotalKm());
    [Fact] public void Earnings_per_driver() => Assert.Equal(["Joe:46", "Max:20", "Sue:40"], TaxiSamples.EarningsPerDriver());
    [Fact] public void Top_earner() => Assert.Equal("Joe", TaxiSamples.TopEarner());
    [Fact] public void Longest_ride() => Assert.Equal("Joe", TaxiSamples.LongestRide());
    [Fact] public void Five_star_rides() => Assert.Equal(3, TaxiSamples.FiveStarRides());
    [Fact] public void Low_rated_drivers() => Assert.Equal(["Joe", "Max"], TaxiSamples.LowRatedDrivers());
    [Fact] public void Average_rating() => Assert.Equal(4.0, TaxiSamples.AverageRating());
    [Fact] public void Short_trips() => Assert.Equal(2, TaxiSamples.ShortTrips());
    [Fact] public void Consistent_drivers() => Assert.Equal(["Sue"], TaxiSamples.ConsistentDrivers());
    [Fact] public void Fare_range() => Assert.Equal(34, TaxiSamples.FareRange());
}
