using System.Text.Json;
using System.Text.Json.Nodes;

namespace LinqSamples.Samples;

/// <summary>
/// LINQ over JSON with System.Text.Json: <c>JsonNode</c> for a mutable DOM, <c>JsonDocument</c>
/// for fast read-only access, and <c>JsonSerializer</c> to move between JSON and typed objects.
/// </summary>
[SampleCategory("LINQ over JSON", 22)]
public static class JsonSamples
{
    private const string StoreJson = """
        {
          "store": "Downtown",
          "orders": [
            { "id": 1, "customer": "Alice", "status": "Completed",
              "items": [ { "sku": "LAP", "qty": 1, "price": 1200 }, { "sku": "MOU", "qty": 2, "price": 25 } ] },
            { "id": 2, "customer": "Bob", "status": "Pending",
              "items": [ { "sku": "DSK", "qty": 1, "price": 300 } ] },
            { "id": 3, "customer": "Alice", "status": "Completed", "coupon": "SPRING",
              "items": [ { "sku": "PEN", "qty": 10, "price": 1.5 } ] },
            { "id": 4, "customer": "Carla", "status": "Cancelled", "items": [] }
          ]
        }
        """;

    [Sample("Query a JsonNode", Description = "A JsonArray is an IEnumerable<JsonNode?> - distinct customers.")]
    public static IEnumerable<string> QueryJsonNode() =>
        OrderNodes().Select(o => o["customer"]!.GetValue<string>()).Distinct();

    [Sample("Filter a JSON array", Description = "Where on a property value.")]
    public static IEnumerable<int> FilterArray() =>
        OrderNodes().Where(o => o["status"]!.GetValue<string>() == "Completed")
                    .Select(o => o["id"]!.GetValue<int>());

    [Sample("Flatten nested arrays", Description = "SelectMany from orders into their line items.")]
    public static IEnumerable<string> FlattenNested() =>
        OrderNodes().SelectMany(o => o["items"]!.AsArray(),
                                (o, item) => $"{o["id"]}: {item!["sku"]} x{item["qty"]}");

    [Sample("Sum nested values", Description = "Revenue of completed orders across all their items.")]
    public static decimal SumNested() =>
        OrderNodes().Where(o => o["status"]!.GetValue<string>() == "Completed")
                    .SelectMany(o => o["items"]!.AsArray())
                    .Sum(i => i!["qty"]!.GetValue<int>() * i["price"]!.GetValue<decimal>());

    [Sample("Optional properties", Description = "A missing property is null - use ?. and ??.")]
    public static IEnumerable<string> OptionalProperties() =>
        OrderNodes().Select(o => $"#{o["id"]}: {o["coupon"]?.GetValue<string>() ?? "(none)"}");

    [Sample("Group by a property", Description = "Orders per customer.")]
    public static IEnumerable<string> GroupByProperty() =>
        OrderNodes().GroupBy(o => o["customer"]!.GetValue<string>())
                    .Select(g => $"{g.Key}: {g.Count()}");

    [Sample("Empty arrays", Description = "Find objects whose array property has no elements.")]
    public static IEnumerable<int> EmptyArrays() =>
        OrderNodes().Where(o => o["items"]!.AsArray().Count == 0).Select(o => o["id"]!.GetValue<int>());

    [Sample("Deserialize, then query", Description = "Typed records are often clearer than walking nodes.")]
    public static string DeserializeThenQuery()
    {
        var store = JsonSerializer.Deserialize<JsonStore>(StoreJson, JsonSerializerOptions.Web)!;
        var biggest = store.Orders.MaxBy(o => o.Items.Sum(i => i.Qty * i.Price))!;
        return $"{store.Store}: biggest order is #{biggest.Id} by {biggest.Customer} ({biggest.Items.Sum(i => i.Qty * i.Price):0.##})";
    }

    [Sample("Serialize a query result", Description = "Anonymous types serialize with camelCase names under JsonSerializerOptions.Web.")]
    public static string SerializeQuery() =>
        JsonSerializer.Serialize(Customers.Where(c => c.City == "Berlin").Select(c => new { c.Id, c.Name }),
                                 JsonSerializerOptions.Web);

    [Sample("Build JSON with JsonNode", Description = "Project data into JsonObject instances inside a JsonArray.")]
    public static string BuildJson() =>
        new JsonArray(Products.Where(p => p.Category == "Furniture")
                              .Select(p => (JsonNode)new JsonObject { ["name"] = p.Name, ["price"] = p.Price })
                              .ToArray())
            .ToJsonString();

    [Sample("JsonDocument", Description = "Read-only and allocation-light: EnumerateArray plus LINQ.")]
    public static string JsonDocumentQuery()
    {
        using var doc = JsonDocument.Parse(StoreJson);
        var completed = doc.RootElement.GetProperty("orders")
                                       .EnumerateArray()
                                       .Count(o => o.GetProperty("status").GetString() == "Completed");
        return $"Completed orders: {completed}";
    }

    [Sample("Enumerate object properties", Description = "A JsonObject is a sequence of key/value pairs.")]
    public static IEnumerable<string> ObjectProperties() =>
        OrderNodes().First().AsObject().Select(kv => kv.Key);

    [Sample("Modify JSON", Description = "Compute a value with LINQ and write it back into each object.")]
    public static IEnumerable<string> ModifyJson()
    {
        var orders = OrderNodes().ToList();
        foreach (var order in orders)
        {
            order["total"] = order["items"]!.AsArray().Sum(i => i!["qty"]!.GetValue<int>() * i["price"]!.GetValue<decimal>());
        }

        return orders.Select(o => $"#{o["id"]}: {o["total"]!.GetValue<decimal>():0.##}");
    }

    [Sample("Quantity per customer", Description = "AggregateBy over flattened items.")]
    public static IEnumerable<string> QuantityPerCustomer() =>
        OrderNodes().SelectMany(o => o["items"]!.AsArray(), (o, item) => (Customer: o["customer"]!.GetValue<string>(), Qty: item!["qty"]!.GetValue<int>()))
                    .AggregateBy(x => x.Customer, 0, (total, x) => total + x.Qty)
                    .Select(kv => $"{kv.Key}: {kv.Value}");

    [Sample("JSON Lines", Description = "Parse one JSON object per line (NDJSON), then filter.")]
    public static IEnumerable<string> JsonLines()
    {
        const string log = """
            {"level":"info","msg":"start"}
            {"level":"error","msg":"disk full"}
            {"level":"warn","msg":"low memory"}
            {"level":"error","msg":"timeout"}
            """;
        return log.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                  .Select(line => JsonNode.Parse(line)!)
                  .Where(entry => entry["level"]!.GetValue<string>() == "error")
                  .Select(entry => entry["msg"]!.GetValue<string>());
    }

    private static IEnumerable<JsonNode> OrderNodes() =>
        JsonNode.Parse(StoreJson)!["orders"]!.AsArray().Select(o => o!);
}

public sealed record JsonStore(string Store, IReadOnlyList<JsonOrder> Orders);

public sealed record JsonOrder(int Id, string Customer, string Status, IReadOnlyList<JsonLineItem> Items, string? Coupon);

public sealed record JsonLineItem(string Sku, int Qty, decimal Price);
