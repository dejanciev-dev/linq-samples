namespace LinqSamples.Tests;

public class MoreFlightSampleTests
{
    [Fact] public void Average_price() => Assert.Equal(298.0, FlightSamples.AveragePrice());
    [Fact] public void Short_haul() => Assert.Equal(["A1", "B2", "E5"], FlightSamples.ShortHaul());
    [Fact] public void Arrivals() => Assert.Equal(["CDG:1", "JFK:2", "LHR:1", "OSL:1"], FlightSamples.Arrivals());
    [Fact] public void Fare_by_origin() => Assert.Equal(["CDG:520", "LHR:640", "OSL:330"], FlightSamples.FareByOrigin());
    [Fact] public void Priciest_per_minute() => Assert.Equal("B2", FlightSamples.PriciestPerMinute());
    [Fact] public void Dead_ends() => Assert.Equal(["JFK"], FlightSamples.DeadEnds());
    [Fact] public void Two_hops() => Assert.Equal(["CDG", "JFK", "LHR", "OSL"], FlightSamples.TwoHops());
    [Fact] public void Price_bands() => Assert.Equal(["0:3", "400:2"], FlightSamples.PriceBands());
    [Fact] public void Two_longest() => Assert.Equal(["D4", "C3"], FlightSamples.TwoLongest());
    [Fact] public void Round_trip() => Assert.Equal(290, FlightSamples.RoundTrip());
}

public class MoreContactSampleTests
{
    [Fact] public void Youngest() => Assert.Equal("Bob", ContactSamples.Youngest());
    [Fact] public void Average_age() => Assert.Equal(31.2, ContactSamples.AverageAge());
    [Fact] public void Names_per_city() => Assert.Equal(["Oslo:Ann,Cy", "Paris:Di", "Rome:Bob,Ed"], ContactSamples.NamesPerCity());
    [Fact] public void Over_thirty() => Assert.Equal(["Cy", "Ed"], ContactSamples.OverThirty());
    [Fact] public void City_of_oldest() => Assert.Equal("Oslo", ContactSamples.CityOfOldest());
    [Fact] public void Phone_prefixes() => Assert.Equal(1, ContactSamples.PhonePrefixes());
    [Fact] public void Unique_phone_owners() => Assert.Equal(["Bob", "Di", "Ed"], ContactSamples.UniquePhoneOwners());
    [Fact] public void Second_page() => Assert.Equal(["Cy", "Di"], ContactSamples.SecondPage());
    [Fact] public void All_thirty_plus() => Assert.Equal(["Oslo"], ContactSamples.AllThirtyPlus());
    [Fact] public void Age_spread() => Assert.Equal(["Oslo:11", "Paris:0", "Rome:10"], ContactSamples.AgeSpread());
}

public class HotelSampleTests
{
    [Fact] public void Total_nights() => Assert.Equal(17, HotelSamples.TotalNights());
    [Fact] public void Revenue() => Assert.Equal(2120, HotelSamples.Revenue());
    [Fact] public void Longest_stay() => Assert.Equal("Cy", HotelSamples.LongestStay());
    [Fact] public void Repeat_guests() => Assert.Equal(["Ann"], HotelSamples.RepeatGuests());
    [Fact] public void Nights_per_room() => Assert.Equal(["101:8", "102:6", "201:3"], HotelSamples.NightsPerRoom());
    [Fact] public void Revenue_per_month() => Assert.Equal(["Jan:540", "Feb:700", "Mar:880"], HotelSamples.RevenuePerMonth());
    [Fact] public void Best_room() => Assert.Equal("101", HotelSamples.BestRoom());
    [Fact] public void Average_rate() => Assert.Equal(140.0, HotelSamples.AverageRate());
    [Fact] public void Long_stays() => Assert.Equal(3, HotelSamples.LongStays());
    [Fact] public void Guest_spend() => Assert.Equal(["Ann:700", "Cy:500", "Ed:480", "Bob:240", "Di:200"], HotelSamples.GuestSpend());
}

public class KitchenSampleTests
{
    [Fact] public void Dishes() => Assert.Equal(["Pasta", "Pizza", "Salad", "Soup"], KitchenSamples.Dishes());
    [Fact] public void Cost_per_dish() => Assert.Equal(["Pasta:3", "Pizza:6", "Salad:5", "Soup:4"], KitchenSamples.CostPerDish());
    [Fact] public void Most_expensive() => Assert.Equal("Pizza", KitchenSamples.MostExpensive());
    [Fact] public void Shared() => Assert.Equal(["cheese", "flour", "tomato"], KitchenSamples.Shared());
    [Fact] public void With_cheese() => Assert.Equal(["Pizza", "Salad"], KitchenSamples.WithCheese());
    [Fact] public void Grams_per_item() => Assert.Equal(["cheese:200", "egg:100", "flour:500", "tomato:320"], KitchenSamples.GramsPerItem());
    [Fact] public void Count_per_dish() => Assert.Equal(["Pasta:2", "Pizza:2", "Salad:2", "Soup:1"], KitchenSamples.CountPerDish());
    [Fact] public void Single_ingredient() => Assert.Equal(["Soup"], KitchenSamples.SingleIngredient());
    [Fact] public void Heaviest() => Assert.Equal("Pizza flour", KitchenSamples.Heaviest());
    [Fact] public void Total_cost() => Assert.Equal(18, KitchenSamples.TotalCost());
}
