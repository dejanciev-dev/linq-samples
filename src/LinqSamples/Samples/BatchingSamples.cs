namespace LinqSamples.Samples;

/// <summary>Batching, pagination, round-robin and interleaving with LINQ.</summary>
[SampleCategory("Batching & pagination", 40)]
public static class BatchingSamples
{
    private static readonly int[] Items = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    [Sample("Chunk into batches", Description = "Chunk splits a sequence into arrays of a fixed size.")]
    public static IEnumerable<string> Batches() => Items.Chunk(4).Select(c => string.Join(",", c));

    [Sample("Page of results", Description = "Skip((page-1)*size).Take(size).")]
    public static IEnumerable<int> Page() => Items.Skip((3 - 1) * 4).Take(4);

    [Sample("Total page count", Description = "Ceiling division of count by page size.")]
    public static int PageCount() => (Items.Length + 3) / 4;

    [Sample("Page numbers with items", Description = "Chunk then Index gives (page, items) pairs.")]
    public static IEnumerable<string> NumberedPages() =>
        Items.Chunk(3).Index().Select(p => $"{p.Index + 1}:{string.Join(",", p.Item)}");

    [Sample("Round-robin split", Description = "Group by index modulo N to deal items into N buckets.")]
    public static IEnumerable<string> RoundRobin() =>
        Items.Select((v, i) => (v, i)).GroupBy(x => x.i % 3).Select(g => string.Join(",", g.Select(x => x.v)));

    [Sample("Interleave two lists", Description = "Zip into pairs, then flatten with SelectMany.")]
    public static IEnumerable<int> Interleave() =>
        new[] { 1, 3, 5 }.Zip(new[] { 2, 4, 6 }, (a, b) => new[] { a, b }).SelectMany(p => p);

    [Sample("Every nth item", Description = "Where with an index predicate.")]
    public static IEnumerable<int> EveryThird() => Items.Where((_, i) => i % 3 == 0);

    [Sample("Take while sum under limit", Description = "TakeWhile with an index predicate and prefix sum; no captured mutable state, so it is safe to enumerate twice.")]
    public static IEnumerable<int> UnderBudget() =>
        Items.TakeWhile((_, i) => Items.Take(i + 1).Sum() <= 20);

    [Sample("Last (partial) page", Description = "Chunk then Last returns the short final batch.")]
    public static string LastPage() => string.Join(",", Items.Chunk(4).Last());

    [Sample("Pad the last batch", Description = "Concat Repeat fills the short final batch up to the full size.")]
    public static IEnumerable<string> PaddedBatches() =>
        Items.Chunk(4).Select(c => string.Join(",", c.Concat(Enumerable.Repeat(0, 4 - c.Length))));

    [Sample("Sum per batch", Description = "Chunk then Select aggregates each batch.")]
    public static IEnumerable<int> BatchSums() => Items.Chunk(3).Select(c => c.Sum());

    [Sample("Split into N contiguous parts", Description = "Group by index * N / count to keep neighbours together.")]
    public static IEnumerable<string> ContiguousParts() =>
        Items.Select((v, i) => (v, i)).GroupBy(x => x.i * 3 / Items.Length)
             .Select(g => string.Join(",", g.Select(x => x.v)));

    [Sample("Every other batch", Description = "Chunk, filter batches by index, flatten with SelectMany.")]
    public static IEnumerable<int> AlternateBatches() =>
        Items.Chunk(2).Where((_, i) => i % 2 == 0).SelectMany(c => c);

    [Sample("Skip last and take last", Description = "SkipLast and TakeLast trim from the end.")]
    public static string Ends() => $"{string.Join(",", Items.SkipLast(7))}|{string.Join(",", Items.TakeLast(2))}";

    [Sample("Batches by total weight", Description = "Aggregate starts a new batch when adding would exceed the cap.")]
    public static IEnumerable<string> WeightBatches() =>
        Items.Aggregate(new List<List<int>> { new() }, (acc, n) =>
        {
            if (acc[^1].Sum() + n > 12) acc.Add([]);
            acc[^1].Add(n);
            return acc;
        }).Select(b => string.Join(",", b));

    [Sample("Page with position", Description = "Chunk, Skip to the page, and format its place in the whole.")]
    public static string PageInfo() => $"page 2/{Items.Chunk(4).Count()}: {string.Join(",", Items.Chunk(4).Skip(1).First())}";

    [Sample("Batch start offsets", Description = "Chunk with Select index gives each batch's start offset and size.")]
    public static IEnumerable<string> BatchOffsets() => Items.Chunk(4).Select((c, i) => $"{i * 4}:{c.Length}");

    [Sample("Pages newest first", Description = "Reverse the batches, not the items inside them.")]
    public static IEnumerable<string> ReversedPages() => Items.Chunk(4).Reverse().Select(c => string.Join(",", c));

    [Sample("Take a range", Description = "Take(Range) slices by index range, like array ranges.")]
    public static IEnumerable<int> TakeRange() => Items.Take(2..5);

    [Sample("Sliding batches", Description = "Overlapping windows via Skip/Take.")]
    public static IEnumerable<string> Overlapping() =>
        Enumerable.Range(0, Items.Length - 2).Select(i => string.Join(",", Items.Skip(i).Take(3))).Take(3);

    [Sample("Batch count per size", Description = "Chunk then Count for several batch sizes.")]
    public static string BatchCounts() => string.Join("|", new[] { 2, 3, 5 }.Select(n => $"{n}:{Items.Chunk(n).Count()}"));

    [Sample("First item of each batch", Description = "Chunk then Select the first element.")]
    public static IEnumerable<int> BatchHeads() => Items.Chunk(4).Select(c => c[0]);

    [Sample("Zip batches with labels", Description = "Zip batches with a letter sequence.")]
    public static IEnumerable<string> LabelledBatches() =>
        Items.Chunk(5).Zip("AB", (c, l) => $"{l}:{string.Join(",", c)}");

    [Sample("Page number per item", Description = "Integer division of the index gives each item its page.")]
    public static IEnumerable<string> PagePerItem() => Items.Select((v, i) => $"{i / 4 + 1}:{v}").Take(6);

    [Sample("Heaviest batch", Description = "Chunk then Max over each batch sum.")]
    public static int HeaviestBatch() => Items.Chunk(4).Max(c => c.Sum());

    [Sample("Skip pages, flatten the rest", Description = "Skip and Take on batches, then SelectMany back to items.")]
    public static IEnumerable<int> MiddlePages() => Items.Chunk(3).Skip(1).Take(2).SelectMany(c => c);
}
