namespace LinqSamples.Samples;

/// <summary>Recipe ingredients, weights and costs.</summary>
[SampleCategory("Kitchen costs", 60)]
public static class KitchenSamples
{
    private sealed record Ingredient(string Dish, string Item, int Grams, int Cost);

    private static readonly Ingredient[] Items =
    [
        new("Pasta", "flour", 200, 1), new("Pasta", "egg", 100, 2), new("Pizza", "flour", 300, 2), new("Pizza", "cheese", 150, 4),
        new("Salad", "tomato", 120, 3), new("Salad", "cheese", 50, 2), new("Soup", "tomato", 200, 4),
    ];

    [Sample("Dishes", Description = "Distinct dish names.")]
    public static IEnumerable<string> Dishes() => Items.Select(i => i.Dish).Distinct().Order();

    [Sample("Cost per dish", Description = "Group by dish and sum cost.")]
    public static IEnumerable<string> CostPerDish() => Items.GroupBy(i => i.Dish).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(i => i.Cost)}");

    [Sample("Most expensive dish", Description = "MaxBy over grouped cost.")]
    public static string MostExpensive() => Items.GroupBy(i => i.Dish).MaxBy(g => g.Sum(i => i.Cost))!.Key;

    [Sample("Shared ingredients", Description = "Items used by more than one dish.")]
    public static IEnumerable<string> Shared() =>
        Items.GroupBy(i => i.Item).Where(g => g.Select(i => i.Dish).Distinct().Count() > 1).Select(g => g.Key).Order();

    [Sample("Dishes with cheese", Description = "Filter then Distinct.")]
    public static IEnumerable<string> WithCheese() => Items.Where(i => i.Item == "cheese").Select(i => i.Dish).Distinct().Order();

    [Sample("Grams per ingredient", Description = "Group by item and sum grams.")]
    public static IEnumerable<string> GramsPerItem() => Items.GroupBy(i => i.Item).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(i => i.Grams)}");

    [Sample("Ingredient count per dish", Description = "CountBy dish.")]
    public static IEnumerable<string> CountPerDish() => Items.CountBy(i => i.Dish).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Single-ingredient dishes", Description = "Groups of exactly one.")]
    public static IEnumerable<string> SingleIngredient() => Items.GroupBy(i => i.Dish).Where(g => g.Count() == 1).Select(g => g.Key);

    [Sample("Heaviest ingredient line", Description = "MaxBy grams.")]
    public static string Heaviest()
    {
        var i = Items.MaxBy(x => x.Grams)!;
        return $"{i.Dish} {i.Item}";
    }

    [Sample("Total cost", Description = "Sum of every ingredient cost.")]
    public static int TotalCost() => Items.Sum(i => i.Cost);
}
