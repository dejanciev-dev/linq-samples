namespace LinqSamples.Tests;

public class AdvancedCompositionSampleTests
{
    [Fact] public void Pivot() => Assert.Equal(["N: Q1=10 Q2=20", "S: Q1=12 Q2=1"], AdvancedCompositionSamples.Pivot());
    [Fact] public void Running_balance() => Assert.Equal(["100", "70", "-20!", "30", "-40!"], AdvancedCompositionSamples.RunningBalance());
    [Fact] public void Sessions() => Assert.Equal(["0-25 (3)", "100-110 (2)", "300-300 (1)"], AdvancedCompositionSamples.Sessions());
    [Fact] public void Cartesian() => Assert.Equal(["1-3-4", "1-3-5", "2-3-4", "2-3-5"], AdvancedCompositionSamples.CartesianProduct());
    [Fact] public void Power_set() => Assert.Equal(["{}", "{1}", "{2}", "{1,2}", "{3}", "{1,3}", "{2,3}", "{1,2,3}"], AdvancedCompositionSamples.PowerSet());
    [Fact] public void Four_queens() => Assert.Equal(["1302", "2031"], AdvancedCompositionSamples.FourQueens());
    [Fact] public void Edit_distance() => Assert.Equal(3, AdvancedCompositionSamples.EditDistance());
    [Fact] public void Common_prefix() => Assert.Equal("fl", AdvancedCompositionSamples.CommonPrefix());
}
