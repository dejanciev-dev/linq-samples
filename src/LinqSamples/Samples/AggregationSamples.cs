namespace LinqSamples.Samples;

[SampleCategory("Aggregation", 8)]
public static class AggregationSamples
{
    [Sample("Count / Sum / Min / Max / Average")]
    public static IEnumerable<string> Basics() =>
    [
        $"Count:   {Numbers.Length}",
        $"Sum:     {Numbers.Sum()}",
        $"Min:     {Numbers.Min()}",
        $"Max:     {Numbers.Max()}",
        $"Average: {Numbers.Average()}",
    ];

    [Sample("Sum with selector", Description = "Revenue of completed orders.")]
    public static decimal Revenue() =>
        Orders.Where(o => o.Status == "Completed")
              .Sum(o => o.Quantity * Products.First(p => p.Id == o.ProductId).Price);

    [Sample("MinBy / MaxBy (.NET 6+)", Description = "Return the element itself, not just the key value.")]
    public static IEnumerable<Product> MinByMaxBy() =>
        [Products.MinBy(p => p.Price)!, Products.MaxBy(p => p.Price)!];

    [Sample("Count with predicate")]
    public static int CountWithPredicate() =>
        Orders.Count(o => o.Status is "Pending" or "Awaiting Payment");

    [Sample("Aggregate", Description = "A general-purpose fold - here: join names into a sentence.")]
    public static string Aggregate() =>
        Customers.Select(c => c.Name)
                 .Aggregate((acc, next) => $"{acc}, {next}");

    [Sample("Aggregate with seed", Description = "Compute 10! with an explicit seed and result type.")]
    public static long Factorial() =>
        Enumerable.Range(1, 10).Aggregate(1L, (acc, n) => acc * n);

    [Sample("Aggregate with a result selector", Description = "Seed, fold and transform the final value in one call - the longest word, upper-cased.")]
    public static string AggregateWithResultSelector() =>
        Words.Aggregate("",
                        (longest, word) => word.Length > longest.Length ? word : longest,
                        longest => longest.ToUpperInvariant());

    [Sample("Max of an empty sequence", Description = "Min/Max/Average throw on empty input; a nullable selector or DefaultIfEmpty avoids it.")]
    public static IEnumerable<string> MaxOfEmpty()
    {
        int[] empty = [];
        string Throws()
        {
            try
            {
                return $"{empty.Max()}";
            }
            catch (InvalidOperationException)
            {
                return "throws InvalidOperationException";
            }
        }

        return
        [
            $"Max():                  {Throws()}",
            $"DefaultIfEmpty().Max(): {empty.DefaultIfEmpty().Max()}",
            $"Max(n => (int?)n):      {empty.Max(n => (int?)n)?.ToString() ?? "null"}",
        ];
    }

    [Sample("Running total", Description = "Scan-like behavior via Select and a captured accumulator (materialize it once!).")]
    public static IEnumerable<int> RunningTotal()
    {
        var total = 0;
        return Numbers.Select(n => total += n).ToList();
    }
}
