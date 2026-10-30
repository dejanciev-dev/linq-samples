namespace LinqSamples.Samples;

/// <summary>Subsets, permutations, combinations and counting problems as LINQ queries.</summary>
[SampleCategory("Combinatorics", 29)]
public static class CombinatoricsSamples
{
    [Sample("All subsets", Description = "The power set: each bit of a counter decides whether an element is in.")]
    public static IEnumerable<string> Subsets()
    {
        string[] items = ["a", "b", "c"];
        return Enumerable.Range(0, 1 << items.Length)
                         .Select(mask => "{" + string.Join(",", items.Where((_, i) => (mask & (1 << i)) != 0)) + "}");
    }

    [Sample("Permutations", Description = "Pick each element first, then permute the rest - recursively.")]
    public static IEnumerable<string> Permutations() =>
        Permute([1, 2, 3]).Select(p => string.Concat(p));

    [Sample("Combinations", Description = "Choose k of n, order does not matter.")]
    public static IEnumerable<string> Combinations() =>
        Combine(["a", "b", "c", "d"], 2).Select(c => string.Concat(c));

    [Sample("Cartesian product of n lists", Description = "Aggregate SelectMany over any number of lists.")]
    public static IEnumerable<string> CartesianProduct()
    {
        string[][] options = [["S", "M"], ["Red", "Blue"], ["Cotton"]];
        IEnumerable<IEnumerable<string>> seed = [[]];
        return options.Aggregate(seed, (acc, list) => acc.SelectMany(_ => list, (prefix, item) => prefix.Append(item)))
                      .Select(combo => string.Join("-", combo));
    }

    [Sample("Subsets with a target sum", Description = "Filter the power set - which items add up to 9?")]
    public static IEnumerable<string> SubsetSum()
    {
        int[] values = [3, 34, 4, 12, 5, 2];
        const int target = 9;
        return Enumerable.Range(0, 1 << values.Length)
                         .Select(mask => values.Where((_, i) => (mask & (1 << i)) != 0).ToList())
                         .Where(subset => subset.Sum() == target)
                         .Select(subset => string.Join(" + ", subset));
    }

    [Sample("Coin change", Description = "Ways to make 10 from coins 1, 2 and 5 - dynamic programming folded with Aggregate.")]
    public static long CoinChange()
    {
        int[] coins = [1, 2, 5];
        const int amount = 10;
        var ways = coins.Aggregate(
            Enumerable.Range(0, amount + 1).Select(i => i == 0 ? 1L : 0L).ToArray(),
            (w, coin) =>
            {
                for (var i = coin; i <= amount; i++)
                {
                    w[i] += w[i - coin];
                }

                return w;
            });
        return ways[amount];
    }

    [Sample("Unordered pairs", Description = "Every pair once - like handshakes in a room.")]
    public static IEnumerable<string> UnorderedPairs()
    {
        var pairs = (from a in Customers.Index()
                     from b in Customers.Index()
                     where a.Index < b.Index
                     select $"{a.Item.Name}-{b.Item.Name}").ToList();
        return [$"Pairs: {pairs.Count}", .. pairs.Take(3)];
    }

    [Sample("Dice sums", Description = "All outcomes of two dice, counted by total - 7 is the most likely.")]
    public static IEnumerable<string> DiceSums() =>
        (from a in Enumerable.Range(1, 6)
         from b in Enumerable.Range(1, 6)
         select a + b)
        .CountBy(sum => sum)
        .OrderBy(kv => kv.Key)
        .Select(kv => $"{kv.Key}: {kv.Value}");

    [Sample("Derangements", Description = "Permutations where nothing stays in its original place.")]
    public static int Derangements() =>
        Permute([0, 1, 2, 3]).Count(p => p.Index().All(t => t.Item != t.Index));

    [Sample("Distinct anagrams", Description = "Permutations of letters with repeats, de-duplicated.")]
    public static IEnumerable<string> DistinctAnagrams() =>
        Permute("aab".ToList()).Select(p => string.Concat(p)).Distinct();

    [Sample("Gray code", Description = "Successive codes differ in exactly one bit: i XOR (i >> 1).")]
    public static IEnumerable<string> GrayCode() =>
        Enumerable.Range(0, 8).Select(i => Convert.ToString(i ^ (i >> 1), 2).PadLeft(3, '0'));

    [Sample("Integer partitions", Description = "All ways to write 5 as a sum of positive integers, largest part first.")]
    public static IEnumerable<string> IntegerPartitions()
    {
        static IEnumerable<IReadOnlyList<int>> Partitions(int n, int max) =>
            n == 0
                ? [[]]
                : Enumerable.Range(1, Math.Min(n, max)).Reverse()
                            .SelectMany(first => Partitions(n - first, first).Select(rest => (IReadOnlyList<int>)[first, .. rest]));

        return Partitions(5, 5).Select(p => string.Join("+", p));
    }

    private static IEnumerable<IReadOnlyList<T>> Permute<T>(IReadOnlyList<T> items) =>
        items.Count == 0
            ? [[]]
            : items.Index().SelectMany(t => Permute(items.Where((_, i) => i != t.Index).ToList())
                                               .Select(rest => (IReadOnlyList<T>)[t.Item, .. rest]));

    private static IEnumerable<IReadOnlyList<T>> Combine<T>(IReadOnlyList<T> items, int k) =>
        k == 0
            ? [[]]
            : items.Index().SelectMany(t => Combine(items.Skip(t.Index + 1).ToList(), k - 1)
                                               .Select(rest => (IReadOnlyList<T>)[t.Item, .. rest]));
}
