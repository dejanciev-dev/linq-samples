using System.Globalization;

namespace LinqSamples.Samples;

/// <summary>Common LINQ mistakes, each shown next to the fix. Most compile fine and fail quietly.</summary>
[SampleCategory("Pitfalls (wrong vs right)", 30)]
public static class PitfallSamples
{
    [Sample("Count() > 0 vs Any()", Description = "Count walks the whole sequence; Any stops at the first element (analyzer rule CA1827 flags this).")]
    public static IEnumerable<string> CountVsAny()
    {
        // A counting Where: a Select over an array would let .NET answer Any() without running the selector at all.
        var visited = 0;
        var query = Numbers.Where(_ => { visited++; return true; });

#pragma warning disable CA1827 // the pitfall being demonstrated
        _ = query.Count() > 0;
#pragma warning restore CA1827
        var byCount = visited;

        visited = 0;
        _ = query.Any();
        return [$"Count() > 0 visited {byCount} items", $"Any()       visited {visited} item"];
    }

    [Sample("First hides duplicates", Description = "First silently picks one; Single fails loudly when the data breaks your assumption.")]
    public static IEnumerable<string> FirstVsSingle()
    {
        string single;
        try
        {
            single = Customers.Single(c => c.City == "Berlin").Name;
        }
        catch (InvalidOperationException)
        {
            single = "InvalidOperationException - more than one match";
        }

        return [$"First:  {Customers.First(c => c.City == "Berlin").Name}", $"Single: {single}"];
    }

    [Sample("OrderBy().OrderBy()", Description = "A second OrderBy replaces the first sort; use ThenBy for a secondary key.")]
    public static IEnumerable<string> OrderByTwice() =>
    [
        $"OrderBy.OrderBy: {string.Join(", ", Products.OrderBy(p => p.Category).OrderBy(p => p.Price).Take(3).Select(p => p.Name))}",
        $"OrderBy.ThenBy:  {string.Join(", ", Products.OrderBy(p => p.Category).ThenBy(p => p.Price).Take(3).Select(p => p.Name))}",
    ];

    [Sample("Select for side effects", Description = "A query that is never enumerated never runs - use foreach for actions.")]
    public static IEnumerable<string> SelectForSideEffects()
    {
        List<int> processed = [];
        _ = Numbers.Select(n => { processed.Add(n); return n; });
        var bySelect = processed.Count;

        processed.Clear();
        foreach (var n in Numbers)
        {
            processed.Add(n);
        }

        return [$"Unenumerated Select processed: {bySelect}", $"foreach processed:            {processed.Count}"];
    }

    [Sample("Closures over a for variable", Description = "All lambdas share the one loop variable of a for loop (foreach gives each iteration its own).")]
    public static IEnumerable<string> ClosuresInForLoop()
    {
        List<Func<int>> shared = [];
        for (var i = 0; i < 3; i++)
        {
            shared.Add(() => i);
        }

        List<Func<int>> copied = [];
        for (var i = 0; i < 3; i++)
        {
            var copy = i;
            copied.Add(() => copy);
        }

        return
        [
            $"Captured loop variable: {string.Join(", ", shared.Select(f => f()))}",
            $"Copied to a local:      {string.Join(", ", copied.Select(f => f()))}",
        ];
    }

    [Sample("Contains on a list in a loop", Description = "A List search is linear; a HashSet looks up in constant time.")]
    public static IEnumerable<string> ContainsInLoop()
    {
        var ids = Enumerable.Range(1, 1000).ToList();

        var listComparer = new CountingComparer();
        _ = ids.Count(id => ids.Contains(id, listComparer));

        var setComparer = new CountingComparer();
        var set = new HashSet<int>(ids, setComparer);
        setComparer.Comparisons = 0;
        _ = ids.Count(set.Contains);

        return [$"List.Contains:    {listComparer.Comparisons} comparisons", $"HashSet.Contains: {setComparer.Comparisons} comparisons"];
    }

    [Sample("Distinct on classes", Description = "Classes compare by reference, so equal-looking objects stay; records compare by value.")]
    public static IEnumerable<string> DistinctOnClasses()
    {
        PointClass[] classes = [new(1, 2), new(1, 2)];
        PointRecord[] records = [new(1, 2), new(1, 2)];
        return
        [
            $"Class:      {classes.Distinct().Count()} distinct",
            $"Record:     {records.Distinct().Count()} distinct",
            $"DistinctBy: {classes.DistinctBy(p => (p.X, p.Y)).Count()} distinct",
        ];
    }

    [Sample("Modifying while enumerating", Description = "Removing from a list inside its own query throws; use RemoveAll or materialize first.")]
    public static IEnumerable<string> ModifyWhileEnumerating()
    {
        var list = Numbers.ToList();
        string attempt;
        try
        {
            foreach (var n in list.Where(n => n % 2 == 0))
            {
                list.Remove(n);
            }

            attempt = "no exception";
        }
        catch (InvalidOperationException)
        {
            attempt = "InvalidOperationException";
        }

        var fixedList = Numbers.ToList();
        fixedList.RemoveAll(n => n % 2 == 0);
        return [$"Remove inside foreach: {attempt}", $"RemoveAll:             {fixedList.Count} left"];
    }

    [Sample("FirstOrDefault on value types", Description = "0 could be a real value or 'not found' - make the absence explicit.")]
    public static IEnumerable<string> FirstOrDefaultValueTypes() =>
    [
        $"FirstOrDefault:            {Numbers.FirstOrDefault(n => n > 100)}",
        $"Cast to int? first:        {Numbers.Select(n => (int?)n).FirstOrDefault(n => n > 100)?.ToString() ?? "null"}",
        $"FirstOrDefault(pred, -1):  {Numbers.FirstOrDefault(n => n > 100, -1)}",
    ];

    [Sample("Sum overflows", Description = "Enumerable.Sum is checked: int totals can throw. Widen to long first.")]
    public static IEnumerable<string> SumOverflow()
    {
        int[] large = [int.MaxValue, 1];
        string asInt;
        try
        {
            asInt = large.Sum().ToString(CultureInfo.InvariantCulture);
        }
        catch (OverflowException)
        {
            asInt = "OverflowException";
        }

        return [$"Sum():               {asInt}", $"Sum(n => (long)n):   {large.Sum(n => (long)n)}"];
    }

    [Sample("Integer division", Description = "Sum() / Count() on ints truncates; Average() returns a double.")]
    public static IEnumerable<string> IntegerDivision() =>
    [
        $"Sum() / Length: {Numbers.Sum() / Numbers.Length}",
        FormattableString.Invariant($"Average():      {Numbers.Average()}"),
    ];

    [Sample("Reverse() is not in place", Description = "LINQ returns a new sequence and leaves the array alone; Array.Reverse changes the array (CA1806 flags the discarded result).")]
    public static IEnumerable<string> ReverseIsNotInPlace()
    {
        int[] numbers = [1, 2, 3];
#pragma warning disable CA1806 // the pitfall being demonstrated: the reversed sequence is thrown away
        numbers.Reverse();
#pragma warning restore CA1806
        var afterLinq = string.Join(", ", numbers);

        Array.Reverse(numbers);
        return [$"numbers.Reverse():      {afterLinq}  (result discarded)", $"Array.Reverse(numbers): {string.Join(", ", numbers)}"];
    }

    [Sample("Filter before you project", Description = "Where after an expensive Select still pays for every element.")]
    public static IEnumerable<string> FilterBeforeProject()
    {
        var projections = 0;
        string Describe(Product p)
        {
            projections++;
            return $"{p.Name} ({p.Price})";
        }

        _ = Products.Select(p => (p.Price, Text: Describe(p))).Where(x => x.Price > 200m).ToList();
        var selectFirst = projections;

        projections = 0;
        _ = Products.Where(p => p.Price > 200m).Select(Describe).ToList();
        return [$"Select then Where: {selectFirst} projections", $"Where then Select: {projections} projections"];
    }

    [Sample("Floating-point keys", Description = "0.1 + 0.2 is not 0.3, so Distinct and GroupBy see two keys - round first.")]
    public static IEnumerable<string> FloatingPointKeys()
    {
        double[] values = [0.1 + 0.2, 0.3];
        return
        [
            $"Distinct:                {values.Distinct().Count()} values",
            $"Distinct after rounding: {values.Select(v => Math.Round(v, 10)).Distinct().Count()} value",
        ];
    }

    [Sample("Lazy query over a disposed resource", Description = "The query runs after the using block closed the reader; materialize inside it.")]
    public static IEnumerable<string> DisposedResource()
    {
        static IEnumerable<string> ReadLines(TextReader reader)
        {
            while (reader.ReadLine() is { } line)
            {
                yield return line;
            }
        }

        IEnumerable<string> lazy;
        using (var reader = new StringReader("alpha\nbeta"))
        {
            lazy = ReadLines(reader).Where(l => l.Length > 0);
        }

        string attempt;
        try
        {
            attempt = string.Join(", ", lazy);
        }
        catch (ObjectDisposedException)
        {
            attempt = "ObjectDisposedException";
        }

        List<string> materialized;
        using (var reader = new StringReader("alpha\nbeta"))
        {
            materialized = ReadLines(reader).Where(l => l.Length > 0).ToList();
        }

        return [$"Enumerated after Dispose: {attempt}", $"ToList() inside using:    {string.Join(", ", materialized)}"];
    }

    private sealed class CountingComparer : IEqualityComparer<int>
    {
        public int Comparisons { get; set; }

        public bool Equals(int x, int y)
        {
            Comparisons++;
            return x == y;
        }

        public int GetHashCode(int obj) => obj;
    }

    private sealed class PointClass(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    private sealed record PointRecord(int X, int Y);
}
