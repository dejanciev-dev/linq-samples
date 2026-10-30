using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LinqSamples.Samples.EfCore;

public sealed class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public List<Order> Orders { get; } = [];
}

public sealed class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }

    // SQLite cannot compare or aggregate decimals server-side, so prices are stored as REAL.
    public double Price { get; set; }
}

public sealed class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public required string Status { get; set; }
    public DateOnly Date { get; set; }
}

/// <summary>
/// The sample shop as a real database: SQLite in memory, so queries are translated to actual SQL.
/// </summary>
public sealed class ShopDbContext(DbConnection connection) : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    /// <summary>The SQL text of every command executed since the database was seeded.</summary>
    public List<string> ExecutedSql { get; } = [];

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite(connection)
                      .LogTo(message => ExecutedSql.Add(message[(message.IndexOf('\n', StringComparison.Ordinal) + 1)..].TrimEnd()),
                             [RelationalEventId.CommandExecuted],
                             LogLevel.Information,
                             DbContextLoggerOptions.None);

    /// <summary>Creates a fresh, seeded in-memory database. Disposing the context deletes it.</summary>
    public static ShopDbContext CreateSeeded()
    {
        var db = new ShopDbContext(ShopDatabase.OpenInMemory());
        ShopDatabase.Seed(db);
        db.ExecutedSql.Clear();
        return db;
    }

    public override void Dispose()
    {
        base.Dispose();
        connection.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await connection.DisposeAsync();
    }
}

/// <summary>
/// The same shop with a global query filter that hides cancelled orders. A separate context keeps
/// the filter out of the SQL of every other sample.
/// </summary>
public sealed class FilteredShopDbContext(DbConnection connection) : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite(connection);

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<Order>().HasQueryFilter(o => o.Status != "Cancelled");

    public static FilteredShopDbContext CreateSeeded()
    {
        var db = new FilteredShopDbContext(ShopDatabase.OpenInMemory());
        ShopDatabase.Seed(db);
        return db;
    }

    public override void Dispose()
    {
        base.Dispose();
        connection.Dispose();
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await connection.DisposeAsync();
    }
}

internal static class ShopDatabase
{
    public static SqliteConnection OpenInMemory()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open(); // an in-memory SQLite database lives as long as its connection
        return connection;
    }

    public static void Seed(DbContext db)
    {
        db.Database.EnsureCreated();

        db.Set<Customer>().AddRange(SampleData.Customers.Select(c => new Customer { Id = c.Id, Name = c.Name, City = c.City }));
        db.Set<Product>().AddRange(SampleData.Products.Select(p => new Product
        {
            Id = p.Id, Name = p.Name, Category = p.Category, Price = (double)p.Price,
        }));
        // The guest-checkout order points at a customer that does not exist, which a foreign key rejects.
        var customerIds = SampleData.Customers.Select(c => c.Id).ToHashSet();
        db.Set<Order>().AddRange(SampleData.Orders.Where(o => customerIds.Contains(o.CustomerId)).Select(o => new Order
        {
            Id = o.Id, CustomerId = o.CustomerId, ProductId = o.ProductId,
            Quantity = o.Quantity, Status = o.Status, Date = o.Date,
        }));
        db.SaveChanges();
        db.ChangeTracker.Clear();
    }
}
