namespace LinqSamples.Tests;

public class MusicSampleTests
{
    [Fact] public void Total_minutes() => Assert.Equal(256, MusicSamples.TotalMinutes());
    [Fact] public void Per_artist() => Assert.Equal(["Beatles:2", "Jackson:2", "Nirvana:2"], MusicSamples.PerArtist());
    [Fact] public void Oldest() => Assert.Equal("Revolver", MusicSamples.Oldest());
    [Fact] public void Longest() => Assert.Equal("Bad", MusicSamples.Longest());
    [Fact] public void Tracks_per_artist() => Assert.Equal(["Beatles:31", "Jackson:20", "Nirvana:25"], MusicSamples.TracksPerArtist());
    [Fact] public void Decades() => Assert.Equal(["1960:2", "1980:2", "1990:2"], MusicSamples.Decades());
    [Fact] public void Long_albums() => Assert.Equal(["Abbey Road", "Bad", "Nevermind"], MusicSamples.LongAlbums());
    [Fact] public void Average_minutes() => Assert.Equal(42.7, MusicSamples.AverageMinutes());
    [Fact] public void By_title_length() => Assert.Equal(["Bad", "Utero", "Revolver", "Thriller", "Nevermind", "Abbey Road"], MusicSamples.ByTitleLength());
    [Fact] public void Artists_with_long_album() => Assert.Equal(["Beatles", "Jackson"], MusicSamples.ArtistsWithLongAlbum());
}
