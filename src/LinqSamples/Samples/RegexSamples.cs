using System.Globalization;
using System.Text.RegularExpressions;

namespace LinqSamples.Samples;

/// <summary>
/// <c>MatchCollection</c> is an <c>IEnumerable&lt;Match&gt;</c>, so regex results flow straight into LINQ.
/// The patterns use source-generated regexes (<c>[GeneratedRegex]</c>).
/// </summary>
[SampleCategory("Regex + LINQ", 17)]
public static partial class RegexSamples
{
    [Sample("Extract emails", Description = "Matches piped into Select.")]
    public static IEnumerable<string> ExtractEmails()
    {
        const string text = "Contact alice@example.com or bob@test.org, not bob@ or @x.";
        return EmailPattern().Matches(text).Select(m => m.Value);
    }

    [Sample("Sum numbers in text", Description = "Find every number, parse it, add them up.")]
    public static int SumNumbers()
    {
        const string text = "3 apples, 12 pears and 7 plums";
        return NumberPattern().Matches(text).Sum(m => int.Parse(m.Value, CultureInfo.InvariantCulture));
    }

    [Sample("Named groups into tuples", Description = "Parse log lines into typed fields, then filter.")]
    public static IEnumerable<string> NamedGroups()
    {
        string[] log = ["2026-01-05 ERROR disk full", "2026-01-06 INFO ok", "2026-01-07 ERROR timeout"];
        return log.Select(line => LogPattern().Match(line))
                  .Where(m => m.Success)
                  .Select(m => (Date: m.Groups["date"].Value, Level: m.Groups["level"].Value, Message: m.Groups["message"].Value))
                  .Where(e => e.Level == "ERROR")
                  .Select(e => $"{e.Date}: {e.Message}");
    }

    [Sample("Validate input", Description = "IsMatch as a predicate.")]
    public static IEnumerable<string> ValidateInput()
    {
        string[] codes = ["SK-1234", "sk-12", "BE-0001", "XX-99999"];
        return codes.Select(c => $"{c}: {(CodePattern().IsMatch(c) ? "valid" : "invalid")}");
    }

    [Sample("Split camelCase", Description = "Break an identifier into its words.")]
    public static IEnumerable<string> SplitCamelCase() =>
        CamelCaseWordPattern().Matches("parseHttpRequestHeader").Select(m => m.Value);

    [Sample("Hashtags", Description = "Capture groups, normalized and de-duplicated.")]
    public static IEnumerable<string> Hashtags()
    {
        const string post = "Learning #linq and #dotnet with #LINQ";
        return HashtagPattern().Matches(post)
                               .Select(m => m.Groups[1].Value.ToLowerInvariant())
                               .Distinct();
    }

    [Sample("Key=value pairs to a dictionary", Description = "Parse a connection-string-like text with ToDictionary.")]
    public static IEnumerable<string> KeyValuePairs()
    {
        const string settings = "host=localhost; port=5432; db=shop";
        var dictionary = KeyValuePattern().Matches(settings)
                                          .ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value.Trim());
        return dictionary.Select(kv => $"{kv.Key}: {kv.Value}");
    }

    [Sample("Repeated words", Description = "A backreference finds accidental doubles like \"the the\".")]
    public static IEnumerable<string> RepeatedWords()
    {
        const string text = "this is is a test test of the the code";
        return RepeatedWordPattern().Matches(text).Select(m => m.Groups[1].Value);
    }

    [GeneratedRegex(@"[\w.+-]+@[\w-]+\.[\w.]+")]
    private static partial Regex EmailPattern();

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberPattern();

    [GeneratedRegex(@"^(?<date>\d{4}-\d{2}-\d{2}) (?<level>[A-Z]+) (?<message>.+)$")]
    private static partial Regex LogPattern();

    [GeneratedRegex(@"^[A-Z]{2}-\d{4}$")]
    private static partial Regex CodePattern();

    [GeneratedRegex("[A-Z]?[a-z]+")]
    private static partial Regex CamelCaseWordPattern();

    [GeneratedRegex(@"#(\w+)")]
    private static partial Regex HashtagPattern();

    [GeneratedRegex(@"(\w+)=([^;]+)")]
    private static partial Regex KeyValuePattern();

    [GeneratedRegex(@"\b(\w+)\s+\1\b")]
    private static partial Regex RepeatedWordPattern();
}
