namespace LinqSamples.Tests;

public class GymSampleTests
{
    [Fact] public void Total_minutes() => Assert.Equal(275, GymSamples.TotalMinutes());
    [Fact] public void Minutes_per_member() => Assert.Equal(["Ann:75", "Bob:80", "Cy:120"], GymSamples.MinutesPerMember());
    [Fact] public void Most_active() => Assert.Equal("Cy", GymSamples.MostActive());
    [Fact] public void Busy_days() => Assert.Equal(["Mon", "Wed"], GymSamples.BusyDays());
    [Fact] public void Average_calories() => Assert.Equal(270.0, GymSamples.AverageCalories());
    [Fact] public void Most_efficient() => Assert.Equal("Bob", GymSamples.MostEfficient());
    [Fact] public void Long_sessions() => Assert.Equal(3, GymSamples.LongSessions());
    [Fact] public void Calories_ranking() => Assert.Equal(["Cy:840", "Bob:550", "Ann:500"], GymSamples.CaloriesRanking());
    [Fact] public void Monday_members() => Assert.Equal(["Ann", "Bob"], GymSamples.MondayMembers());
    [Fact] public void Top_two() => Assert.Equal(["Bob", "Cy"], GymSamples.TopTwo());
}

public class ParkingSampleTests
{
    [Fact] public void Total_fees() => Assert.Equal(42, ParkingSamples.TotalFees());
    [Fact] public void Fees_per_lot() => Assert.Equal(["East:20", "North:14", "South:8"], ParkingSamples.FeesPerLot());
    [Fact] public void Longest_stay() => Assert.Equal("D4", ParkingSamples.LongestStay());
    [Fact] public void Repeat_plates() => Assert.Equal(["A1", "B2"], ParkingSamples.RepeatPlates());
    [Fact] public void Average_hours() => Assert.Equal(3.5, ParkingSamples.AverageHours());
    [Fact] public void Lots_per_plate() => Assert.Equal(["A1:2", "B2:2", "C3:1", "D4:1"], ParkingSamples.LotsPerPlate());
    [Fact] public void Over_three_hours() => Assert.Equal(["B2", "D4"], ParkingSamples.OverThreeHours());
    [Fact] public void Cheapest_lot() => Assert.Equal("South", ParkingSamples.CheapestLot());
    [Fact] public void Flat_rate() => Assert.True(ParkingSamples.FlatRate());
    [Fact] public void Last_two() => Assert.Equal(["D4", "B2"], ParkingSamples.LastTwo());
}
