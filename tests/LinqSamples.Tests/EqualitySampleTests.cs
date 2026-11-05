namespace LinqSamples.Tests;

public class EqualitySampleTests
{
    [Fact] public void Seq_equal() => Assert.Equal("True", EqualitySamples.SeqEqual());
    [Fact] public void Unordered() => Assert.Equal("True", EqualitySamples.Unordered());
    [Fact] public void With_comparer() => Assert.Equal("True", EqualitySamples.WithComparer());
    [Fact] public void Contains_ignore_case() => Assert.Equal("True", EqualitySamples.ContainsIgnoreCase());
    [Fact] public void Same_set() => Assert.Equal("True", EqualitySamples.SameSet());
    [Fact] public void Subset() => Assert.Equal("True", EqualitySamples.Subset());
    [Fact] public void Disjoint() => Assert.Equal("True", EqualitySamples.Disjoint());
    [Fact] public void Distinct_by_length() => Assert.Equal(["a", "bb", "eee"], EqualitySamples.DistinctByLength());
    [Fact] public void Longest() => Assert.Equal("ccc", EqualitySamples.Longest());
    [Fact] public void Hamming() => Assert.Equal(3, EqualitySamples.Hamming());
    [Fact] public void Symmetric_diff() => Assert.Equal([1, 4], EqualitySamples.SymmetricDiff());
}

public class EqualityMoreTests
{
    [Fact] public void Superset() => Assert.Equal("True", EqualitySamples.Superset());
    [Fact] public void Common() => Assert.Equal([3, 4], EqualitySamples.Common());
    [Fact] public void Anagram() => Assert.Equal("True", EqualitySamples.Anagram());
    [Fact] public void Palindrome() => Assert.Equal("True", EqualitySamples.Palindrome());
}
