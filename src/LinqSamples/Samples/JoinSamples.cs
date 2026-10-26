namespace LinqSamples.Samples;

[SampleCategory("Joins", 6)]
public static class JoinSamples
{
    [Sample("Inner join", Description = "Only orders that have a matching customer.")]
    public static IEnumerable<string> InnerJoin() =>
        Orders.Join(Customers,
                    o => o.CustomerId,
                    c => c.Id,
                    (o, c) => $"#{o.Id} {c.Name}");

    [Sample("GroupJoin", Description = "Each customer with all of their orders (customers without orders included).")]
    public static IEnumerable<string> GroupJoin() =>
        Customers.GroupJoin(Orders,
                            c => c.Id,
                            o => o.CustomerId,
                            (c, orders) => $"{c.Name}: {orders.Count()} orders");

    [Sample("LeftJoin (.NET 10+)", Description = "All customers; the order is null when there is no match.")]
    public static IEnumerable<string> LeftJoin() =>
        Customers.LeftJoin(Orders,
                           c => c.Id,
                           o => o.CustomerId,
                           (c, o) => $"{c.Name} -> {Describe(o)}");

    [Sample("Left join (classic pattern)", Description = "Before .NET 10: join ... into + DefaultIfEmpty.")]
    public static IEnumerable<string> LeftJoinClassic() =>
        from c in Customers
        join o in Orders on c.Id equals o.CustomerId into customerOrders
        from o in customerOrders.DefaultIfEmpty()
        select $"{c.Name} -> {Describe(o)}";

    [Sample("RightJoin (.NET 10+)", Description = "All orders; the customer is null for guest checkouts.")]
    public static IEnumerable<string> RightJoin() =>
        Customers.RightJoin(Orders,
                            c => c.Id,
                            o => o.CustomerId,
                            (c, o) => $"#{o.Id} {c?.Name ?? "(guest)"}");

    [Sample("Multi-table join", Description = "Completed order lines with customer, product and line total.")]
    public static IEnumerable<string> MultiJoin() =>
        from o in Orders
        join c in Customers on o.CustomerId equals c.Id
        join p in Products on o.ProductId equals p.Id
        where o.Status == "Completed"
        select $"{c.Name} bought {o.Quantity} x {p.Name} = {o.Quantity * p.Price}";

    [Sample("Self join", Description = "Join a table to itself - employees with their manager.")]
    public static IEnumerable<string> SelfJoin() =>
        from e in Employees
        join m in Employees on e.ManagerId equals m.Id
        select $"{e.Name} reports to {m.Name}";

    private static string Describe(Order? order) => order is null ? "(none)" : $"#{order.Id}";
}
