namespace LinqSamples.Samples;

/// <summary>
/// Custom LINQ operators written with C# 14 extension members (<c>extension(...)</c> blocks).
/// </summary>
public static class CustomOperators
{
    extension<T>(IEnumerable<T> source)
    {
        /// <summary>Keeps elements only when <paramref name="condition"/> is true.</summary>
        public IEnumerable<T> WhereIf(bool condition, Func<T, bool> predicate) =>
            condition ? source.Where(predicate) : source;

        /// <summary>Yields each element paired with its predecessor.</summary>
        public IEnumerable<(T Previous, T Current)> Pairwise()
        {
            using var e = source.GetEnumerator();
            if (!e.MoveNext())
            {
                yield break;
            }

            var previous = e.Current;
            while (e.MoveNext())
            {
                yield return (previous, e.Current);
                previous = e.Current;
            }
        }

        /// <summary>Like Aggregate, but yields every intermediate accumulator.</summary>
        public IEnumerable<TAccumulate> Scan<TAccumulate>(TAccumulate seed, Func<TAccumulate, T, TAccumulate> func)
        {
            var acc = seed;
            foreach (var item in source)
            {
                acc = func(acc, item);
                yield return acc;
            }
        }

        /// <summary>Extension property: true when the sequence has no elements.</summary>
        public bool IsEmpty => !source.Any();
    }
}

[SampleCategory("Custom operators (C# 14)", 13)]
public static class CustomOperatorSamples
{
    [Sample("WhereIf", Description = "Apply a filter only when a condition holds - handy for optional search filters.")]
    public static IEnumerable<string> WhereIf()
    {
        string? cityFilter = "Berlin";
        string? nameFilter = null;
        return Customers.WhereIf(cityFilter is not null, c => c.City == cityFilter)
                        .WhereIf(nameFilter is not null, c => c.Name == nameFilter)
                        .Select(c => c.Name);
    }

    [Sample("Pairwise", Description = "Compute deltas between consecutive elements.")]
    public static IEnumerable<int> Pairwise() =>
        Numbers.Pairwise().Select(p => p.Current - p.Previous);

    [Sample("Scan", Description = "Running total as a reusable, lazy operator.")]
    public static IEnumerable<int> Scan() =>
        Numbers.Scan(0, (acc, n) => acc + n);

    [Sample("Extension property", Description = "C# 14 lets you add properties, not only methods.")]
    public static IEnumerable<string> ExtensionProperty() =>
    [
        $"Orders empty?           {Orders.IsEmpty}",
        $"Orders over 10000 empty? {Orders.Where(o => o.Quantity > 10_000).IsEmpty}",
    ];
}
