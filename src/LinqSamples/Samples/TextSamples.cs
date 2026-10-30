using System.Globalization;
using System.Text;

namespace LinqSamples.Samples;

/// <summary>A string is an <c>IEnumerable&lt;char&gt;</c>, so every LINQ operator works on text.</summary>
[SampleCategory("Strings & text", 16)]
public static class TextSamples
{
    [Sample("Reverse words", Description = "Split, reverse, join.")]
    public static string ReverseWords()
    {
        const string sentence = "LINQ makes code readable";
        // AsEnumerable: on an array, C# 14 would bind Reverse() to the in-place MemoryExtensions.Reverse.
        return string.Join(" ", sentence.Split(' ').AsEnumerable().Reverse());
    }

    [Sample("Capitalize each word", Description = "Upper-case the first character of every word.")]
    public static string CapitalizeWords()
    {
        const string text = "hello linq world";
        return string.Join(" ", text.Split(' ').Select(w => char.ToUpperInvariant(w[0]) + w[1..]));
    }

    [Sample("Count vowels", Description = "Count with a predicate over the characters of a string.")]
    public static int CountVowels()
    {
        const string text = "Language Integrated Query";
        return text.Count(c => "aeiou".Contains(char.ToLowerInvariant(c)));
    }

    [Sample("Character frequency", Description = "CountBy on characters, most frequent first.")]
    public static IEnumerable<string> CharacterFrequency() =>
        "mississippi".CountBy(c => c)
                     .OrderByDescending(kv => kv.Value)
                     .ThenBy(kv => kv.Key)
                     .Select(kv => $"{kv.Key}: {kv.Value}");

    [Sample("Longest word", Description = "MaxBy with the length as the key.")]
    public static string LongestWord() =>
        Words.MaxBy(w => w.Length)!;

    [Sample("Acronym", Description = "First letter of each word, upper-cased.")]
    public static string Acronym()
    {
        const string name = "portable network graphics";
        return string.Concat(name.Split(' ').Select(w => char.ToUpperInvariant(w[0])));
    }

    [Sample("Pangram check", Description = "Does the text use every letter of the alphabet?")]
    public static IEnumerable<string> Pangram()
    {
        static bool IsPangram(string s) =>
            s.ToLowerInvariant().Where(char.IsAsciiLetterLower).Distinct().Count() == 26;

        string[] texts = ["The quick brown fox jumps over the lazy dog", "Hello world"];
        return texts.Select(t => $"{t}: {IsPangram(t)}");
    }

    [Sample("Parse CSV", Description = "Skip the header, split each line and map the fields to records.")]
    public static IEnumerable<string> ParseCsv()
    {
        const string csv = "Id,Name,City\n1,Alice,Skopje\n2,Bob,Berlin";
        return csv.Split('\n')
                  .Skip(1)
                  .Select(line => line.Split(','))
                  .Select(f => new Customer(int.Parse(f[0], CultureInfo.InvariantCulture), f[1], f[2]))
                  .Select(c => $"#{c.Id} {c.Name} ({c.City})");
    }

    [Sample("Slugify", Description = "Turn a title into a URL-friendly slug.")]
    public static string Slugify()
    {
        const string title = "LINQ in .NET 10: What's New?";
        var cleaned = string.Concat(title.Select(c => char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : ' '));
        return string.Join("-", cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    [Sample("Remove accents", Description = "Decompose characters, then drop the combining marks.")]
    public static string RemoveAccents()
    {
        const string text = "Crème Brûlée à Skopje";
        return string.Concat(text.Normalize(NormalizationForm.FormD)
                                 .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
    }

    [Sample("Words by first letter", Description = "Group words case-insensitively by their first letter.")]
    public static IEnumerable<string> WordsByFirstLetter() =>
        Words.GroupBy(w => char.ToLowerInvariant(w[0]))
             .OrderBy(g => g.Key)
             .Select(g => $"{g.Key}: {string.Join(", ", g)}");

    [Sample("Join with \"and\"", Description = "A human-friendly list: commas, then \"and\" before the last item.")]
    public static string JoinWithAnd()
    {
        var names = Customers.Select(c => c.Name).ToList();
        return names.Count <= 1
            ? string.Concat(names)
            : $"{string.Join(", ", names.SkipLast(1))} and {names[^1]}";
    }

    [Sample("Collapse whitespace", Description = "Split with RemoveEmptyEntries and join with a single space.")]
    public static string CollapseWhitespace()
    {
        const string text = "too   many    spaces  here";
        return string.Join(" ", text.Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    [Sample("Sort by last name", Description = "Order by a part of the string.")]
    public static IEnumerable<string> SortByLastName()
    {
        string[] people = ["Ada Lovelace", "Alan Turing", "Grace Hopper", "Linus Torvalds"];
        return people.OrderBy(p => p.Split(' ')[^1], StringComparer.Ordinal);
    }

    [Sample("Filter log lines", Description = "Keep lines with a given prefix and strip it.")]
    public static IEnumerable<string> FilterLogLines()
    {
        string[] log = ["INFO start", "ERROR disk full", "WARN low memory", "ERROR timeout"];
        return log.Where(line => line.StartsWith("ERROR ", StringComparison.Ordinal))
                  .Select(line => line["ERROR ".Length..]);
    }
}
