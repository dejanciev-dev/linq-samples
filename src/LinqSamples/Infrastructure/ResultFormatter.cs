using System.Collections;

namespace LinqSamples.Infrastructure;

/// <summary>Turns sample results (scalars, sequences, nested sequences) into readable text.</summary>
public static class ResultFormatter
{
    public static string Format(object? value) => value switch
    {
        null => "null",
        string s => s,
        IEnumerable sequence => string.Join(Environment.NewLine, sequence.Cast<object?>().Select(FormatItem)),
        _ => FormatItem(value),
    };

    private static string FormatItem(object? item) => item switch
    {
        null => "null",
        string s => s,
        decimal d => d.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture),
        IEnumerable nested => $"[{string.Join(", ", nested.Cast<object?>().Select(FormatItem))}]",
        _ => item.ToString() ?? string.Empty,
    };
}
