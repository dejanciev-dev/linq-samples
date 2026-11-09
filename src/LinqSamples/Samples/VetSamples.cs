namespace LinqSamples.Samples;

/// <summary>Veterinary visits: pets, species, vets and costs.</summary>
[SampleCategory("Vet visits", 63)]
public static class VetSamples
{
    private sealed record Visit(string Pet, string Species, string Vet, int Cost);

    private static readonly Visit[] Visits =
    [
        new("Rex", "Dog", "Kim", 50), new("Tom", "Cat", "Lee", 30), new("Rex", "Dog", "Lee", 70),
        new("Fido", "Dog", "Kim", 40), new("Kit", "Cat", "Kim", 60), new("Tom", "Cat", "Kim", 20),
    ];

    [Sample("Total cost", Description = "Sum of all visit costs.")]
    public static int TotalCost() => Visits.Sum(v => v.Cost);

    [Sample("Cost per species", Description = "Group by species and sum cost.")]
    public static IEnumerable<string> CostPerSpecies() => Visits.GroupBy(v => v.Species).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Sum(v => v.Cost)}");

    [Sample("Repeat pets", Description = "Pets with more than one visit.")]
    public static IEnumerable<string> RepeatPets() => Visits.CountBy(v => v.Pet).Where(c => c.Value > 1).Select(c => c.Key).Order();

    [Sample("Top spender", Description = "MaxBy over grouped cost.")]
    public static string TopSpender() => Visits.GroupBy(v => v.Pet).MaxBy(g => g.Sum(v => v.Cost))!.Key;

    [Sample("Vets", Description = "Distinct vet names.")]
    public static IEnumerable<string> Vets() => Visits.Select(v => v.Vet).Distinct().Order();

    [Sample("Average cost", Description = "Average visit cost.")]
    public static double AverageCost() => Visits.Average(v => v.Cost);

    [Sample("Visits per vet", Description = "CountBy vet.")]
    public static IEnumerable<string> VisitsPerVet() => Visits.CountBy(v => v.Vet).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Seen by both vets", Description = "Pets treated by more than one vet.")]
    public static IEnumerable<string> SeenByBoth() =>
        Visits.GroupBy(v => v.Pet).Where(g => g.Select(v => v.Vet).Distinct().Count() > 1).Select(g => g.Key).Order();

    [Sample("Expensive visits", Description = "Count visits costing 50 or more.")]
    public static int ExpensiveVisits() => Visits.Count(v => v.Cost >= 50);

    [Sample("Cheapest visit", Description = "MinBy cost.")]
    public static string Cheapest() => Visits.MinBy(v => v.Cost)!.Pet;
}
