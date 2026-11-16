namespace LinqSamples.Samples;

/// <summary>Recipes: cuisines, cooking time, calories and servings.</summary>
[SampleCategory("Meals", 78)]
public static class MealSamples
{
    private sealed record Recipe(string Name, string Cuisine, int Minutes, int Calories, int Servings);

    private static readonly Recipe[] Recipes =
    [
        new("Pizza", "Italian", 45, 800, 2), new("Pasta", "Italian", 20, 600, 2),
        new("Sushi", "Japanese", 50, 400, 1), new("Ramen", "Japanese", 30, 550, 1),
        new("Taco", "Mexican", 15, 300, 3), new("Burrito", "Mexican", 25, 700, 1),
    ];

    [Sample("Total calories", Description = "Sum of calories.")]
    public static int TotalCalories() => Recipes.Sum(r => r.Calories);

    [Sample("Recipes per cuisine", Description = "Group by cuisine and count.")]
    public static IEnumerable<string> PerCuisine() => Recipes.GroupBy(r => r.Cuisine).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Count()}");

    [Sample("Quickest recipe", Description = "MinBy cooking time.")]
    public static string Quickest() => Recipes.MinBy(r => r.Minutes)!.Name;

    [Sample("Slowest recipe", Description = "MaxBy cooking time.")]
    public static string Slowest() => Recipes.MaxBy(r => r.Minutes)!.Name;

    [Sample("Quick meals", Description = "Filter by time and sort names.")]
    public static IEnumerable<string> QuickMeals() => Recipes.Where(r => r.Minutes <= 25).Select(r => r.Name).Order();

    [Sample("Minutes per cuisine", Description = "Sum time per group.")]
    public static IEnumerable<string> MinutesPerCuisine() => Recipes.GroupBy(r => r.Cuisine).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(r => r.Minutes)}");

    [Sample("Lightest per cuisine", Description = "MinBy calories inside each group.")]
    public static IEnumerable<string> LightestPerCuisine() => Recipes.GroupBy(r => r.Cuisine).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MinBy(r => r.Calories)!.Name}");

    [Sample("Average calories", Description = "Average rounded to one decimal.")]
    public static double AverageCalories() => Math.Round(Recipes.Average(r => r.Calories), 1);

    [Sample("Shareable recipes", Description = "Serves more than one person.")]
    public static IEnumerable<string> Shareable() => Recipes.Where(r => r.Servings > 1).Select(r => r.Name).Order();

    [Sample("Cuisine then slowest first", Description = "OrderBy cuisine, ThenByDescending time.")]
    public static IEnumerable<string> ByCuisineThenTime() => Recipes.OrderBy(r => r.Cuisine).ThenByDescending(r => r.Minutes).Select(r => r.Name);
}
