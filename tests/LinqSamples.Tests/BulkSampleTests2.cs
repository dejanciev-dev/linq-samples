namespace LinqSamples.Tests;

public class LeagueSampleTests
{
    [Fact] public void Points_table() => Assert.Equal(["Reds:8", "Blues:4", "Greens:1"], LeagueSamples.PointsTable());
    [Fact] public void Leader() => Assert.Equal("Reds", LeagueSamples.Leader());
    [Fact] public void Total_goals() => Assert.Equal(10, LeagueSamples.TotalGoals());
    [Fact] public void Draws() => Assert.Equal(2, LeagueSamples.Draws());
    [Fact] public void Biggest_win() => Assert.Equal("Blues 3-1 Greens", LeagueSamples.BiggestWin());
    [Fact] public void Goals_scored() => Assert.Equal(["Blues:5", "Greens:1", "Reds:4"], LeagueSamples.GoalsScored());
    [Fact] public void Goal_difference() => Assert.Equal(["Blues:1", "Greens:-3", "Reds:2"], LeagueSamples.GoalDifference());
    [Fact] public void Unbeaten() => Assert.Equal(["Reds"], LeagueSamples.Unbeaten());
    [Fact] public void Home_wins() => Assert.Equal(3, LeagueSamples.HomeWins());
    [Fact] public void Clean_sheets() => Assert.Equal(["Greens", "Reds"], LeagueSamples.CleanSheets());
}

public class FlightSampleTests
{
    [Fact] public void Cheapest() => Assert.Equal("E5", FlightSamples.Cheapest());
    [Fact] public void From_osl() => Assert.Equal(["A1", "B2"], FlightSamples.FromOsl());
    [Fact] public void Departures() => Assert.Equal(["CDG:1", "LHR:2", "OSL:2"], FlightSamples.Departures());
    [Fact] public void Airports() => Assert.Equal(["CDG", "JFK", "LHR", "OSL"], FlightSamples.Airports());
    [Fact] public void Longest() => Assert.Equal("D4", FlightSamples.Longest());
    [Fact] public void Total_hours() => Assert.Equal(21.75, FlightSamples.TotalHours());
    [Fact] public void Itineraries() => Assert.Equal(["A1+C3", "B2+D4"], FlightSamples.Itineraries());
    [Fact] public void Cheapest_itinerary() => Assert.Equal(650, FlightSamples.CheapestItinerary());
    [Fact] public void Busiest() => Assert.Equal("JFK", FlightSamples.Busiest());
    [Fact] public void With_return() => Assert.Equal(["A1", "E5"], FlightSamples.WithReturn());
}

public class ContactSampleTests
{
    [Fact] public void Duplicate_phones() => Assert.Equal(["555-1111"], ContactSamples.DuplicatePhones());
    [Fact] public void Cities() => Assert.Equal(["Oslo", "Paris", "Rome"], ContactSamples.Cities());
    [Fact] public void Oldest() => Assert.Equal("Cy", ContactSamples.Oldest());
    [Fact] public void Same_age() => Assert.Equal(["25:Bob,Di"], ContactSamples.SameAge());
    [Fact] public void Per_city() => Assert.Equal(["Oslo:2", "Paris:1", "Rome:2"], ContactSamples.PerCity());
    [Fact] public void Rome_average_age() => Assert.Equal(30.0, ContactSamples.RomeAverageAge());
    [Fact] public void By_city_then_name() => Assert.Equal(["Ann", "Cy", "Di", "Bob", "Ed"], ContactSamples.ByCityThenName());
    [Fact] public void Initials() => Assert.Equal("ABCDE", ContactSamples.Initials());
    [Fact] public void Missing_city() => Assert.Equal(0, ContactSamples.MissingCity());
    [Fact] public void Age_decades() => Assert.Equal(["20:2", "30:2", "40:1"], ContactSamples.AgeDecades());
}

public class JobSampleTests
{
    [Fact] public void Open_by_priority() => Assert.Equal(["deploy", "review", "test"], JobSamples.OpenByPriority());
    [Fact] public void Hours_remaining() => Assert.Equal(7, JobSamples.HoursRemaining());
    [Fact] public void Percent_done() => Assert.Equal(40, JobSamples.PercentDone());
    [Fact] public void Hours_per_priority() => Assert.Equal(["1:4", "2:7", "3:1"], JobSamples.HoursPerPriority());
    [Fact] public void Next() => Assert.Equal("deploy", JobSamples.Next());
    [Fact] public void Has_urgent() => Assert.True(JobSamples.HasUrgent());
    [Fact] public void Long_titles() => Assert.Equal(["write", "deploy", "review"], JobSamples.LongTitles());
    [Fact] public void Id_pairs() => Assert.Equal(["1,2", "3,4", "5"], JobSamples.IdPairs());
    [Fact] public void Done_versus_open() => Assert.Equal("done:2 open:3", JobSamples.DoneVersusOpen());
    [Fact] public void Longest_title() => Assert.Equal("deploy", JobSamples.LongestTitle());
}

public class CarSampleTests
{
    [Fact] public void Per_brand() => Assert.Equal(["audi:1", "bmw:2", "vw:2"], CarSamples.PerBrand());
    [Fact] public void Lowest_mileage() => Assert.Equal("B2", CarSamples.LowestMileage());
    [Fact] public void Average_km() => Assert.Equal(100000.0, CarSamples.AverageKm());
    [Fact] public void Oldest() => Assert.Equal("D4", CarSamples.Oldest());
    [Fact] public void Red() => Assert.Equal(["A1", "E5"], CarSamples.Red());
    [Fact] public void Color_count() => Assert.Equal(3, CarSamples.ColorCount());
    [Fact] public void Km_per_brand() => Assert.Equal(["audi:200000", "bmw:50000", "vw:100000"], CarSamples.KmPerBrand());
    [Fact] public void Total_age() => Assert.Equal(46, CarSamples.TotalAge());
    [Fact] public void High_mileage() => Assert.Equal(["A1", "D4"], CarSamples.HighMileage());
    [Fact] public void Newest_per_color() => Assert.Equal(["black:D4", "blue:B2", "red:E5"], CarSamples.NewestPerColor());
}
