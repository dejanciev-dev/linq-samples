namespace LinqSamples.Tests;

public class MatchSampleTests
{
    [Fact] public void Total_goals() => Assert.Equal(12, MatchSamples.TotalGoals());
    [Fact] public void Draws() => Assert.Equal(2, MatchSamples.Draws());
    [Fact] public void Biggest_win() => Assert.Equal("Blues-Greens", MatchSamples.BiggestWin());
    [Fact] public void Home_winners() => Assert.Equal(["Blues", "Reds"], MatchSamples.HomeWinners());
    [Fact] public void Goals_per_team() => Assert.Equal(["Blues:5", "Greens:3", "Reds:4"], MatchSamples.GoalsPerTeam());
    [Fact] public void Matches_per_team() => Assert.Equal(["Blues:3", "Greens:3", "Reds:4"], MatchSamples.MatchesPerTeam());
    [Fact] public void Points_per_team() => Assert.Equal(["Blues:4", "Greens:4", "Reds:5"], MatchSamples.PointsPerTeam());
    [Fact] public void Leader() => Assert.Equal("Reds", MatchSamples.Leader());
    [Fact] public void Teams() => Assert.Equal(["Blues", "Greens", "Reds"], MatchSamples.Teams());
    [Fact] public void High_scoring() => Assert.Equal(["Reds-Blues", "Blues-Greens", "Reds-Greens"], MatchSamples.HighScoring());
}
