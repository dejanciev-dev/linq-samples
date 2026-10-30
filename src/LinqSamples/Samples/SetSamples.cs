namespace LinqSamples.Samples;

[SampleCategory("Set operations", 7)]
public static class SetSamples
{
    private static readonly int[] First = [1, 2, 3, 4, 5];
    private static readonly int[] Second = [4, 5, 6, 7];

    [Sample("Distinct")]
    public static IEnumerable<int> Distinct() => Numbers.Distinct();

    [Sample("DistinctBy (.NET 6+)", Description = "First customer from each city.")]
    public static IEnumerable<string> DistinctBy() =>
        Customers.DistinctBy(c => c.City).Select(c => $"{c.City}: {c.Name}");

    [Sample("Union")]
    public static IEnumerable<int> Union() => First.Union(Second);

    [Sample("UnionBy (.NET 6+)", Description = "Merge updates into a list by key: the first sequence wins on duplicates.")]
    public static IEnumerable<string> UnionBy()
    {
        Product[] updates = [new(2, "Mouse", "Electronics", 20m), new(8, "Webcam", "Electronics", 80m)];
        return updates.UnionBy(Products, p => p.Id)
                      .OrderBy(p => p.Id)
                      .Select(p => $"{p.Id} {p.Name}: {p.Price}");
    }

    [Sample("Intersect")]
    public static IEnumerable<int> Intersect() => First.Intersect(Second);

    [Sample("Except")]
    public static IEnumerable<int> Except() => First.Except(Second);

    [Sample("ExceptBy (.NET 6+)", Description = "Products that were never ordered.")]
    public static IEnumerable<string> ExceptBy() =>
        Products.ExceptBy(Orders.Select(o => o.ProductId), p => p.Id).Select(p => p.Name);

    [Sample("IntersectBy (.NET 6+)", Description = "Customers who placed at least one order.")]
    public static IEnumerable<string> IntersectBy() =>
        Customers.IntersectBy(Orders.Select(o => o.CustomerId), c => c.Id).Select(c => c.Name);

    [Sample("Distinct with a comparer", Description = "Case-insensitive de-duplication of first letters.")]
    public static IEnumerable<string> DistinctWithComparer() =>
        Words.Select(w => w[..1]).Distinct(StringComparer.OrdinalIgnoreCase);

    [Sample("Set operators remove duplicates", Description = "Except, Union and Intersect also de-duplicate the first sequence.")]
    public static IEnumerable<int> SetOperatorsDeduplicate()
    {
        int[] source = [1, 1, 2, 2, 3];
        return source.Except([3]);
    }

    [Sample("SequenceEqual")]
    public static bool SequenceEqual() => First.Take(3).SequenceEqual([1, 2, 3]);
}
