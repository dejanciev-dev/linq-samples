namespace LinqSamples.Tests;

public class AdvancedLinqSampleTests
{
    [Fact] public void Pivot() => Assert.Equal(["EU: A=9, B=3", "US: A=8, B=2"], AdvancedLinqSamples.Pivot());
    [Fact] public void Running_per_group() => Assert.Equal(["EU: 5,8,12", "US: 7,9,10"], AdvancedLinqSamples.RunningPerGroup());
    [Fact] public void Consecutive_runs() => Assert.Equal(["a2", "b1", "c3", "d1"], AdvancedLinqSamples.ConsecutiveRuns());
    [Fact] public void Flatten_tree() => Assert.Equal([1, 2, 4, 3], AdvancedLinqSamples.FlattenTree());
    [Fact] public void Fibonacci() => Assert.Equal([0, 1, 1, 2, 3, 5, 8, 13, 21, 34], AdvancedLinqSamples.Fibonacci());
    [Fact] public void Pythagorean_triples() => Assert.Equal(["3,4,5", "5,12,13", "6,8,10", "8,15,17", "9,12,15", "12,16,20"], AdvancedLinqSamples.PythagoreanTriples());
    [Fact] public void Power_set() => Assert.Equal(["{}", "{a}", "{b}", "{ab}", "{c}", "{ac}", "{bc}", "{abc}"], AdvancedLinqSamples.PowerSet());
    [Fact] public void Transpose() => Assert.Equal(["1,4", "2,5", "3,6"], AdvancedLinqSamples.Transpose());
    [Fact] public void Max_per_group_with_ties() => Assert.Equal(["A:US:7", "B:EU:3"], AdvancedLinqSamples.MaxPerGroupWithTies());
    [Fact] public void Islands() => Assert.Equal(["1-3", "7-8", "10"], AdvancedLinqSamples.Islands());
    [Fact] public void Collatz() => Assert.Equal([6, 3, 10, 5, 16, 8, 4, 2, 1], AdvancedLinqSamples.Collatz());
    [Fact] public void Weighted_average() => Assert.Equal(2.0, AdvancedLinqSamples.WeightedAverage(), 4);
    [Fact] public void Distinct_composite() => Assert.Equal(["EUA", "EUB", "USA", "USB"], AdvancedLinqSamples.DistinctComposite());
}
