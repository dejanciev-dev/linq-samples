namespace LinqSamples.Samples;

/// <summary>Larger algorithms assembled from LINQ operators.</summary>
[SampleCategory("Advanced compositions", 39)]
public static class AdvancedCompositionSamples
{
    [Sample("Crosstab / pivot", Description = "Group by row, then build one column per distinct key.")]
    public static IEnumerable<string> Pivot()
    {
        (string Region, string Quarter, int Amount)[] sales =
            [("N", "Q1", 10), ("N", "Q2", 20), ("S", "Q1", 5), ("S", "Q1", 7), ("S", "Q2", 1)];
        var quarters = sales.Select(s => s.Quarter).Distinct().Order().ToArray();
        return sales.GroupBy(s => s.Region).OrderBy(g => g.Key)
            .Select(g => $"{g.Key}: " + string.Join(" ", quarters.Select(q => $"{q}={g.Where(s => s.Quarter == q).Sum(s => s.Amount)}")));
    }

    [Sample("Running balance with overdraft flag", Description = "Aggregate carries the balance list; Select annotates it.")]
    public static IEnumerable<string> RunningBalance() =>
        new[] { 100, -30, -90, 50, -70 }
            .Aggregate(new List<int>(), (acc, t) => { acc.Add((acc.Count > 0 ? acc[^1] : 0) + t); return acc; })
            .Select(b => b < 0 ? $"{b}!" : $"{b}");

    [Sample("Sessionize events", Description = "Start a new session whenever the gap exceeds 30 minutes.")]
    public static IEnumerable<string> Sessions() =>
        new[] { 0, 10, 25, 100, 110, 300 }
            .Aggregate(new List<List<int>>(), (acc, t) =>
            {
                if (acc.Count > 0 && t - acc[^1][^1] <= 30) acc[^1].Add(t);
                else acc.Add([t]);
                return acc;
            })
            .Select(s => $"{s[0]}-{s[^1]} ({s.Count})");

    [Sample("Cartesian product of N sequences", Description = "Aggregate with SelectMany folds any number of lists.")]
    public static IEnumerable<string> CartesianProduct() =>
        new[] { new[] { 1, 2 }, [3], [4, 5] }
            .Aggregate(Enumerable.Repeat(Enumerable.Empty<int>(), 1),
                (acc, seq) => acc.SelectMany(prefix => seq.Select(x => prefix.Append(x))))
            .Select(p => string.Join("-", p));

    [Sample("Power set", Description = "Each element doubles the subsets: the old ones plus the old ones with it added.")]
    public static IEnumerable<string> PowerSet() =>
        new[] { 1, 2, 3 }
            .Aggregate(Enumerable.Repeat(Enumerable.Empty<int>(), 1),
                (acc, x) => acc.Concat(acc.Select(s => s.Append(x))))
            .Select(s => "{" + string.Join(",", s) + "}");

    [Sample("N-Queens (4x4)", Description = "Place one queen per row, keeping only safe columns at each step.")]
    public static IEnumerable<string> FourQueens() =>
        Enumerable.Range(0, 4)
            .Aggregate(Enumerable.Repeat(Array.Empty<int>(), 1), (boards, row) =>
                boards.SelectMany(b => Enumerable.Range(0, 4)
                    .Where(c => b.Select((bc, r) => bc != c && Math.Abs(bc - c) != row - r).All(ok => ok))
                    .Select(c => b.Append(c).ToArray())))
            .Select(b => string.Concat(b));

    [Sample("Levenshtein distance", Description = "Aggregate one dynamic-programming row per character of the first word.")]
    public static int EditDistance()
    {
        const string a = "kitten", b = "sitting";
        var first = Enumerable.Range(0, b.Length + 1).ToArray();
        return a.Select((ch, i) => (ch, i)).Aggregate(first, (prev, t) =>
        {
            var cur = new int[prev.Length];
            cur[0] = t.i + 1;
            for (var j = 1; j < cur.Length; j++)
                cur[j] = new[] { prev[j] + 1, cur[j - 1] + 1, prev[j - 1] + (t.ch == b[j - 1] ? 0 : 1) }.Min();
            return cur;
        })[^1];
    }

    [Sample("Longest common prefix", Description = "TakeWhile over the first word, checking every other word.")]
    public static string CommonPrefix()
    {
        string[] words = ["flower", "flow", "flight"];
        return string.Concat(words[0].TakeWhile((c, i) => words.All(w => i < w.Length && w[i] == c)));
    }
}
