namespace LinqSamples.Samples;

/// <summary>
/// The same queries in query syntax and method syntax. The compiler translates the
/// former into the latter, so pick whichever reads better for the problem at hand.
/// </summary>
[SampleCategory("Query syntax", 11)]
public static class QuerySyntaxSamples
{
    [Sample("where / orderby / select")]
    public static IEnumerable<string> Basic() =>
        from p in Products
        where p.Category == "Electronics"
        orderby p.Price descending
        select p.Name;

    [Sample("Same query in method syntax")]
    public static IEnumerable<string> BasicMethodSyntax() =>
        Products.Where(p => p.Category == "Electronics")
                .OrderByDescending(p => p.Price)
                .Select(p => p.Name);

    [Sample("let", Description = "Introduce an intermediate variable - awkward in method syntax.")]
    public static IEnumerable<string> Let() =>
        from o in Orders
        join p in Products on o.ProductId equals p.Id
        let total = o.Quantity * p.Price
        where total >= 100m
        orderby total descending
        select $"#{o.Id} {total}";

    [Sample("group ... into", Description = "Continue querying after grouping.")]
    public static IEnumerable<string> GroupInto() =>
        from o in Orders
        group o by o.Status into g
        where g.Count() > 1
        orderby g.Key
        select $"{g.Key} ({g.Count()})";

    [Sample("Multiple from clauses", Description = "Equivalent to SelectMany.")]
    public static IEnumerable<string> MultipleFrom() =>
        from c in Customers
        where c.City == "Skopje"
        from o in Orders
        where o.CustomerId == c.Id
        select $"{c.Name} #{o.Id}";
}
