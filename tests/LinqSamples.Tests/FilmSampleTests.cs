namespace LinqSamples.Tests;

public class FilmSampleTests
{
    [Fact] public void Total_minutes() => Assert.Equal(767, FilmSamples.TotalMinutes());
    [Fact] public void Per_director() => Assert.Equal(["Mann:2", "Scott:2", "Spielberg:2"], FilmSamples.PerDirector());
    [Fact] public void Oldest() => Assert.Equal("Jaws", FilmSamples.Oldest());
    [Fact] public void Longest() => Assert.Equal("Heat", FilmSamples.Longest());
    [Fact] public void Top_rated() => Assert.Equal(["Alien", "Blade Runner", "Heat", "Jaws"], FilmSamples.TopRated());
    [Fact] public void Rating_sum_per_director() => Assert.Equal(["Mann:15", "Scott:16", "Spielberg:15"], FilmSamples.RatingSumPerDirector());
    [Fact] public void Decades() => Assert.Equal(["1970:2", "1980:3", "1990:1"], FilmSamples.Decades());
    [Fact] public void Busy_years() => Assert.Equal([1982], FilmSamples.BusyYears());
    [Fact] public void By_title_length() => Assert.Equal(["ET", "Heat", "Jaws", "Alien", "Thief", "Blade Runner"], FilmSamples.ByTitleLength());
    [Fact] public void Same_runtime() => Assert.Equal(["Alien+Blade Runner", "Jaws+Thief"], FilmSamples.SameRuntime());
}
