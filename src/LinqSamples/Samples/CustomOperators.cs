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

        /// <summary>Yields overlapping windows of <paramref name="size"/> consecutive elements.</summary>
        public IEnumerable<T[]> Window(int size)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(size);
            var window = new Queue<T>(size);
            foreach (var item in source)
            {
                window.Enqueue(item);
                if (window.Count > size)
                {
                    window.Dequeue();
                }

                if (window.Count == size)
                {
                    yield return [.. window];
                }
            }
        }

        /// <summary>Extension property: true when the sequence has no elements.</summary>
        public bool IsEmpty => !source.Any();
    }

    extension<T>(IEnumerable<T>)
    {
        /// <summary>Static extension member: an infinite sequence seed, next(seed), next(next(seed)), ...</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000", Justification = "Static extension members are meant to be called on the extended type.")]
        public static IEnumerable<T> Iterate(T seed, Func<T, T> next)
        {
            for (var current = seed; ; current = next(current))
            {
                yield return current;
            }
        }
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

    [Sample("Window", Description = "Sliding windows make a moving average a one-liner.")]
    public static IEnumerable<double> MovingAverage() =>
        Numbers.Window(3).Select(w => Math.Round(w.Average(), 2));

    [Sample("Static extension member", Description = "C# 14 can also add static members to a type - here a factory on IEnumerable<T>.")]
    public static IEnumerable<int> Iterate() =>
        IEnumerable<int>.Iterate(1, n => n * 2).Take(8);

    [Sample("Extension property", Description = "C# 14 lets you add properties, not only methods.")]
    public static IEnumerable<string> ExtensionProperty() =>
    [
        $"Orders empty?           {Orders.IsEmpty}",
        $"Orders over 10000 empty? {Orders.Where(o => o.Quantity > 10_000).IsEmpty}",
    ];
}
