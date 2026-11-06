namespace LinqSamples.Tests;

public class WeatherSampleTests
{
    [Fact] public void Hottest() => Assert.Equal("Sat", WeatherSamples.Hottest());
    [Fact] public void Coldest() => Assert.Equal("Wed", WeatherSamples.Coldest());
    [Fact] public void Average_rain() => Assert.Equal(4.0, WeatherSamples.AverageRain());
    [Fact] public void Dry_days() => Assert.Equal(["Mon", "Thu", "Sat"], WeatherSamples.DryDays());
    [Fact] public void Rainy_days() => Assert.Equal(4, WeatherSamples.RainyDays());
    [Fact] public void Narrowest_range() => Assert.Equal("Fri", WeatherSamples.NarrowestRange());
    [Fact] public void Warmer_than_yesterday() => Assert.Equal(["Tue", "Thu", "Sat"], WeatherSamples.WarmerThanYesterday());
    [Fact] public void Total_rain() => Assert.Equal(28, WeatherSamples.TotalRain());
    [Fact] public void Above_average() => Assert.Equal(["Tue", "Thu", "Sat"], WeatherSamples.AboveAverage());
    [Fact] public void Weekend_high() => Assert.Equal(24.5, WeatherSamples.WeekendHigh());
}

public class MovieSampleTests
{
    [Fact] public void Top_rated() => Assert.Equal("Se7en", MovieSamples.TopRated());
    [Fact] public void Per_genre() => Assert.Equal(["crime:3", "family:2", "scifi:2"], MovieSamples.PerGenre());
    [Fact] public void Best_per_genre() => Assert.Equal(["crime:86", "family:84", "scifi:85"], MovieSamples.BestPerGenre());
    [Fact] public void Short() => Assert.Equal(["Fargo", "Up"], MovieSamples.Short());
    [Fact] public void Total_minutes() => Assert.Equal(829, MovieSamples.TotalMinutes());
    [Fact] public void Longest() => Assert.Equal("Heat", MovieSamples.Longest());
    [Fact] public void From_1995() => Assert.Equal(["Heat", "Se7en"], MovieSamples.From1995());
    [Fact] public void Newest_per_genre() => Assert.Equal(["crime:Fargo", "family:Coco", "scifi:Arrival"], MovieSamples.NewestPerGenre());
    [Fact] public void High_scores() => Assert.Equal(["Se7en", "Alien", "Coco", "Heat"], MovieSamples.HighScores());
    [Fact] public void Per_decade() => Assert.Equal(["1970:1", "1990:3", "2000:1", "2010:2"], MovieSamples.PerDecade());
}

public class EmployeeSampleTests
{
    [Fact] public void Payroll() => Assert.Equal(24700, EmployeeSamples.Payroll());
    [Fact] public void Average_per_dept() => Assert.Equal(["eng:5000", "hr:3200", "ops:3250"], EmployeeSamples.AveragePerDept());
    [Fact] public void Highest_paid() => Assert.Equal("Fay", EmployeeSamples.HighestPaid());
    [Fact] public void Veterans() => Assert.Equal(["Ana", "Cat", "Fay"], EmployeeSamples.Veterans());
    [Fact] public void Payroll_after_raise() => Assert.Equal(26100, EmployeeSamples.PayrollAfterRaise());
    [Fact] public void Headcount() => Assert.Equal(["eng:3", "hr:1", "ops:2"], EmployeeSamples.Headcount());
    [Fact] public void Top_per_dept() => Assert.Equal(["eng:Fay", "hr:Eve", "ops:Dan"], EmployeeSamples.TopPerDept());
    [Fact] public void Rich_departments() => Assert.Equal(["eng", "ops"], EmployeeSamples.RichDepartments());
    [Fact] public void Ranking() => Assert.Equal(["1.Fay", "2.Ana", "3.Ben", "4.Dan", "5.Eve", "6.Cat"], EmployeeSamples.Ranking());
    [Fact] public void Pay_gap() => Assert.Equal(["eng:2000", "hr:0", "ops:500"], EmployeeSamples.PayGap());
}

public class PlaylistSampleTests
{
    [Fact] public void Total_seconds() => Assert.Equal(1020, PlaylistSamples.TotalSeconds());
    [Fact] public void Most_played() => Assert.Equal("Star", PlaylistSamples.MostPlayed());
    [Fact] public void Plays_per_artist() => Assert.Equal(["Leo:90", "Mia:80", "Zed:60"], PlaylistSamples.PlaysPerArtist());
    [Fact] public void Longest_per_artist() => Assert.Equal(["Leo:Star", "Mia:Sun", "Zed:Sea"], PlaylistSamples.LongestPerArtist());
    [Fact] public void Short() => Assert.Equal(["Moon", "Sea"], PlaylistSamples.Short());
    [Fact] public void Top_two() => Assert.Equal(["Star", "Sea"], PlaylistSamples.TopTwo());
    [Fact] public void Listening_seconds() => Assert.Equal(48100, PlaylistSamples.ListeningSeconds());
    [Fact] public void Multi_track_artists() => Assert.Equal(["Leo", "Mia"], PlaylistSamples.MultiTrackArtists());
    [Fact] public void Length() => Assert.Equal("17:00", PlaylistSamples.Length());
    [Fact] public void Titles() => Assert.Equal(["Moon", "Sea", "Sky", "Star", "Sun"], PlaylistSamples.Titles());
}

public class OrderSampleTests
{
    [Fact] public void Paid_revenue() => Assert.Equal(190m, OrderSamples.PaidRevenue());
    [Fact] public void Unpaid() => Assert.Equal([2, 6], OrderSamples.Unpaid());
    [Fact] public void Per_customer() => Assert.Equal(["ann:80", "bob:60", "cy:80"], OrderSamples.PerCustomer());
    [Fact] public void Best_customer() => Assert.Equal("ann", OrderSamples.BestCustomer());
    [Fact] public void Largest() => Assert.Equal(4, OrderSamples.Largest());
    [Fact] public void Average_paid() => Assert.Equal(47.5m, OrderSamples.AveragePaid());
    [Fact] public void With_unpaid() => Assert.Equal(["bob", "cy"], OrderSamples.WithUnpaid());
    [Fact] public void Always_paid() => Assert.Equal(["ann"], OrderSamples.AlwaysPaid());
    [Fact] public void Running_total() => Assert.Equal([50m, 70m, 100m, 170m, 210m, 220m], OrderSamples.RunningTotal());
    [Fact] public void Big_spenders() => Assert.Equal(["ann", "cy"], OrderSamples.BigSpenders());
}
