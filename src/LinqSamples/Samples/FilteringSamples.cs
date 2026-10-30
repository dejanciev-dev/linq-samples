namespace LinqSamples.Samples;

[SampleCategory("Filtering", 1)]
public static class FilteringSamples
{
    [Sample("Where", Description = "Keep only elements that match a predicate.")]
    public static IEnumerable<Product> Where() =>
        Products.Where(p => p.Price > 200m);

    [Sample("Where with index", Description = "The predicate can also receive the element's position.")]
    public static IEnumerable<int> WhereWithIndex() =>
        Numbers.Where((n, index) => index % 2 == 0);

    [Sample("Multiple conditions", Description = "Combine conditions with && / || or chain several Where calls.")]
    public static IEnumerable<Order> MultipleConditions() =>
        Orders.Where(o => o.Status == "Completed")
              .Where(o => o.Date.Month >= 2);

    [Sample("OfType", Description = "Filter a mixed sequence by runtime type.")]
    public static IEnumerable<string> OfType()
    {
        object?[] mixed = [1, "one", 2.0, "two", null, 'c', "three"];
        return mixed.OfType<string>();
    }

    [Sample("Filter nulls", Description = "OfType<T>() on a nullable sequence drops nulls and fixes the static type.")]
    public static IEnumerable<int> FilterNulls()
    {
        int?[] values = [1, null, 3, null, 5];
        return values.OfType<int>();
    }

    [Sample("Filter by a list of values", Description = "The LINQ version of SQL's IN (...): Contains on a local collection.")]
    public static IEnumerable<string> FilterByList()
    {
        HashSet<string> cities = ["Berlin", "Vienna"];
        return Customers.Where(c => cities.Contains(c.City)).Select(c => c.Name);
    }

    [Sample("Where with pattern matching", Description = "Property patterns express several conditions on one object.")]
    public static IEnumerable<string> PatternMatching() =>
        Products.Where(p => p is { Category: "Furniture", Price: < 200m }).Select(p => p.Name);

    [Sample("Filter by a date range", Description = "Half-open ranges (>= start, < end) avoid off-by-one errors at the boundary.")]
    public static IEnumerable<int> DateRange()
    {
        DateOnly start = new(2026, 2, 1);
        DateOnly end = new(2026, 3, 1);
        return Orders.Where(o => o.Date >= start && o.Date < end).Select(o => o.Id);
    }
}
