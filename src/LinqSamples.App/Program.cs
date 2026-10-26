using System.Globalization;
using LinqSamples.Infrastructure;

// Usage:
//   dotnet run                 -> run every sample
//   dotnet run -- --list       -> list categories and samples
//   dotnet run -- joins        -> run samples whose category or title contains "joins"

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
var categories = SampleCatalog.Categories;

if (args is ["--list" or "-l"])
{
    foreach (var category in categories)
    {
        Console.WriteLine(category.Name);
        foreach (var sample in category.Samples)
        {
            Console.WriteLine($"  - {sample.Title}");
        }
    }

    return 0;
}

var filter = string.Join(' ', args);
var selected = categories
    .SelectMany(c => c.Samples)
    .Where(s => filter.Length == 0
                || s.Category.Contains(filter, StringComparison.OrdinalIgnoreCase)
                || s.Title.Contains(filter, StringComparison.OrdinalIgnoreCase))
    .ToList();

if (selected.Count == 0)
{
    Console.Error.WriteLine($"No samples match '{filter}'. Use --list to see what is available.");
    return 1;
}

foreach (var categoryGroup in selected.GroupBy(s => s.Category))
{
    WriteColored($"=== {categoryGroup.Key} ===", ConsoleColor.Cyan);
    Console.WriteLine();

    foreach (var sample in categoryGroup)
    {
        WriteColored($"> {sample.Title}", ConsoleColor.Yellow);
        if (sample.Description is { } description)
        {
            WriteColored($"  {description}", ConsoleColor.DarkGray);
        }

        foreach (var line in ResultFormatter.Format(sample.Run()).Split(Environment.NewLine))
        {
            Console.WriteLine($"  {line}");
        }

        Console.WriteLine();
    }
}

return 0;

static void WriteColored(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}
