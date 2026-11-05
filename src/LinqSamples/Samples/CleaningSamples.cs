namespace LinqSamples.Samples;

/// <summary>Cleaning, parsing and validating messy input with LINQ.</summary>
[SampleCategory("Data cleaning & validation", 41)]
public static class CleaningSamples
{
    private static readonly string?[] Raw = [" Ada ", "bob", "", "  ", "Cy", "ada", null, "Di"];
    private static readonly string[] Numbers = ["1", "x", "3", "", "4.5", "7"];

    private static IEnumerable<string> Cleaned() =>
        Raw.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s!.Trim());

    [Sample("Trim and drop blanks", Description = "Filter null/whitespace first, then Trim.")]
    public static IEnumerable<string> TrimNonEmpty() => Cleaned();

    [Sample("Case-insensitive distinct", Description = "Distinct with StringComparer.OrdinalIgnoreCase keeps the first spelling.")]
    public static IEnumerable<string> DistinctIgnoreCase() => Cleaned().Distinct(StringComparer.OrdinalIgnoreCase);

    [Sample("Normalize to title case", Description = "Normalize each value, then Distinct.")]
    public static IEnumerable<string> TitleCase() =>
        Cleaned().Select(s => char.ToUpperInvariant(s[0]) + s[1..].ToLowerInvariant()).Distinct();

    [Sample("Count invalid entries", Description = "Count with IsNullOrWhiteSpace as the predicate.")]
    public static int InvalidCount() => Raw.Count(string.IsNullOrWhiteSpace);

    [Sample("Replace nulls with a default", Description = "Null-conditional Trim plus ?? keeps every position.")]
    public static string WithDefault() => string.Join("|", Raw.Select(s => s?.Trim() ?? "n/a"));

    [Sample("Parse safely", Description = "SelectMany returns one item or none, so failures vanish without exceptions.")]
    public static IEnumerable<int> SafeParse() =>
        Numbers.SelectMany(s => int.TryParse(s, out var n) ? new[] { n } : []);

    [Sample("Report unparseable inputs", Description = "Select with index, then keep the failures.")]
    public static IEnumerable<string> BadInputs() =>
        Numbers.Select((s, i) => (s, i)).Where(x => !int.TryParse(x.s, out _)).Select(x => $"{x.i}:{x.s}");

    [Sample("All inputs valid?", Description = "All short-circuits on the first failure.")]
    public static string AllValid() => $"{Numbers.All(s => int.TryParse(s, out _))}";

    [Sample("Find duplicate ids", Description = "GroupBy then keep groups with more than one item.")]
    public static IEnumerable<int> DuplicateIds() =>
        new[] { 1, 2, 2, 3, 3, 3 }.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key);

    [Sample("Ids that appear once", Description = "GroupBy then keep groups of exactly one.")]
    public static IEnumerable<int> UniqueIds() =>
        new[] { 1, 2, 2, 3, 3, 3 }.GroupBy(x => x).Where(g => g.Count() == 1).Select(g => g.Key);

    [Sample("Find missing numbers", Description = "Range over min..max, then Except the present values.")]
    public static IEnumerable<int> MissingNumbers()
    {
        int[] ids = [1, 2, 4, 7];
        return Enumerable.Range(ids.Min(), ids.Max() - ids.Min() + 1).Except(ids);
    }

    [Sample("Clamp out-of-range values", Description = "Math.Clamp inside Select.")]
    public static IEnumerable<int> Clamped() => new[] { -5, 3, 12, 8 }.Select(x => Math.Clamp(x, 0, 10));

    [Sample("Collapse repeated spaces", Description = "Split with RemoveEmptyEntries, then Join with one space.")]
    public static string CollapseSpaces() => string.Join(" ", "a   b  c".Split(' ', StringSplitOptions.RemoveEmptyEntries));

    [Sample("Keep digits only", Description = "A string is a sequence of chars, so Where(char.IsDigit) filters it.")]
    public static string DigitsOnly() => new("a1b2c3".Where(char.IsDigit).ToArray());

    [Sample("Last valid entry", Description = "LastOrDefault with the validity predicate.")]
    public static string? LastValid() => Raw.LastOrDefault(s => !string.IsNullOrWhiteSpace(s));

    [Sample("Sum of valid numbers", Description = "TryParse inside Select, keep successes, then Sum.")]
    public static int ValidSum() =>
        Numbers.Select(s => int.TryParse(s, out var n) ? (int?)n : null).Where(n => n.HasValue).Sum(n => n!.Value);

    [Sample("Longest cleaned name", Description = "MaxBy on the cleaned values.")]
    public static string? LongestName() => Cleaned().MaxBy(s => s.Length);

    [Sample("Sorted unique names", Description = "Distinct ignoring case, then Order ignoring case.")]
    public static IEnumerable<string> SortedUnique() =>
        Cleaned().Distinct(StringComparer.OrdinalIgnoreCase).Order(StringComparer.OrdinalIgnoreCase);

    [Sample("Initials", Description = "Select the first char of each distinct cleaned value.")]
    public static string Initials() =>
        new(Cleaned().Distinct(StringComparer.OrdinalIgnoreCase).Select(s => char.ToUpperInvariant(s[0])).ToArray());

    [Sample("Collapse whitespace", Description = "Split on spaces dropping empties, then Join with a single space.")]
    public static string CollapseSingleSpaces() => string.Join(" ", "  a   b  c ".Split(" ", StringSplitOptions.RemoveEmptyEntries));

    [Sample("Strip phone formatting", Description = "Where(char.IsDigit) strips formatting from a phone number.")]
    public static string PhoneDigits() => new(" (555) 123-4567".Where(char.IsDigit).ToArray());

    [Sample("Nulls to n/a", Description = "Select with the null-coalescing operator.")]
    public static IEnumerable<string> NullsToDefault() => new string?[] { "a", null, "c" }.Select(s => s ?? "n/a");
}
