using Microsoft.EntityFrameworkCore;

namespace LinqSamples.Samples.EfCore;

/// <summary>
/// LINQ against a database: the same operators build an expression tree (IQueryable) that
/// EF Core translates to SQL. Each sample prints the generated SQL followed by the result.
/// </summary>
[SampleCategory("EF Core (IQueryable)", 15)]
public static class EfCoreSamples
{
    [Sample("ToQueryString", Description = "See the SQL a query will run without executing it.")]
    public static IEnumerable<string> ToQueryString()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Products.Where(p => p.Price > 100).OrderBy(p => p.Price).Select(p => p.Name));
    }

    [Sample("Project only what you need", Description = "Select limits the SQL to the columns you use.")]
    public static IEnumerable<string> Projection()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Where(c => c.City == "Berlin").Select(c => new { c.Name, c.City }));
    }

    [Sample("Navigation properties become JOINs")]
    public static IEnumerable<string> NavigationJoins()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Orders.Where(o => o.Customer.City == "Skopje")
                                     .OrderBy(o => o.Id)
                                     .Select(o => new { o.Id, Customer = o.Customer.Name, Product = o.Product.Name }));
    }

    [Sample("Include", Description = "Load related entities together with the parent.")]
    public static IEnumerable<string> Include()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Include(c => c.Orders).Where(c => c.City == "Berlin").OrderBy(c => c.Id),
                            c => $"{c.Name}: {c.Orders.Count} orders");
    }

    [Sample("GroupBy with aggregates", Description = "Translated to GROUP BY with COUNT and SUM.")]
    public static IEnumerable<string> GroupBy()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Orders.GroupBy(o => o.Status)
                                     .Select(g => new { Status = g.Key, Orders = g.Count(), Quantity = g.Sum(o => o.Quantity) })
                                     .OrderBy(x => x.Status));
    }

    [Sample("LeftJoin (EF Core 10+)", Description = "The new .NET 10 operator translates straight to LEFT JOIN.")]
    public static IEnumerable<string> LeftJoin()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.LeftJoin(db.Orders,
                                                  c => c.Id,
                                                  o => o.CustomerId,
                                                  (c, o) => new { c.Name, OrderId = o == null ? (int?)null : o.Id })
                                        .OrderBy(x => x.Name).ThenBy(x => x.OrderId));
    }

    [Sample("Contains with a local list", Description = "A collection from your code becomes an SQL IN with parameters.")]
    public static IEnumerable<string> ContainsLocalList()
    {
        using var db = ShopDbContext.CreateSeeded();
        int[] ids = [1, 3, 5];
        return SqlAndResult(db.Customers.Where(c => ids.Contains(c.Id)).Select(c => c.Name));
    }

    [Sample("Paging", Description = "Skip/Take become LIMIT/OFFSET. Always order before paging.")]
    public static IEnumerable<string> Paging()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Products.OrderBy(p => p.Id).Skip(3).Take(3).Select(p => p.Name));
    }

    [Sample("Untranslatable code", Description = "Calling your own C# method inside a query throws; switch to LINQ to Objects explicitly.")]
    public static IEnumerable<string> ClientEvaluation()
    {
        using var db = ShopDbContext.CreateSeeded();
        string outcome;
        try
        {
            _ = db.Customers.Where(c => IsVip(c.Name)).ToList();
            outcome = "no exception";
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("could not be translated"))
        {
            outcome = "InvalidOperationException: the LINQ expression could not be translated";
        }

        // Filter what SQL can in the database, then continue in memory after AsEnumerable().
        var vips = db.Customers.Where(c => c.City == "Skopje")
                               .AsEnumerable()
                               .Where(c => IsVip(c.Name))
                               .Select(c => c.Name);

        return [$"Where(c => IsVip(c.Name)) -> {outcome}", $"AsEnumerable() first      -> {string.Join(", ", vips)}"];
    }

    [Sample("IQueryable vs IEnumerable", Description = "Filtering an IEnumerable happens in memory, after every row was loaded.")]
    public static IEnumerable<string> QueryableVsEnumerable()
    {
        using var db = ShopDbContext.CreateSeeded();

        IQueryable<Order> queryable = db.Orders;
        var fromSql = queryable.Where(o => o.Quantity > 2).ToList();
        var loadedBySql = db.ChangeTracker.Entries().Count();
        db.ChangeTracker.Clear();

        IEnumerable<Order> enumerable = db.Orders;
        var inMemory = enumerable.Where(o => o.Quantity > 2).ToList();
        var loadedInMemory = db.ChangeTracker.Entries().Count();

        return
        [
            $"IQueryable:  {fromSql.Count} matches, {loadedBySql} rows loaded",
            $"IEnumerable: {inMemory.Count} matches, {loadedInMemory} rows loaded",
        ];
    }

    [Sample("AsNoTracking", Description = "Read-only queries skip change tracking.")]
    public static IEnumerable<string> AsNoTracking()
    {
        using var db = ShopDbContext.CreateSeeded();

        _ = db.Products.ToList();
        var tracked = db.ChangeTracker.Entries().Count();
        db.ChangeTracker.Clear();

        _ = db.Products.AsNoTracking().ToList();
        var untracked = db.ChangeTracker.Entries().Count();

        return [$"Tracked entities:   {tracked}", $"With AsNoTracking:  {untracked}"];
    }

    [Sample("ExecuteUpdate / ExecuteDelete (EF Core 7+)", Description = "Bulk changes in one SQL statement, without loading entities.")]
    public static IEnumerable<string> BulkOperations()
    {
        using var db = ShopDbContext.CreateSeeded();

        var updated = db.Orders.Where(o => o.Status == "Pending")
                               .ExecuteUpdate(s => s.SetProperty(o => o.Status, "Cancelled"));
        var deleted = db.Orders.Where(o => o.Status == "Cancelled").ExecuteDelete();

        return
        [
            $"Updated Pending -> Cancelled: {updated}",
            $"Deleted Cancelled:            {deleted}",
            $"Orders left:                  {db.Orders.Count()}",
        ];
    }

    private static bool IsVip(string name) => name.StartsWith('A');

    internal static List<string> SqlAndResult<T>(IQueryable<T> query, Func<T, string>? format = null)
    {
        var sql = query.ToQueryString().Split('\n').Select(line => "  " + line.TrimEnd('\r'));
        var rows = query.ToList().Select(row => "  " + (format is null ? row?.ToString() : format(row)));
        return ["SQL:", .. sql, "Result:", .. rows];
    }
}
