namespace LinqSamples.Tests;

public class CleaningSampleTests
{
    [Fact] public void Trim_non_empty() => Assert.Equal(["Ada", "bob", "Cy", "ada", "Di"], CleaningSamples.TrimNonEmpty());
    [Fact] public void Distinct_ignore_case() => Assert.Equal(["Ada", "bob", "Cy", "Di"], CleaningSamples.DistinctIgnoreCase());
    [Fact] public void Title_case() => Assert.Equal(["Ada", "Bob", "Cy", "Di"], CleaningSamples.TitleCase());
    [Fact] public void Invalid_count() => Assert.Equal(3, CleaningSamples.InvalidCount());
    [Fact] public void With_default() => Assert.Equal("Ada|bob|||Cy|ada|n/a|Di", CleaningSamples.WithDefault());
    [Fact] public void Safe_parse() => Assert.Equal([1, 3, 7], CleaningSamples.SafeParse());
    [Fact] public void Bad_inputs() => Assert.Equal(["1:x", "3:", "4:4.5"], CleaningSamples.BadInputs());
    [Fact] public void All_valid() => Assert.Equal("False", CleaningSamples.AllValid());
    [Fact] public void Duplicate_ids() => Assert.Equal([2, 3], CleaningSamples.DuplicateIds());
    [Fact] public void Unique_ids() => Assert.Equal([1], CleaningSamples.UniqueIds());
    [Fact] public void Missing_numbers() => Assert.Equal([3, 5, 6], CleaningSamples.MissingNumbers());
    [Fact] public void Clamped() => Assert.Equal([0, 3, 10, 8], CleaningSamples.Clamped());

}

public class CleaningExtraTests
{
    [Fact] public void Collapse_spaces() => Assert.Equal("a b c", CleaningSamples.CollapseSpaces());
    [Fact] public void Digits_only() => Assert.Equal("123", CleaningSamples.DigitsOnly());
    [Fact] public void Last_valid() => Assert.Equal("Di", CleaningSamples.LastValid());
}

public class CleaningMoreTests
{
    [Fact] public void Valid_sum() => Assert.Equal(11, CleaningSamples.ValidSum());
    [Fact] public void Longest_name() => Assert.Equal("Ada", CleaningSamples.LongestName());
    [Fact] public void Sorted_unique() => Assert.Equal(["Ada", "bob", "Cy", "Di"], CleaningSamples.SortedUnique());
    [Fact] public void Initials() => Assert.Equal("ABCD", CleaningSamples.Initials());
}
