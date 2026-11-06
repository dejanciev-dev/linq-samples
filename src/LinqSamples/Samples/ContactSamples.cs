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

    [Sample("Youngest contact", Description = "MinBy age.")]
    public static string Youngest() => Book.MinBy(c => c.Age)!.Name;

    [Sample("Average age", Description = "Average over all contacts.")]
    public static double AverageAge() => Book.Average(c => c.Age);

    [Sample("Names per city", Description = "Group by city and join the names.")]
    public static IEnumerable<string> NamesPerCity() =>
        Book.GroupBy(c => c.City).OrderBy(g => g.Key).Select(g => $"{g.Key}:{string.Join(",", g.Select(c => c.Name))}");

    [Sample("Over thirty", Description = "Filter by age.")]
    public static IEnumerable<string> OverThirty() => Book.Where(c => c.Age > 30).Select(c => c.Name);

    [Sample("City of the oldest", Description = "MaxBy then read another property.")]
    public static string CityOfOldest() => Book.MaxBy(c => c.Age)!.City;

    [Sample("Distinct phone prefixes", Description = "Select a prefix, Distinct, Count.")]
    public static int PhonePrefixes() => Book.Select(c => c.Phone[..3]).Distinct().Count();

    [Sample("Unique phone owners", Description = "Groups of one, flattened with SelectMany.")]
    public static IEnumerable<string> UniquePhoneOwners() =>
        Book.GroupBy(c => c.Phone).Where(g => g.Count() == 1).SelectMany(g => g).Select(c => c.Name);

    [Sample("Second page", Description = "Skip and Take for paging.")]
    public static IEnumerable<string> SecondPage() => Book.Skip(2).Take(2).Select(c => c.Name);

    [Sample("Cities with only 30+", Description = "GroupBy with All.")]
    public static IEnumerable<string> AllThirtyPlus() => Book.GroupBy(c => c.City).Where(g => g.All(c => c.Age >= 30)).Select(g => g.Key);

    [Sample("Age spread per city", Description = "Max minus Min inside each group.")]
    public static IEnumerable<string> AgeSpread() =>
        Book.GroupBy(c => c.City).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Max(c => c.Age) - g.Min(c => c.Age)}");
}
