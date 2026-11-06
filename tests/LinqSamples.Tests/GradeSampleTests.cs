namespace LinqSamples.Tests;

public class GradeSampleTests
{
    [Fact] public void Average_per_student() => Assert.Equal(["Ann:80", "Bob:70", "Cy:85", "Di:50"], GradeSamples.AveragePerStudent());
    [Fact] public void Top_student() => Assert.Equal("Cy", GradeSamples.TopStudent());
    [Fact] public void Passing() => Assert.Equal(["Ann", "Bob", "Cy"], GradeSamples.Passing());
    [Fact] public void Best_per_subject() => Assert.Equal(["art:Cy", "math:Ann"], GradeSamples.BestPerSubject());
    [Fact] public void Letter_counts() => Assert.Equal(["A:2", "B:1", "C:2", "F:2"], GradeSamples.LetterCounts());
    [Fact] public void Honor_roll() => Assert.Equal(["Ann", "Cy"], GradeSamples.HonorRoll());
    [Fact] public void Both_subjects() => Assert.Equal(["Ann", "Bob", "Cy"], GradeSamples.BothSubjects());
    [Fact] public void Better_at_art() => Assert.Equal(["Bob", "Cy"], GradeSamples.BetterAtArt());
    [Fact] public void Ranking() => Assert.Equal(["Cy", "Ann", "Bob", "Di"], GradeSamples.Ranking());
    [Fact] public void Total_points() => Assert.Equal(520, GradeSamples.TotalPoints());
    [Fact] public void Median_score() => Assert.Equal(75, GradeSamples.MedianScore());
    [Fact] public void Spread_per_subject() => Assert.Equal(["art:25", "math:40"], GradeSamples.SpreadPerSubject());
    [Fact] public void Lowest_score() => Assert.Equal("Di", GradeSamples.LowestScore());
    [Fact] public void Capped_coupon() => Assert.Equal(10m, CartSamples.CappedCoupon());
    [Fact] public void Top_category() => Assert.Equal("office", CartSamples.TopCategoryByUnits());
}
