namespace LinqSamples.Data;

/// <summary>
/// A tiny in-memory "shop" shared by all samples. Small enough to verify results by eye.
/// </summary>
public static class SampleData
{
    public static IReadOnlyList<Customer> Customers { get; } =
    [
        new(1, "Alice", "Skopje"),
        new(2, "Bob", "Berlin"),
        new(3, "Carla", "Skopje"),
        new(4, "Dan", "Vienna"),
        new(5, "Eva", "Berlin"), // has no orders - useful for left joins
    ];

    public static IReadOnlyList<Product> Products { get; } =
    [
        new(1, "Laptop", "Electronics", 1200m),
        new(2, "Mouse", "Electronics", 25m),
        new(3, "Desk", "Furniture", 300m),
        new(4, "Chair", "Furniture", 150m),
        new(5, "Notebook", "Stationery", 3m),
        new(6, "Pen", "Stationery", 1.5m),
        new(7, "Monitor", "Electronics", 250m), // never ordered
    ];

    public static IReadOnlyList<Order> Orders { get; } =
    [
        new(1, 1, 1, 1, "Completed", new(2026, 1, 5)),
        new(2, 1, 2, 2, "Shipped", new(2026, 1, 20)),
        new(3, 2, 3, 1, "Pending", new(2026, 2, 2)),
        new(4, 2, 4, 4, "Awaiting Payment", new(2026, 2, 14)),
        new(5, 3, 5, 10, "Completed", new(2026, 2, 28)),
        new(6, 3, 1, 1, "Cancelled", new(2026, 3, 3)),
        new(7, 4, 6, 20, "Declined", new(2026, 3, 15)),
        new(8, 4, 2, 1, "Completed", new(2026, 3, 21)),
        new(9, 1, 4, 2, "Shipped", new(2026, 4, 1)),
        new(10, 3, 3, 1, "Awaiting Payment", new(2026, 4, 10)),
        new(11, 6, 5, 5, "Pending", new(2026, 4, 18)), // guest checkout: customer 6 does not exist
    ];

    public static IReadOnlyList<Employee> Employees { get; } =
    [
        new(1, "Grace", null),
        new(2, "Linus", 1),
        new(3, "Ada", 1),
        new(4, "Ken", 2),
        new(5, "Barbara", 2),
    ];

    public static int[] Numbers { get; } = [5, 3, 9, 1, 7, 3, 8, 2, 9, 4];

    public static string[] Words { get; } = ["apple", "Banana", "cherry", "avocado", "blueberry", "Apricot", "date"];
}
