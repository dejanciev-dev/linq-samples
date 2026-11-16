namespace LinqSamples.Samples;

/// <summary>Advanced LINQ techniques: custom aggregation, recursion, state machines and composition.</summary>
[SampleCategory("Advanced LINQ", 73)]
public static class AdvancedLinqSamples
{
    private sealed record Sale(string Region, string Product, int Qty, int Day);

    private static readonly Sale[] Sales =
    [
        new("EU", "A", 5, 1), new("EU", "B", 3, 1), new("US", "A", 7, 2),
        new("US", "B", 2, 3), new("EU", "A", 4, 3), new("US", "A", 1, 4),
    ];

    [Sample("Pivot with dictionary", Description = "Group by region, then project each group into a product -> quantity dictionary.")]
    public static IEnumerable<string> Pivot() =>
        Sales.GroupBy(s => s.Region).OrderBy(g => g.Key).Select(g =>
        {
            var byProduct = g.GroupBy(s => s.Product).OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Sum(s => s.Qty)}");
            return $"{g.Key}: {string.Join(", ", byProduct)}";
        });

    [Sample("Running total per group", Description = "Group, then a seeded Aggregate builds a running total inside each group.")]
    public static IEnumerable<string> RunningPerGroup() =>
        Sales.GroupBy(s => s.Region).OrderBy(g => g.Key).Select(g =>
            $"{g.Key}: " + string.Join(",", g.OrderBy(s => s.Day).Aggregate(new List<int>(), (acc, s) =>
            {
                acc.Add((acc.Count > 0 ? acc[^1] : 0) + s.Qty);
                return acc;
            })));

    [Sample("Group consecutive runs", Description = "Split a sequence into runs of equal neighbours with Aggregate.")]
    public static IEnumerable<string> ConsecutiveRuns() =>
        "aabcccd".Aggregate(new List<(char C, int N)>(), (runs, c) =>
        {
            if (runs.Count > 0 && runs[^1].C == c) runs[^1] = (c, runs[^1].N + 1);
            else runs.Add((c, 1));
            return runs;
        }).Select(r => $"{r.C}{r.N}");

    [Sample("Flatten a tree recursively", Description = "A recursive local function with SelectMany walks nested data.")]
    public static IEnumerable<int> FlattenTree()
    {
        var tree = new Node(1, [new Node(2, [new Node(4, [])]), new Node(3, [])]);
        static IEnumerable<int> Walk(Node n) => n.Children.SelectMany(Walk).Prepend(n.Value);
        return Walk(tree);
    }

    private sealed record Node(int Value, Node[] Children);

    [Sample("Fibonacci via Aggregate state", Description = "Generate a sequence by carrying a tuple of state.")]
    public static IEnumerable<int> Fibonacci() =>
        Enumerable.Range(0, 10).Aggregate(new List<int>(), (f, i) => { f.Add(i < 2 ? i : f[^1] + f[^2]); return f; });

    [Sample("Cartesian product of three", Description = "Three from clauses via SelectMany, filtered by a condition.")]
    public static IEnumerable<string> PythagoreanTriples() =>
        from a in Enumerable.Range(1, 20)
        from b in Enumerable.Range(a, 20 - a + 1)
        from c in Enumerable.Range(b, 20 - b + 1)
        where a * a + b * b == c * c
        select $"{a},{b},{c}";

    [Sample("Power set", Description = "All subsets of a sequence with a bitmask.")]
    public static IEnumerable<string> PowerSet() =>
        Enumerable.Range(0, 1 << 3).Select(mask =>
            "{" + string.Join("", "abc".Where((_, i) => (mask >> i & 1) == 1)) + "}");

    [Sample("Transpose a jagged array", Description = "Rows to columns with Range and Select.")]
    public static IEnumerable<string> Transpose()
    {
        int[][] m = [[1, 2, 3], [4, 5, 6]];
        return Enumerable.Range(0, m[0].Length).Select(c => string.Join(",", m.Select(r => r[c])));
    }

    [Sample("Top N per group with ties", Description = "Keep every row whose quantity equals the group maximum.")]
    public static IEnumerable<string> MaxPerGroupWithTies() =>
        Sales.GroupBy(s => s.Product).OrderBy(g => g.Key).SelectMany(g =>
        {
            var max = g.Max(s => s.Qty);
            return g.Where(s => s.Qty == max).Select(s => $"{g.Key}:{s.Region}:{s.Qty}");
        });

    [Sample("Gaps and islands", Description = "Group consecutive numbers by subtracting the index.")]
    public static IEnumerable<string> Islands() =>
        new[] { 1, 2, 3, 7, 8, 10 }.Select((n, i) => (n, key: n - i)).GroupBy(x => x.key)
            .Select(g => g.Count() == 1 ? $"{g.First().n}" : $"{g.First().n}-{g.Last().n}");

    [Sample("Lazy generator with yield", Description = "Take from an infinite sequence built with yield return.")]
    public static IEnumerable<int> Collatz()
    {
        static IEnumerable<int> Gen(int n)
        {
            while (true) { yield return n; if (n == 1) yield break; n = n % 2 == 0 ? n / 2 : 3 * n + 1; }
        }
        return Gen(6);
    }

    [Sample("Weighted average", Description = "Sum of products divided by sum of weights.")]
    public static double WeightedAverage() =>
        Sales.Sum(s => s.Qty * s.Day) / (double)Sales.Sum(s => s.Qty);

    [Sample("Custom comparer distinct", Description = "DistinctBy on a composite tuple key.")]
    public static IEnumerable<string> DistinctComposite() =>
        Sales.DistinctBy(s => (s.Region, s.Product)).Select(s => $"{s.Region}{s.Product}");
}
