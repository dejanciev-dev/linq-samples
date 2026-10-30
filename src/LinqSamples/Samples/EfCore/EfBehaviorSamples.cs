using Microsoft.EntityFrameworkCore;
using static LinqSamples.Samples.EfCore.EfCoreSamples;
using static LinqSamples.Samples.EfCore.EfTranslationSamples;

namespace LinqSamples.Samples.EfCore;

/// <summary>
/// Loading related data, change tracking, saving, raw SQL and async queries. Several samples count
/// the SQL commands EF Core executes, which makes round trips visible.
/// </summary>
[SampleCategory("EF Core: loading, tracking & saving", 26)]
public static class EfBehaviorSamples
{
    // Explicit type arguments: with OrderBy the lambda returns IOrderedQueryable, and inference would
    // otherwise pick the single-result overload and fail at runtime.
    private static readonly Func<ShopDbContext, string, IEnumerable<Customer>> CustomersInCity =
        EF.CompileQuery<ShopDbContext, string, Customer>((db, city) => db.Customers.Where(c => c.City == city).OrderBy(c => c.Id));

    [Sample("Single vs split query", Description = "AsSplitQuery loads a collection Include with one extra query instead of one wide JOIN.")]
    public static IEnumerable<string> SplitQuery()
    {
        using var db = ShopDbContext.CreateSeeded();
        _ = db.Customers.Include(c => c.Orders).ToList();
        var single = db.ExecutedSql.Count;

        db.ChangeTracker.Clear();
        db.ExecutedSql.Clear();
        _ = db.Customers.Include(c => c.Orders).AsSplitQuery().ToList();
        var split = db.ExecutedSql.Count;

        return [$"Single query: {single} SQL command", $"Split query:  {split} SQL commands"];
    }

    [Sample("The N+1 problem", Description = "Loading related rows per parent costs one query each; Include needs one in total.")]
    public static IEnumerable<string> NPlusOne()
    {
        using var db = ShopDbContext.CreateSeeded();
        foreach (var customer in db.Customers.ToList())
        {
            db.Entry(customer).Collection(c => c.Orders).Load();
        }

        var perCustomer = db.ExecutedSql.Count;

        db.ChangeTracker.Clear();
        db.ExecutedSql.Clear();
        _ = db.Customers.Include(c => c.Orders).ToList();
        var included = db.ExecutedSql.Count;

        return [$"Load per customer: {perCustomer} SQL commands", $"Include:           {included} SQL command"];
    }

    [Sample("Filtered Include", Description = "Load only the related rows you need (EF Core 5+).")]
    public static IEnumerable<string> FilteredInclude()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Include(c => c.Orders.Where(o => o.Status == "Completed")).OrderBy(c => c.Id),
                            c => $"{c.Name}: {c.Orders.Count} completed");
    }

    [Sample("ThenInclude", Description = "Load a chain of relationships: customers, their orders, and each order's product.")]
    public static IEnumerable<string> ThenInclude()
    {
        using var db = ShopDbContext.CreateSeeded();
        return SqlAndResult(db.Customers.Include(c => c.Orders).ThenInclude(o => o.Product)
                                        .Where(c => c.City == "Berlin")
                                        .OrderBy(c => c.Id),
                            c => $"{c.Name}: {string.Join(", ", c.Orders.OrderBy(o => o.Id).Select(o => o.Product.Name))}");
    }

    [Sample("Query a navigation without loading it", Description = "Entry(...).Collection(...).Query() counts in SQL, leaving the collection empty.")]
    public static IEnumerable<string> QueryNavigation()
    {
        using var db = ShopDbContext.CreateSeeded();
        var alice = db.Customers.Single(c => c.Name == "Alice");
        var count = db.Entry(alice).Collection(c => c.Orders).Query().Count();
        return [$"Orders counted in SQL: {count}", $"Orders loaded:         {alice.Orders.Count}"];
    }

    [Sample("Find uses the change tracker", Description = "Find returns an already tracked entity without asking the database again.")]
    public static IEnumerable<string> FindUsesTracker()
    {
        using var db = ShopDbContext.CreateSeeded();
        _ = db.Products.Find(1);
        _ = db.Products.Find(1);
        var finds = db.ExecutedSql.Count;

        db.ChangeTracker.Clear();
        db.ExecutedSql.Clear();
        _ = db.Products.FirstOrDefault(p => p.Id == 1);
        _ = db.Products.FirstOrDefault(p => p.Id == 1);
        var queries = db.ExecutedSql.Count;

        return [$"Find twice:           {finds} SQL command", $"FirstOrDefault twice: {queries} SQL commands"];
    }

    [Sample("Identity resolution", Description = "A tracking context returns the same instance for the same row; AsNoTracking creates new ones.")]
    public static IEnumerable<string> IdentityResolution()
    {
        using var db = ShopDbContext.CreateSeeded();
        var byId = db.Customers.Single(c => c.Id == 1);
        var byName = db.Customers.Single(c => c.Name == "Alice");
        var first = db.Customers.AsNoTracking().Single(c => c.Id == 1);
        var second = db.Customers.AsNoTracking().Single(c => c.Id == 1);
        return [$"Tracked, same instance:   {ReferenceEquals(byId, byName)}", $"AsNoTracking, same:       {ReferenceEquals(first, second)}"];
    }

    [Sample("Change tracking", Description = "Edit a loaded entity; SaveChanges updates only the modified column.")]
    public static IEnumerable<string> ChangeTracking()
    {
        using var db = ShopDbContext.CreateSeeded();
        var mouse = db.Products.Single(p => p.Name == "Mouse");
        mouse.Price = 20;
        db.ChangeTracker.DetectChanges();

        var entry = db.Entry(mouse);
        var state = entry.State;
        var modified = entry.Properties.Where(p => p.IsModified).Select(p => p.Metadata.Name).ToList();

        db.ExecutedSql.Clear();
        var saved = db.SaveChanges();
        return
        [
            $"State after edit:  {state}",
            $"Modified columns:  {string.Join(", ", modified)}",
            $"Rows saved:        {saved}",
            .. db.ExecutedSql.Single().Split('\n').Select(line => "  " + line.TrimEnd('\r')),
        ];
    }

    [Sample("Add and generated keys", Description = "Added entities get their database-generated key after SaveChanges.")]
    public static IEnumerable<string> AddEntity()
    {
        using var db = ShopDbContext.CreateSeeded();
        var webcam = new Product { Name = "Webcam", Category = "Electronics", Price = 80 };
        db.Products.Add(webcam);
        var before = db.Entry(webcam).State;
        db.SaveChanges();
        return [$"Before save: {before}", $"After save:  {db.Entry(webcam).State}, Id = {webcam.Id}"];
    }

    [Sample("Remove", Description = "Mark an entity Deleted; SaveChanges issues the DELETE.")]
    public static IEnumerable<string> RemoveEntity()
    {
        using var db = ShopDbContext.CreateSeeded();
        var monitor = db.Products.Single(p => p.Name == "Monitor");
        db.Products.Remove(monitor);
        var state = db.Entry(monitor).State;
        db.SaveChanges();
        return [$"State:         {state}", $"Products left: {db.Products.Count()}"];
    }

    [Sample("Transactions", Description = "Roll back work - including ExecuteDelete - that ran inside a transaction.")]
    public static IEnumerable<string> Transactions()
    {
        using var db = ShopDbContext.CreateSeeded();
        using var transaction = db.Database.BeginTransaction();
        var deleted = db.Orders.ExecuteDelete();
        var inside = db.Orders.Count();
        transaction.Rollback();
        return [$"Deleted in transaction: {deleted}", $"Count inside:           {inside}", $"Count after rollback:   {db.Orders.Count()}"];
    }

    [Sample("Global query filters", Description = "A filter defined in the model applies to every query; IgnoreQueryFilters switches it off.")]
    public static IEnumerable<string> GlobalQueryFilters()
    {
        using var db = FilteredShopDbContext.CreateSeeded();
        var where = db.Orders.ToQueryString().Split('\n').Select(line => line.TrimEnd('\r')).First(line => line.StartsWith("WHERE", StringComparison.Ordinal));
        return
        [
            $"Generated:          {where}",
            $"Orders (filtered):  {db.Orders.Count()}",
            $"IgnoreQueryFilters: {db.Orders.IgnoreQueryFilters().Count()}",
        ];
    }

    [Sample("Compiled queries", Description = "EF.CompileQuery skips translation on every call after the first (give the type arguments explicitly).")]
    public static IEnumerable<string> CompiledQuery()
    {
        using var db = ShopDbContext.CreateSeeded();
        return CustomersInCity(db, "Skopje").Select(c => c.Name).ToList();
    }

    [Sample("FromSql, then LINQ", Description = "Interpolated values become parameters, and LINQ composes on top of the raw SQL.")]
    public static IEnumerable<string> FromSqlComposed()
    {
        using var db = ShopDbContext.CreateSeeded();
        var minPrice = 100.0;
        return SqlAndResult(db.Products.FromSql($"SELECT * FROM Products WHERE Price > {minPrice}")
                                       .Where(p => p.Category == "Furniture")
                                       .OrderBy(p => p.Name)
                                       .Select(p => p.Name));
    }

    [Sample("SqlQuery for scalars", Description = "Database.SqlQuery<T> maps raw SQL to any type; name the column Value to compose on it.")]
    public static IEnumerable<string> SqlQueryScalar()
    {
        using var db = ShopDbContext.CreateSeeded();
        return LoggedSqlAndResult(db, () => db.Database.SqlQuery<int>($"SELECT COUNT(*) AS Value FROM Orders").Single());
    }

    [Sample("Async queries", Description = "ToListAsync, CountAsync and AnyAsync free the thread while the database works.")]
    public static async Task<IEnumerable<string>> AsyncQueries()
    {
        await using var db = ShopDbContext.CreateSeeded();
        var count = await db.Orders.CountAsync();
        var anyDeclined = await db.Orders.AnyAsync(o => o.Status == "Declined");
        var topTwo = await db.Products.OrderByDescending(p => p.Price).Select(p => p.Name).Take(2).ToListAsync();
        return [$"Orders:        {count}", $"Any declined?  {anyDeclined}", $"Top two:       {string.Join(", ", topTwo)}"];
    }

    [Sample("Streaming with AsAsyncEnumerable", Description = "Process rows as they arrive instead of buffering the whole result.")]
    public static async Task<List<string>> Streaming()
    {
        await using var db = ShopDbContext.CreateSeeded();
        List<string> names = [];
        await foreach (var name in db.Products.OrderBy(p => p.Id).Select(p => p.Name).AsAsyncEnumerable())
        {
            names.Add(name);
        }

        return names;
    }
}
