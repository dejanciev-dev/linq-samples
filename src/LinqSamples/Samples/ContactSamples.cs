namespace LinqSamples.Samples;

/// <summary>Address-book clean-up and lookups.</summary>
[SampleCategory("Contacts", 56)]
public static class ContactSamples
{
    private sealed record Contact(string Name, string Phone, string City, int Age);

    private static readonly Contact[] Book =
    [
        new("Ann", "555-1111", "Oslo", 30), new("Bob", "555-2222", "Rome", 25), new("Cy", "555-1111", "Oslo", 41),
        new("Di", "555-3333", "Paris", 25), new("Ed", "555-4444", "Rome", 35),
    ];

    [Sample("Duplicate phone numbers", Description = "Groups with more than one contact.")]
    public static IEnumerable<string> DuplicatePhones() => Book.GroupBy(c => c.Phone).Where(g => g.Count() > 1).Select(g => g.Key);

    [Sample("Distinct cities", Description = "Distinct then sorted.")]
    public static IEnumerable<string> Cities() => Book.Select(c => c.City).Distinct().Order();

    [Sample("Oldest contact", Description = "MaxBy age.")]
    public static string Oldest() => Book.MaxBy(c => c.Age)!.Name;

    [Sample("Same age", Description = "Group by age and keep shared ages.")]
    public static IEnumerable<string> SameAge() =>
        Book.GroupBy(c => c.Age).Where(g => g.Count() > 1).Select(g => $"{g.Key}:{string.Join(",", g.Select(c => c.Name))}");

    [Sample("Contacts per city", Description = "CountBy city.")]
    public static IEnumerable<string> PerCity() => Book.CountBy(c => c.City).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Average age in Rome", Description = "Filter then Average.")]
    public static double RomeAverageAge() => Book.Where(c => c.City == "Rome").Average(c => c.Age);

    [Sample("Sorted by city then name", Description = "OrderBy then ThenBy.")]
    public static IEnumerable<string> ByCityThenName() => Book.OrderBy(c => c.City).ThenBy(c => c.Name).Select(c => c.Name);

    [Sample("Initials", Description = "Concat the first character of each name.")]
    public static string Initials() => string.Concat(Book.Select(c => c.Name[0]));

    [Sample("Lookup with missing key", Description = "A Lookup returns an empty group instead of throwing.")]
    public static int MissingCity() => Book.ToLookup(c => c.City)["Lima"].Count();

    [Sample("Age decades", Description = "CountBy age rounded down to ten.")]
    public static IEnumerable<string> AgeDecades() => Book.CountBy(c => c.Age / 10 * 10).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");
}
