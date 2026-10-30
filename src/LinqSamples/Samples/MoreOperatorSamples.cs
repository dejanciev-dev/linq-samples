namespace LinqSamples.Samples;

/// <summary>
/// More hand-written operators in the style of popular LINQ extension libraries, as C# 14 extension members.
/// Each one is a small, lazy iterator where possible.
/// </summary>
public static class MoreOperators
{
    extension<T>(IEnumerable<T> source)
    {
        /// <summary>Pairs each element with the one <paramref name="offset"/> positions before it.</summary>
        public IEnumerable<(T Current, T Lagged)> Lag(int offset, T defaultValue)
        {
            var buffer = new Queue<T>();
            foreach (var item in source)
            {
                yield return (item, buffer.Count == offset ? buffer.Dequeue() : defaultValue);
                buffer.Enqueue(item);
            }
        }

        /// <summary>Pairs each element with the one <paramref name="offset"/> positions after it.</summary>
        public IEnumerable<(T Current, T Led)> Lead(int offset, T defaultValue)
        {
            var buffer = new Queue<T>();
            foreach (var item in source)
            {
                buffer.Enqueue(item);
                if (buffer.Count > offset)
                {
                    yield return (buffer.Dequeue(), item);
                }
            }

            while (buffer.Count > 0)
            {
                yield return (buffer.Dequeue(), defaultValue);
            }
        }

        /// <summary>Splits into matching and non-matching elements in a single pass.</summary>
        public (List<T> Matching, List<T> Others) Partition(Func<T, bool> predicate)
        {
            List<T> matching = [];
            List<T> others = [];
            foreach (var item in source)
            {
                (predicate(item) ? matching : others).Add(item);
            }

            return (matching, others);
        }

        /// <summary>Starts a new group whenever <paramref name="startsNewGroup"/>(previous, current) is true.</summary>
        public IEnumerable<List<T>> SplitWhen(Func<T, T, bool> startsNewGroup)
        {
            List<T> group = [];
            foreach (var item in source)
            {
                if (group.Count > 0 && startsNewGroup(group[^1], item))
                {
                    yield return group;
                    group = [];
                }

                group.Add(item);
            }

            if (group.Count > 0)
            {
                yield return group;
            }
        }

        /// <summary>Every <paramref name="step"/>-th element, starting with the first.</summary>
        public IEnumerable<T> TakeEvery(int step) =>
            source.Where((_, index) => index % step == 0);

        /// <summary>Collapses runs of equal elements into (value, count) pairs.</summary>
        public IEnumerable<(T Value, int Count)> RunLength()
        {
            var comparer = EqualityComparer<T>.Default;
            var count = 0;
            T current = default!;
            foreach (var item in source)
            {
                if (count > 0 && comparer.Equals(current, item))
                {
                    count++;
                    continue;
                }

                if (count > 0)
                {
                    yield return (current, count);
                }

                current = item;
                count = 1;
            }

            if (count > 0)
            {
                yield return (current, count);
            }
        }

        /// <summary>Appends <paramref name="padding"/> until the sequence has at least <paramref name="width"/> elements.</summary>
        public IEnumerable<T> Pad(int width, T padding)
        {
            var count = 0;
            foreach (var item in source)
            {
                count++;
                yield return item;
            }

            for (; count < width; count++)
            {
                yield return padding;
            }
        }

        /// <summary>The position of the first match, or -1.</summary>
        public int FindIndex(Func<T, bool> predicate) =>
            source.Index().FirstOrDefault(t => predicate(t.Item), (Index: -1, Item: default!)).Index;

        /// <summary>Inserts <paramref name="item"/> at <paramref name="index"/>.</summary>
        public IEnumerable<T> InsertAt(int index, T item)
        {
            var position = 0;
            foreach (var element in source)
            {
                if (position++ == index)
                {
                    yield return item;
                }

                yield return element;
            }

            if (position == index)
            {
                yield return item;
            }
        }

        /// <summary>True when the sequence has at least <paramref name="count"/> elements; stops reading after that many.</summary>
        public bool AtLeast(int count) =>
            source.Take(count).Count() == count;

        /// <summary>Select and filter in one step: keep only the results the chooser marks as wanted.</summary>
        public IEnumerable<TResult> Choose<TResult>(Func<T, (bool Keep, TResult Value)> chooser) =>
            source.Select(chooser).Where(r => r.Keep).Select(r => r.Value);

        /// <summary>Enumerates the source at most once; later enumerations replay the cached elements.</summary>
        public IEnumerable<T> Memoize()
        {
            List<T> cache = [];
            IEnumerator<T>? enumerator = null;
            var finished = false;
            return Replay();

            IEnumerable<T> Replay()
            {
                for (var i = 0; ; i++)
                {
                    if (i < cache.Count)
                    {
                        yield return cache[i];
                        continue;
                    }

                    if (finished)
                    {
                        yield break;
                    }

                    enumerator ??= source.GetEnumerator();
                    if (!enumerator.MoveNext())
                    {
                        finished = true;
                        enumerator.Dispose();
                        yield break;
                    }

                    cache.Add(enumerator.Current);
                    yield return enumerator.Current;
                }
            }
        }

        /// <summary>Like Zip, but continues to the end of the longer sequence using <paramref name="filler"/>.</summary>
        public IEnumerable<(T First, T Second)> ZipLongest(IEnumerable<T> other, T filler)
        {
            using var first = source.GetEnumerator();
            using var second = other.GetEnumerator();
            while (true)
            {
                var hasFirst = first.MoveNext();
                var hasSecond = second.MoveNext();
                if (!hasFirst && !hasSecond)
                {
                    yield break;
                }

                yield return (hasFirst ? first.Current : filler, hasSecond ? second.Current : filler);
            }
        }

        /// <summary>Every element that shares the maximum key - MaxBy returns only the first.</summary>
        public IEnumerable<T> MaxByAll<TKey>(Func<T, TKey> keySelector)
        {
            var items = source.ToList();
            if (items.Count == 0)
            {
                return [];
            }

            var max = items.Max(keySelector);
            return items.Where(item => Comparer<TKey>.Default.Compare(keySelector(item), max) == 0);
        }

        /// <summary>Repeats the sequence forever (or yields nothing if it is empty).</summary>
        public IEnumerable<T> Cycle()
        {
            while (true)
            {
                var any = false;
                foreach (var item in source)
                {
                    any = true;
                    yield return item;
                }

                if (!any)
                {
                    yield break;
                }
            }
        }
    }

    extension<T>(IEnumerable<T> source) where T : IComparable<T>
    {
        /// <summary>Minimum and maximum in a single pass.</summary>
        public (T Min, T Max) MinMax()
        {
            using var e = source.GetEnumerator();
            if (!e.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements.");
            }

            var (min, max) = (e.Current, e.Current);
            while (e.MoveNext())
            {
                if (e.Current.CompareTo(min) < 0)
                {
                    min = e.Current;
                }

                if (e.Current.CompareTo(max) > 0)
                {
                    max = e.Current;
                }
            }

            return (min, max);
        }
    }
}

[SampleCategory("More custom operators", 27)]
public static class MoreOperatorSamples
{
    [Sample("Lag", Description = "Look back: each element with its predecessor (SQL's LAG).")]
    public static IEnumerable<string> Lag() =>
        Numbers.Take(5).Lag(1, 0).Select(t => $"{t.Current} (previous {t.Lagged})");

    [Sample("Lead", Description = "Look ahead: each element with its successor (SQL's LEAD).")]
    public static IEnumerable<string> Lead() =>
        Numbers.Take(5).Lead(1, 0).Select(t => $"{t.Current} (next {t.Led})");

    [Sample("Partition", Description = "Both halves of a Where in one pass.")]
    public static IEnumerable<string> Partition()
    {
        var (even, odd) = Numbers.Partition(n => n % 2 == 0);
        return [$"Even: {string.Join(", ", even)}", $"Odd:  {string.Join(", ", odd)}"];
    }

    [Sample("SplitWhen", Description = "Group consecutive runs - here compress numbers into ranges.")]
    public static string SplitWhen()
    {
        int[] pages = [1, 2, 3, 7, 8, 10];
        return string.Join(", ", pages.SplitWhen((previous, current) => current != previous + 1)
                                      .Select(g => g.Count == 1 ? $"{g[0]}" : $"{g[0]}-{g[^1]}"));
    }

    [Sample("TakeEvery", Description = "Sample every n-th element.")]
    public static IEnumerable<int> TakeEvery() =>
        Enumerable.Range(1, 10).TakeEvery(3);

    [Sample("MinMax", Description = "Both extremes in one pass instead of two.")]
    public static string MinMax()
    {
        var (min, max) = Numbers.MinMax();
        return $"Min: {min}, Max: {max}";
    }

    [Sample("Run-length encoding", Description = "Encode runs as value+count, then decode with Repeat.")]
    public static IEnumerable<string> RunLength()
    {
        var runs = "aaabccdddd".RunLength().ToList();
        return
        [
            $"Encoded: {string.Concat(runs.Select(r => $"{r.Value}{r.Count}"))}",
            $"Decoded: {string.Concat(runs.SelectMany(r => Enumerable.Repeat(r.Value, r.Count)))}",
        ];
    }

    [Sample("Pad", Description = "Fill up to a fixed width - handy for tables and fixed-size batches.")]
    public static IEnumerable<int> Pad() =>
        new[] { 1, 2 }.Pad(4, 0);

    [Sample("FindIndex", Description = "The position of the first match, -1 when there is none.")]
    public static IEnumerable<string> FindIndex() =>
    [
        $"First > 8 at index:   {Numbers.FindIndex(n => n > 8)}",
        $"First > 100 at index: {Numbers.FindIndex(n => n > 100)}",
    ];

    [Sample("InsertAt", Description = "Insert an element into a lazy sequence.")]
    public static IEnumerable<int> InsertAt() =>
        new[] { 1, 2, 4 }.InsertAt(2, 3);

    [Sample("AtLeast", Description = "Answer \"at least n?\" without counting everything - it even works on infinite sequences.")]
    public static IEnumerable<string> AtLeast() =>
    [
        $"Numbers has at least 11:   {Numbers.AtLeast(11)}",
        $"Infinite has at least 5:   {IEnumerable<int>.Iterate(1, n => n + 1).AtLeast(5)}",
    ];

    [Sample("Choose", Description = "Parse and filter in one step - keep only the inputs that parse.")]
    public static IEnumerable<int> Choose()
    {
        string[] inputs = ["1", "x", "3", "4.5"];
        return inputs.Choose(s => (int.TryParse(s, out var n), n));
    }

    [Sample("Memoize", Description = "Cache a lazy query so enumerating it twice does the work once.")]
    public static IEnumerable<string> Memoize()
    {
        var plain = 0;
        var query = Numbers.Select(n => { plain++; return n; });
        _ = query.Count();
        _ = query.Sum();

        var memoized = 0;
        var cached = Numbers.Select(n => { memoized++; return n; }).Memoize();
        _ = cached.Count();
        _ = cached.Sum();

        return [$"Plain query: {plain} evaluations", $"Memoized:    {memoized} evaluations"];
    }

    [Sample("ZipLongest", Description = "Zip that pads the shorter side instead of stopping.")]
    public static IEnumerable<string> ZipLongest() =>
        new[] { 1, 2, 3 }.ZipLongest([10], 0).Select(t => $"({t.First}, {t.Second})");

    [Sample("MaxByAll", Description = "All elements tied for the maximum - MaxBy keeps only the first.")]
    public static IEnumerable<string> MaxByAll() =>
    [
        $"MaxBy:    #{Orders.MaxBy(o => o.Date.Month)!.Id}",
        $"MaxByAll: {string.Join(", ", Orders.MaxByAll(o => o.Date.Month).Select(o => $"#{o.Id}"))}",
    ];

    [Sample("Cycle", Description = "Repeat a sequence endlessly - take what you need.")]
    public static string Cycle() =>
        string.Concat(new[] { "R", "G", "B" }.Cycle().Take(7));
}
