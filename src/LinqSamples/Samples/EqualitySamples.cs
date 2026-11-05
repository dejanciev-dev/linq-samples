namespace LinqSamples.Samples;

/// <summary>Comparing sequences, sets and strings with LINQ.</summary>
[SampleCategory("Equality & comparison", 42)]
public static class EqualitySamples
{
    [Sample("Sequences equal", Description = "SequenceEqual compares length, order and items.")]
    public static string SeqEqual() => $"{new[] { 1, 2, 3 }.SequenceEqual([1, 2, 3])}";

    [Sample("Equal ignoring order", Description = "Order both sides, then SequenceEqual.")]
    public static string Unordered() => $"{new[] { 3, 1, 2 }.Order().SequenceEqual(new[] { 1, 2, 3 }.Order())}";

    [Sample("Equal with a comparer", Description = "SequenceEqual accepts an IEqualityComparer.")]
    public static string WithComparer() => $"{new[] { "a", "B" }.SequenceEqual(["A", "b"], StringComparer.OrdinalIgnoreCase)}";

    [Sample("Contains with a comparer", Description = "Contains also takes an IEqualityComparer.")]
    public static string ContainsIgnoreCase() => $"{new[] { "Ada", "Bob" }.Contains("ADA", StringComparer.OrdinalIgnoreCase)}";

    [Sample("Same set of items", Description = "HashSet.SetEquals ignores order and duplicates.")]
    public static string SameSet() => $"{new HashSet<int>([1, 2, 2]).SetEquals([2, 1])}";

    [Sample("Is subset", Description = "IsSubsetOf checks every item exists in the other set.")]
    public static string Subset() => $"{new HashSet<int>([1, 2]).IsSubsetOf([1, 2, 3, 4])}";

    [Sample("Disjoint sequences", Description = "No Intersect results means no overlap.")]
    public static string Disjoint() => $"{!new[] { 1, 2 }.Intersect([3, 4]).Any()}";

    [Sample("Distinct by key", Description = "DistinctBy keeps the first item per key.")]
    public static IEnumerable<string> DistinctByLength() => new[] { "a", "bb", "c", "dd", "eee" }.DistinctBy(s => s.Length);

    [Sample("Longest item", Description = "MaxBy returns the element, not the key.")]
    public static string? Longest() => new[] { "a", "bb", "ccc", "dd" }.MaxBy(s => s.Length);

    [Sample("Hamming distance", Description = "Zip two strings and count mismatched positions.")]
    public static int Hamming() => "karolin".Zip("kathrin").Count(p => p.First != p.Second);

    [Sample("Symmetric difference", Description = "Except both ways, then Union.")]
    public static IEnumerable<int> SymmetricDiff()
    {
        int[] a = [1, 2, 3], b = [2, 3, 4];
        return a.Except(b).Union(b.Except(a));
    }

    [Sample("Is superset", Description = "IsSupersetOf is the mirror of IsSubsetOf.")]
    public static string Superset() => $"{new HashSet<int>([1, 2, 3]).IsSupersetOf([1, 3])}";

    [Sample("Common items", Description = "Intersect keeps items present in both.")]
    public static IEnumerable<int> Common() => new[] { 1, 2, 3, 4 }.Intersect([3, 4, 5]);

    [Sample("Anagram check", Description = "Sorted letters of both words are equal.")]
    public static string Anagram() => $"{"listen".Order().SequenceEqual("silent".Order())}";

    [Sample("Palindrome check", Description = "A sequence equals its reverse.")]
    public static string Palindrome() => $"{"level".SequenceEqual("level".Reverse())}";

    [Sample("Anagrams", Description = "Two words are anagrams when their ordered letters are equal.")]
    public static string Anagrams() => $"{"listen".Order().SequenceEqual("silent".Order())}";

    [Sample("Count common items", Description = "Intersect then Count.")]
    public static int CommonCount() => new[] { 1, 2, 3, 4 }.Intersect([3, 4, 5]).Count();

    [Sample("Same items, same counts", Description = "CountBy on both sides, ordered, then SequenceEqual compares multisets.")]
    public static string SameMultiset() =>
        $"{new[] { 1, 1, 2 }.CountBy(x => x).OrderBy(p => p.Key).SequenceEqual(new[] { 1, 2, 1 }.CountBy(x => x).OrderBy(p => p.Key))}";
}
