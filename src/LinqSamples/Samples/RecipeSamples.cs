namespace LinqSamples.Samples;

/// <summary>Everyday problems solved with LINQ.</summary>
[SampleCategory("Real-world recipes", 14)]
public static class RecipeSamples
{
    [Sample("Top N per group", Description = "The most expensive product in each category.")]
    public static IEnumerable<string> TopNPerGroup() =>
        Products.GroupBy(p => p.Category)
                .Select(g => g.MaxBy(p => p.Price)!)
                .OrderBy(p => p.Category)
                .Select(p => $"{p.Category}: {p.Name}");

    [Sample("Customer revenue report", Description = "Join, group, sum and rank in one pipeline.")]
    public static IEnumerable<string> RevenueReport() =>
        Orders.Where(o => o.Status is "Completed" or "Shipped")
              .Join(Products, o => o.ProductId, p => p.Id, (o, p) => (o.CustomerId, Total: o.Quantity * p.Price))
              .GroupBy(x => x.CustomerId, x => x.Total)
              .Join(Customers, g => g.Key, c => c.Id, (g, c) => (c.Name, Revenue: g.Sum()))
              .OrderByDescending(x => x.Revenue)
              .Select((x, rank) => $"{rank + 1}. {x.Name}: {x.Revenue}");

    [Sample("Pivot", Description = "Turn rows into columns: order count per status per month.")]
    public static IEnumerable<string> Pivot()
    {
        string[] statuses = ["Completed", "Shipped", "Pending"];
        var header = $"Month   | {string.Join(" | ", statuses)}";
        var rows = Orders.GroupBy(o => o.Date.Month)
                         .OrderBy(g => g.Key)
                         .Select(g => $"2026-{g.Key:00} | " +
                                      string.Join(" | ", statuses.Select(s => g.Count(o => o.Status == s).ToString().PadLeft(s.Length))));
        return rows.Prepend(header);
    }

    [Sample("Find duplicates", Description = "Values that appear more than once.")]
    public static IEnumerable<int> FindDuplicates() =>
        Numbers.CountBy(n => n).Where(kv => kv.Value > 1).Select(kv => kv.Key).Order();

    [Sample("Word frequency", Description = "Count words in a text, case-insensitively.")]
    public static IEnumerable<string> WordFrequency()
    {
        const string text = "The quick brown fox jumps over the lazy dog. The dog sleeps.";
        return text.Split([' ', '.'], StringSplitOptions.RemoveEmptyEntries)
                   .CountBy(w => w.ToLowerInvariant())
                   .OrderByDescending(kv => kv.Value)
                   .ThenBy(kv => kv.Key)
                   .Take(3)
                   .Select(kv => $"{kv.Key}: {kv.Value}");
    }

    [Sample("Flatten a hierarchy", Description = "Recursively walk an org chart with SelectMany.")]
    public static IEnumerable<string> FlattenHierarchy()
    {
        IEnumerable<string> Walk(int? managerId, int depth) =>
            Employees.Where(e => e.ManagerId == managerId)
                     .SelectMany(e => Walk(e.Id, depth + 1).Prepend(new string(' ', depth * 2) + e.Name));

        return Walk(null, 0);
    }

    [Sample("Batch processing", Description = "Process items in batches of 3, e.g. for bulk API calls.")]
    public static IEnumerable<string> BatchProcessing() =>
        Orders.Select(o => o.Id)
              .Chunk(3)
              .Select((batch, i) => $"Batch {i + 1}: {string.Join(",", batch)}");
}
