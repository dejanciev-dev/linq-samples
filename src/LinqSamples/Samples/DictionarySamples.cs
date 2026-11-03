namespace LinqSamples.Samples;

/// <summary>Building, querying and reshaping dictionaries, lookups and sets with LINQ.</summary>
[SampleCategory("Dictionaries & lookups", 33)]
public static class DictionarySamples
{
    private static readonly string[] Words = ["apple", "avocado", "banana", "blueberry", "cherry", "apricot", "cranberry"];

    private static readonly Dictionary<string, int> Stock = new() { ["Pen"] = 120, ["Notebook"] = 15, ["Mouse"] = 0, ["Monitor"] = 7 };

    [Sample("ToDictionary", Description = "Index a sequence by a unique key; duplicate keys throw.")]
    public static IEnumerable<string> ToDictionarySample()
    {
        var byFirst = Words.ToDictionary(w => w[..3]);
        var result = new List<string> { $"Keys: {string.Join(", ", byFirst.Keys)}" };
        try
        {
            _ = Words.ToDictionary(w => w[0]);
        }
        catch (ArgumentException)
        {
            result.Add("Duplicate key: ArgumentException");
        }

        return result;
    }

    [Sample("ToLookup", Description = "Like a dictionary of groups: missing keys return an empty sequence instead of throwing.")]
    public static IEnumerable<string> ToLookupSample()
    {
        var lookup = Words.ToLookup(w => w[0]);
        return
        [
            $"a: {string.Join(", ", lookup['a'])}",
            $"z: {lookup['z'].Count()} items",
            $"Contains b: {lookup.Contains('b')}",
        ];
    }

    [Sample("Filter a dictionary", Description = "Where over key/value pairs, then back to a dictionary.")]
    public static IEnumerable<string> FilterDictionary() =>
        Stock.Where(kv => kv.Value > 0).ToDictionary().Select(kv => $"{kv.Key}={kv.Value}");

    [Sample("Transform values", Description = "Project values while keeping the keys.")]
    public static IEnumerable<string> TransformValues() =>
        Stock.ToDictionary(kv => kv.Key, kv => kv.Value > 10 ? "plenty" : kv.Value > 0 ? "low" : "out")
            .Select(kv => $"{kv.Key}: {kv.Value}");

    [Sample("Invert a dictionary", Description = "Swap keys and values; group when values repeat.")]
    public static IEnumerable<string> Invert()
    {
        var regions = new Dictionary<string, string> { ["France"] = "EU", ["Spain"] = "EU", ["Japan"] = "Asia" };
        return regions.GroupBy(kv => kv.Value, kv => kv.Key)
            .OrderBy(g => g.Key)
            .Select(g => $"{g.Key}: {string.Join(", ", g.Order())}");
    }

    [Sample("Merge dictionaries", Description = "Combine several dictionaries; the last one wins on conflicts.")]
    public static IEnumerable<string> Merge()
    {
        var defaults = new Dictionary<string, string> { ["theme"] = "light", ["lang"] = "en" };
        var user = new Dictionary<string, string> { ["theme"] = "dark", ["font"] = "mono" };
        return defaults.Concat(user)
            .GroupBy(kv => kv.Key)
            .ToDictionary(g => g.Key, g => g.Last().Value)
            .OrderBy(kv => kv.Key)
            .Select(kv => $"{kv.Key}={kv.Value}");
    }

    [Sample("Diff two dictionaries", Description = "Added, removed and changed keys between two snapshots.")]
    public static IEnumerable<string> Diff()
    {
        var before = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2, ["c"] = 3 };
        var after = new Dictionary<string, int> { ["b"] = 2, ["c"] = 30, ["d"] = 4 };
        return
        [
            $"Added:   {string.Join(", ", after.Keys.Except(before.Keys))}",
            $"Removed: {string.Join(", ", before.Keys.Except(after.Keys))}",
            $"Changed: {string.Join(", ", before.Keys.Intersect(after.Keys).Where(k => before[k] != after[k]))}",
        ];
    }

    [Sample("Dictionary of lists", Description = "Group into a Dictionary<K, List<V>> for later mutation.")]
    public static IEnumerable<string> DictionaryOfLists()
    {
        var map = Words.GroupBy(w => w[0]).ToDictionary(g => g.Key, g => g.ToList());
        map['a'].Add("almond");
        return map.OrderBy(kv => kv.Key).Select(kv => $"{kv.Key}: {kv.Value.Count}");
    }

    [Sample("Top N per key", Description = "Keep the two longest words per first letter.")]
    public static IEnumerable<string> TopPerKey() =>
        Words.GroupBy(w => w[0])
            .OrderBy(g => g.Key)
            .Select(g => $"{g.Key}: {string.Join(", ", g.OrderByDescending(w => w.Length).ThenBy(w => w).Take(2))}");

    [Sample("Count occurrences", Description = "CountBy gives a frequency table without a GroupBy.")]
    public static IEnumerable<string> Frequencies() =>
        "mississippi".CountBy(c => c).OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).Select(kv => $"{kv.Key}: {kv.Value}");

    [Sample("Safe lookups", Description = "TryGetValue and GetValueOrDefault instead of exceptions.")]
    public static IEnumerable<string> SafeLookups() =>
        new[] { "Pen", "Stapler" }.Select(name =>
            Stock.TryGetValue(name, out var qty) ? $"{name}: {qty}" : $"{name}: not stocked ({Stock.GetValueOrDefault(name, -1)})");

    [Sample("Join with a dictionary", Description = "Look names up in a dictionary instead of running a Join.")]
    public static IEnumerable<string> DictionaryJoin()
    {
        var prices = new Dictionary<string, decimal> { ["Pen"] = 1.5m, ["Notebook"] = 4m, ["Mouse"] = 20m };
        return Stock.Where(kv => prices.ContainsKey(kv.Key))
            .Select(kv => FormattableString.Invariant($"{kv.Key}: {kv.Value * prices[kv.Key]:0.00}"));
    }

    [Sample("Sort a dictionary", Description = "Dictionaries are unordered: order the pairs, not the dictionary.")]
    public static IEnumerable<string> SortDictionary() =>
        Stock.OrderByDescending(kv => kv.Value).Select(kv => kv.Key);

    [Sample("HashSet operations", Description = "Set algebra: union, intersect, symmetric difference, subset.")]
    public static IEnumerable<string> HashSetOps()
    {
        var a = new HashSet<int> { 1, 2, 3, 4 };
        var b = new HashSet<int> { 3, 4, 5 };
        return
        [
            $"Union:     {string.Join(",", a.Union(b).Order())}",
            $"Intersect: {string.Join(",", a.Intersect(b).Order())}",
            $"Symmetric: {string.Join(",", a.Except(b).Concat(b.Except(a)).Order())}",
            $"Subset:    {new HashSet<int> { 3, 4 }.IsSubsetOf(a)}",
        ];
    }

    [Sample("Case-insensitive keys", Description = "Pass a comparer to ToDictionary, GroupBy or Distinct.")]
    public static IEnumerable<string> CaseInsensitive()
    {
        string[] tags = ["LINQ", "linq", "C#", "Linq", "c#"];
        var counts = tags.GroupBy(t => t, StringComparer.OrdinalIgnoreCase).Select(g => $"{g.Key}: {g.Count()}");
        return counts.Append($"Distinct: {tags.Distinct(StringComparer.OrdinalIgnoreCase).Count()}");
    }
}
