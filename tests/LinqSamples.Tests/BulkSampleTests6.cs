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

public class PizzaSampleTests
{
    [Fact] public void Total_revenue() => Assert.Equal(70, PizzaSamples.TotalRevenue());
    [Fact] public void Orders_per_size() => Assert.Equal(["L:3", "M:2", "S:1"], PizzaSamples.OrdersPerSize());
    [Fact] public void Delivered() => Assert.Equal(4, PizzaSamples.Delivered());
    [Fact] public void Average_toppings() => Assert.Equal(2.0, PizzaSamples.AverageToppings());
    [Fact] public void Most_toppings() => Assert.Equal("Bob", PizzaSamples.MostToppings());
    [Fact] public void Plain_pizza_customers() => Assert.Equal(["Ann"], PizzaSamples.PlainPizzaCustomers());
    [Fact] public void Spend_per_customer() => Assert.Equal(["Bob:25", "Ann:21", "Cid:13", "Dee:11"], PizzaSamples.SpendPerCustomer());
    [Fact] public void Pickup_customers() => Assert.Equal(["Ann", "Bob"], PizzaSamples.PickupCustomers());
    [Fact] public void Any_small_delivered() => Assert.False(PizzaSamples.AnySmallDelivered());
    [Fact] public void Cheapest_large() => Assert.Equal(13, PizzaSamples.CheapestLarge());
}

public class HikingSampleTests
{
    [Fact] public void Total_km() => Assert.Equal(60, HikingSamples.TotalKm());
    [Fact] public void Trails() => Assert.Equal(["Forest", "Lake", "Peak", "Ridge"], HikingSamples.Trails());
    [Fact] public void Km_per_hiker() => Assert.Equal(["Ann:18", "Bob:18", "Cid:15", "Dee:9"], HikingSamples.KmPerHiker());
    [Fact] public void Toughest_trail() => Assert.Equal("Peak", HikingSamples.ToughestTrail());
    [Fact] public void Hikes_per_trail() => Assert.Equal(["Forest:1", "Lake:2", "Peak:1", "Ridge:2"], HikingSamples.HikesPerTrail());
    [Fact] public void Popular_trails() => Assert.Equal(["Lake", "Ridge"], HikingSamples.PopularTrails());
    [Fact] public void Average_km() => Assert.Equal(10.0, HikingSamples.AverageKm());
    [Fact] public void Long_hikes() => Assert.Equal(3, HikingSamples.LongHikes());
    [Fact] public void Trail_buddies_of_Ann() => Assert.Equal(["Bob"], HikingSamples.TrailBuddiesOfAnn());
    [Fact] public void Easy_trails() => Assert.Equal(["Forest", "Lake"], HikingSamples.EasyTrails());
}

public class CoffeeSampleTests
{
    [Fact] public void Total_revenue() => Assert.Equal(24, CoffeeSamples.TotalRevenue());
    [Fact] public void Orders_per_drink() => Assert.Equal(["Espresso:2", "Latte:2", "Mocha:1", "Tea:1"], CoffeeSamples.OrdersPerDrink());
    [Fact] public void Revenue_per_hour() => Assert.Equal(["8:7", "9:4", "12:6", "15:7"], CoffeeSamples.RevenuePerHour());
    [Fact] public void Morning_orders() => Assert.Equal(3, CoffeeSamples.MorningOrders());
    [Fact] public void Priciest_drink() => Assert.Equal("Mocha", CoffeeSamples.PriciestDrink());
    [Fact] public void Sizes() => Assert.Equal(["L", "M", "S"], CoffeeSamples.Sizes());
    [Fact] public void Average_price() => Assert.Equal(4.0, CoffeeSamples.AveragePrice());
    [Fact] public void Busiest_hour() => Assert.Equal(8, CoffeeSamples.BusiestHour());
    [Fact] public void Drinks_over_4() => Assert.Equal(["Latte", "Mocha", "Tea"], CoffeeSamples.DrinksOver4());
    [Fact] public void Revenue_per_drink() => Assert.Equal(["Latte:9", "Mocha:6", "Tea:5", "Espresso:4"], CoffeeSamples.RevenuePerDrink());
}

public class TrainSampleTests
{
    [Fact] public void Total_fare() => Assert.Equal(490, TrainSamples.TotalFare());
    [Fact] public void Departures_per_city() => Assert.Equal(["Bergen:1", "Oslo:4", "Trondheim:1"], TrainSamples.DeparturesPerCity());
    [Fact] public void Popular_route() => Assert.Equal("Oslo-Bergen", TrainSamples.PopularRoute());
    [Fact] public void Cities() => Assert.Equal(["Bergen", "Oslo", "Trondheim"], TrainSamples.Cities());
    [Fact] public void Round_trippers() => Assert.Equal(["Ann"], TrainSamples.RoundTrippers());
    [Fact] public void Cheapest_Oslo_Bergen() => Assert.Equal(70, TrainSamples.CheapestOsloBergen());
    [Fact] public void Fare_per_passenger() => Assert.Equal(["Ann:180", "Bob:155", "Cid:70", "Dee:85"], TrainSamples.FarePerPassenger());
    [Fact] public void Total_minutes() => Assert.Equal(2480, TrainSamples.TotalMinutes());
    [Fact] public void Arrivals_in_Oslo() => Assert.Equal(["Ann", "Dee"], TrainSamples.ArrivalsInOslo());
    [Fact] public void Fare_bands() => Assert.Equal(["high:4", "low:2"], TrainSamples.FareBands());
}
