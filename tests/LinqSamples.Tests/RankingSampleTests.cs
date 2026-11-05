namespace LinqSamples.Tests;

public class RankingSampleTests
{
    [Fact] public void Leaderboard() => Assert.Equal(["Ada:90", "Cy:90", "Bob:75", "Ed:75", "Di:60", "Flo:40"], RankingSamples.Leaderboard());
    [Fact] public void Competition_rank() => Assert.Equal(["Ada=1", "Cy=1", "Bob=3", "Ed=3", "Di=5", "Flo=6"], RankingSamples.CompetitionRank());
    [Fact] public void Dense_rank() => Assert.Equal(["Ada=1", "Cy=1", "Bob=2", "Ed=2", "Di=3", "Flo=4"], RankingSamples.DenseRank());
    [Fact] public void Top_three() => Assert.Equal(["Ada", "Cy", "Bob", "Ed", "Di"], RankingSamples.TopThreeWithTies());
    [Fact] public void By_score() => Assert.Equal(["90: Ada,Cy", "75: Bob,Ed", "60: Di", "40: Flo"], RankingSamples.ByScore());
    [Fact] public void Percentile() => Assert.Equal(["Ada=67", "Bob=33", "Cy=67", "Di=17", "Ed=33", "Flo=0"], RankingSamples.PercentileRank());
    [Fact] public void Median() => Assert.Equal(75, RankingSamples.Median());
    [Fact] public void Rank_changes() => Assert.Equal(["Cy:+2", "Ada:-1", "Di:+1", "Bob:-2"], RankingSamples.RankChanges());
    [Fact] public void Best_and_worst() => Assert.Equal(["high: best=Ada worst=Bob", "low: best=Di worst=Flo"], RankingSamples.BestAndWorst());
    [Fact] public void Weighted() => Assert.Equal(78, RankingSamples.WeightedAverage());
}
