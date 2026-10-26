using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlite(connection);

    /// <summary>Creates a fresh, seeded in-memory database. Disposing the context deletes it.</summary>
    public static ShopDbContext CreateSeeded()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open(); // an in-memory SQLite database lives as long as its connection

        var db = new ShopDbContext(connection);
        db.Database.EnsureCreated();

        db.Customers.AddRange(SampleData.Customers.Select(c => new Customer { Id = c.Id, Name = c.Name, City = c.City }));
        db.Products.AddRange(SampleData.Products.Select(p => new Product
        {
            Id = p.Id, Name = p.Name, Category = p.Category, Price = (double)p.Price,
        }));
        // The guest-checkout order points at a customer that does not exist, which a foreign key rejects.
        var customerIds = SampleData.Customers.Select(c => c.Id).ToHashSet();
        db.Orders.AddRange(SampleData.Orders.Where(o => customerIds.Contains(o.CustomerId)).Select(o => new Order
        {
            Id = o.Id, CustomerId = o.CustomerId, ProductId = o.ProductId,
            Quantity = o.Quantity, Status = o.Status, Date = o.Date,
        }));
        db.SaveChanges();
        db.ChangeTracker.Clear();

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
