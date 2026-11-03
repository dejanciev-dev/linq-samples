namespace LinqSamples.Tests;

public class DictionarySampleTests
{
    [Fact]
    public void ToLookup_missing_key_is_empty() =>
        Assert.Equal(["a: apple, avocado, apricot", "z: 0 items", "Contains b: True"], DictionarySamples.ToLookupSample());

    [Fact]
    public void Merge_last_wins() =>
        Assert.Equal(["font=mono", "lang=en", "theme=dark"], DictionarySamples.Merge());

    [Fact]
    public void Diff_reports_changes() =>
        Assert.Equal(["Added:   d", "Removed: a", "Changed: c"], DictionarySamples.Diff());

    [Fact]
    public void Frequencies_are_sorted() =>
        Assert.Equal(["i: 4", "s: 4", "p: 2", "m: 1"], DictionarySamples.Frequencies());

    [Fact]
    public void Case_insensitive_grouping() =>
        Assert.Equal(["LINQ: 3", "C#: 2", "Distinct: 2"], DictionarySamples.CaseInsensitive());

    [Fact]
    public void Top_per_key() =>
        Assert.Equal(["a: apricot, avocado", "b: blueberry, banana", "c: cranberry, cherry"], DictionarySamples.TopPerKey());
}
