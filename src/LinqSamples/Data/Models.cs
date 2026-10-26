namespace LinqSamples.Data;

public sealed record Customer(int Id, string Name, string City);

public sealed record Product(int Id, string Name, string Category, decimal Price);

public sealed record Order(int Id, int CustomerId, int ProductId, int Quantity, string Status, DateOnly Date);

public sealed record Employee(int Id, string Name, int? ManagerId);
