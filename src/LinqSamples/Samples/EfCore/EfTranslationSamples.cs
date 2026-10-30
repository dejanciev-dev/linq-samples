using Microsoft.EntityFrameworkCore;
using static LinqSamples.Samples.EfCore.EfCoreSamples;

namespace LinqSamples.Samples.EfCore;

/// <summary>How common LINQ shapes become SQL. Each sample prints the generated SQL and the result.</summary>
[SampleCategory("EF Core: query translation", 25)]
public static class EfTranslationSamples
{
    [Sample("String methods", Description = "StartsWith and Contains become LIKE / instr.")]
    public static IEnumerable<string> StringMethods()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Where(c => c.Name.StartsWith("Al") || c.City.Contains("enn")).Select(c => c.Name));
    }

    [Sample("EF.Functions.Like", Description = "Use SQL LIKE patterns directly.")]
    public static IEnumerable<string> Like()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Products.Where(p => EF.Functions.Like(p.Name, "%o%")).OrderBy(p => p.Id).Select(p => p.Name));
    }

    [Sample("String functions in projections", Description = "Substring and ToUpper run in the database.")]
    public static IEnumerable<string> StringProjection()
    {
        using var db = ShopDbContext.CreateSeeded();
        // Translated to SQL upper(); the culture-aware overloads the analyzer suggests are not translatable.
#pragma warning disable CA1304, CA1311
        return SqlAndResult(db.Customers.OrderBy(c => c.Id).Select(c => c.Name.Substring(0, 3).ToUpper()));
#pragma warning restore CA1304, CA1311
    }

    [Sample("Date parts", Description = "DateOnly.Month is translated, so grouping by month happens in SQL.")]
    public static IEnumerable<string> DateParts()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Orders.GroupBy(o => o.Date.Month)
                                     .Select(g => new { Month = g.Key, Orders = g.Count() })
                                     .OrderBy(x => x.Month));
    }

    [Sample("Conditional to CASE", Description = "The ?: operator becomes CASE WHEN.")]
    public static IEnumerable<string> ConditionalToCase()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Orders.OrderBy(o => o.Id)
                                     .Take(5)
                                     .Select(o => new { o.Id, Size = o.Quantity >= 5 ? "bulk" : "single" }));
    }

    [Sample("Null-safe aggregates", Description = "Max over an empty set is NULL in SQL - cast to int? and use ??.")]
    public static IEnumerable<string> NullSafeAggregates()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.OrderBy(c => c.Id)
                                        .Select(c => new { c.Name, Largest = c.Orders.Max(o => (int?)o.Quantity) ?? 0 }));
    }

    [Sample("Any becomes EXISTS", Description = "A predicate on a navigation collection.")]
    public static IEnumerable<string> AnyToExists()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Where(c => c.Orders.Any(o => o.Status == "Cancelled")).Select(c => c.Name));
    }

    [Sample("All becomes NOT EXISTS", Description = "Careful: All is true for customers with no orders at all.")]
    public static IEnumerable<string> AllToNotExists()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Where(c => c.Orders.All(o => o.Status == "Completed")).Select(c => c.Name));
    }

    [Sample("Count distinct", Description = "Distinct().Count() runs as one query that returns a single number.")]
    public static IEnumerable<string> CountDistinct()
    {
        using var db = ShopDbContext.CreateSeeded();
        return LoggedSqlAndResult(db, () => db.Orders.Select(o => o.CustomerId).Distinct().Count());
    }

    [Sample("SelectMany over a navigation", Description = "Flatten related rows with a JOIN.")]
    public static IEnumerable<string> SelectManyNavigation()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Where(c => c.City == "Skopje")
                                        .SelectMany(c => c.Orders)
                                        .OrderBy(o => o.Id)
                                        .Select(o => o.Id));
    }

    [Sample("Union", Description = "Combine two queries with SQL UNION (duplicates removed).")]
    public static IEnumerable<string> Union()
    {
        using var db = ShopDbContext.CreateSeeded();
        var skopje = db.Customers.Where(c => c.City == "Skopje").Select(c => c.Name);
        var bulkBuyers = db.Orders.Where(o => o.Quantity > 5).Select(o => o.Customer.Name);
        return SqlAndResult(skopje.Union(bulkBuyers).OrderBy(name => name));
    }

    [Sample("Concat", Description = "UNION ALL keeps duplicates.")]
    public static IEnumerable<string> Concat()
    {
        using var db = ShopDbContext.CreateSeeded();
        var skopje = db.Customers.Where(c => c.City == "Skopje").Select(c => c.Name);
        var bulkBuyers = db.Orders.Where(o => o.Quantity > 5).Select(o => o.Customer.Name);
        return SqlAndResult(skopje.Concat(bulkBuyers).OrderBy(name => name));
    }

    [Sample("Except", Description = "SQL EXCEPT - customers who never ordered.")]
    public static IEnumerable<string> Except()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Select(c => c.Name).Except(db.Orders.Select(o => o.Customer.Name)));
    }

    [Sample("Top 1 per group", Description = "The most expensive product of each category, in a single query.")]
    public static IEnumerable<string> TopPerGroup()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Products.GroupBy(p => p.Category)
                                       .Select(g => new { Category = g.Key, Top = g.OrderByDescending(p => p.Price).Select(p => p.Name).First() })
                                       .OrderBy(x => x.Category));
    }

    [Sample("Nested collections in a projection", Description = "Each customer with a list of order ids, loaded in one round trip.")]
    public static IEnumerable<string> NestedCollections()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Where(c => c.City == "Berlin")
                                        .OrderBy(c => c.Id)
                                        .Select(c => new { c.Name, OrderIds = c.Orders.OrderBy(o => o.Id).Select(o => o.Id).ToList() }),
                            x => $"{x.Name}: [{string.Join(",", x.OrderIds)}]");
    }

    [Sample("GroupBy over a navigation", Description = "Group by a column of a related table - the JOIN is added for you.")]
    public static IEnumerable<string> GroupByNavigation()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Orders.GroupBy(o => o.Customer.City)
                                     .Select(g => new { City = g.Key, Orders = g.Count(), Quantity = g.Sum(o => o.Quantity) })
                                     .OrderBy(x => x.City));
    }

    [Sample("Keyset pagination", Description = "Continue after the last seen key instead of OFFSET - stays fast on deep pages.")]
    public static IEnumerable<string> KeysetPagination()
    {
        using var db = ShopDbContext.CreateSeeded();
        var lastSeenId = 3;
        return SqlAndResult(db.Products.Where(p => p.Id > lastSeenId).OrderBy(p => p.Id).Take(2).Select(p => p.Name));
    }

    [Sample("Parameters vs EF.Constant", Description = "Captured variables become SQL parameters; EF.Constant inlines the value instead.")]
    public static IEnumerable<string> ParametersVsConstants()
    {
        using var db = ShopDbContext.CreateSeeded();
        var city = "Berlin";
        static string WhereClause(IQueryable<Customer> query) =>
            query.ToQueryString().Split('\n').Select(line => line.TrimEnd('\r')).First(line => line.StartsWith("WHERE", StringComparison.Ordinal));

        return
        [
            $"Variable:    {WhereClause(db.Customers.Where(c => c.City == city))}",
            $"EF.Constant: {WhereClause(db.Customers.Where(c => c.City == EF.Constant(city)))}",
        ];
    }

    [Sample("Math functions", Description = "Math.Round and arithmetic are evaluated by the database.")]
    public static IEnumerable<string> MathFunctions()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Products.Where(p => p.Category == "Furniture")
                                       .OrderBy(p => p.Id)
                                       .Select(p => new { p.Name, Third = Math.Round(p.Price / 3, 1) }));
    }

    [Sample("TagWith", Description = "Add a comment to the SQL so the query is easy to find in logs and profilers.")]
    public static IEnumerable<string> TagWith()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Products.TagWith("Dashboard: expensive products")
                                       .Where(p => p.Price > 200)
                                       .OrderBy(p => p.Id)
                                       .Select(p => p.Name));
    }

    /// <summary>For queries that end in a scalar (Count, Single...) and so cannot use ToQueryString.</summary>
    internal static List<string> LoggedSqlAndResult(ShopDbContext db, Func<object?> run)
    {
        db.ExecutedSql.Clear();
        var result = run();
        var sql = db.ExecutedSql.SelectMany(command => command.Split('\n')).Select(line => "  " + line.TrimEnd('\r'));
        return ["SQL:", .. sql, "Result:", $"  {result}"];
    }
}
