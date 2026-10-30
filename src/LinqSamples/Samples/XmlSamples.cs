using System.Globalization;
using System.Xml.Linq;

namespace LinqSamples.Samples;

/// <summary>
/// LINQ to XML (System.Xml.Linq): query, transform and build XML with the same operators.
/// Explicit casts such as <c>(decimal)element</c> parse values culture-invariantly.
/// </summary>
[SampleCategory("LINQ to XML", 21)]
public static class XmlSamples
{
    private const string LibraryXml = """
        <library>
          <book id="b1" year="2019" available="true">
            <title>LINQ Pocket Guide</title>
            <subtitle>A Field Manual</subtitle>
            <author>M. Rivera</author>
            <genre>Programming</genre>
            <price>29.50</price>
            <tags><tag>linq</tag><tag>csharp</tag></tags>
          </book>
          <book id="b2" year="2021" available="false">
            <title>Async Patterns</title>
            <author>S. Kim</author>
            <genre>Programming</genre>
            <price>45.00</price>
            <tags><tag>async</tag><tag>csharp</tag></tags>
          </book>
          <book id="b3" year="2015" available="true">
            <title>The Quiet Garden</title>
            <author>L. Novak</author>
            <genre>Fiction</genre>
            <price>12.99</price>
            <tags><tag>novel</tag></tags>
          </book>
          <book id="b4" year="2023" available="true">
            <title>Data at Scale</title>
            <author>M. Rivera</author>
            <genre>Programming</genre>
            <price>52.00</price>
            <tags><tag>data</tag><tag>sql</tag></tags>
          </book>
          <book id="b5" year="2018" available="true">
            <title>Mountain Trails</title>
            <author>A. Petrov</author>
            <genre>Travel</genre>
            <price>18.75</price>
            <tags><tag>hiking</tag><tag>outdoors</tag></tags>
          </book>
        </library>
        """;

    [Sample("Elements", Description = "Parse a document and read child elements.")]
    public static IEnumerable<string> Elements() =>
        Books().Select(b => Text(b, "title"));

    [Sample("Filter by attribute", Description = "Cast an attribute to int and compare.")]
    public static IEnumerable<string> FilterByAttribute() =>
        Books().Where(b => (int)b.Attribute("year")! >= 2020).Select(b => Text(b, "title"));

    [Sample("Descendants", Description = "Find elements at any depth.")]
    public static IEnumerable<string> Descendants() =>
        Load().Descendants("tag").Select(t => t.Value).Distinct();

    [Sample("Typed values", Description = "(decimal)element parses the text invariantly - sum all prices.")]
    public static decimal TypedValues() =>
        Books().Sum(b => (decimal)b.Element("price")!);

    [Sample("Optional elements", Description = "Cast to a nullable type: a missing element becomes null instead of an exception.")]
    public static IEnumerable<string> OptionalElements() =>
        Books().Take(2).Select(b => $"{Text(b, "title")}: {(string?)b.Element("subtitle") ?? "(none)"}");

    [Sample("Group by element value", Description = "Count books per genre.")]
    public static IEnumerable<string> GroupByGenre() =>
        Books().GroupBy(b => Text(b, "genre")).Select(g => $"{g.Key}: {g.Count()}");

    [Sample("Order by attribute", Description = "Newest books first.")]
    public static IEnumerable<string> OrderByAttribute() =>
        Books().OrderByDescending(b => (int)b.Attribute("year")!)
               .Select(b => $"{b.Attribute("year")!.Value} {Text(b, "title")}");

    [Sample("Authors with several books", Description = "Group, filter groups, join the titles.")]
    public static IEnumerable<string> AuthorsWithSeveralBooks() =>
        Books().GroupBy(b => Text(b, "author"))
               .Where(g => g.Count() > 1)
               .Select(g => $"{g.Key}: {string.Join(", ", g.Select(b => Text(b, "title")))}");

    [Sample("Ancestors", Description = "Walk up the tree from a match to its containing book.")]
    public static string Ancestors()
    {
        var tag = Load().Descendants("tag").First(t => t.Value == "sql");
        return Text(tag.Ancestors("book").First(), "title");
    }

    [Sample("Attributes", Description = "Enumerate the attributes of an element.")]
    public static IEnumerable<string> Attributes() =>
        Books().First().Attributes().Select(a => $"{a.Name}={a.Value}");

    [Sample("Functional construction", Description = "Build XML from a query by nesting XElement constructors.")]
    public static IEnumerable<string> FunctionalConstruction()
    {
        var xml = new XElement("customers",
            Customers.Where(c => c.City == "Berlin")
                     .Select(c => new XElement("customer",
                         new XAttribute("id", c.Id),
                         new XAttribute("city", c.City),
                         c.Name)));
        return Lines(xml);
    }

    [Sample("XML to objects", Description = "Project elements into typed tuples, then query them like any data.")]
    public static IEnumerable<string> XmlToObjects() =>
        Books().Select(b => (Title: Text(b, "title"), Price: (decimal)b.Element("price")!))
               .OrderBy(b => b.Price)
               .Take(2)
               .Select(b => FormattableString.Invariant($"{b.Title}: {b.Price}"));

    [Sample("XML to XML", Description = "Transform one shape into another.")]
    public static IEnumerable<string> XmlToXml()
    {
        var catalog = new XElement("catalog",
            Books().Where(b => Text(b, "genre") != "Programming")
                   .Select(b => new XElement("item",
                       new XAttribute("title", Text(b, "title")),
                       new XAttribute("genre", Text(b, "genre")))));
        return Lines(catalog);
    }

    [Sample("Namespaces", Description = "Element names include their namespace - query with an XNamespace.")]
    public static IEnumerable<string> Namespaces()
    {
        var feed = XDocument.Parse("""
            <feed xmlns="http://www.w3.org/2005/Atom">
              <entry><title>First</title></entry>
              <entry><title>Second</title></entry>
            </feed>
            """);
        XNamespace atom = "http://www.w3.org/2005/Atom";
        return
        [
            $"Without namespace: {feed.Root!.Elements("entry").Count()} entries",
            $"With namespace:    {string.Join(", ", feed.Root.Elements(atom + "entry").Select(e => e.Element(atom + "title")!.Value))}",
        ];
    }

    [Sample("Modify XML", Description = "Remove matching elements and set attributes in place.")]
    public static IEnumerable<string> ModifyXml()
    {
        var doc = Load();
        doc.Root!.Elements("book").Where(b => !(bool)b.Attribute("available")!).Remove();

        var programming = doc.Root.Elements("book").Where(b => Text(b, "genre") == "Programming").ToList();
        programming.ForEach(b => b.SetAttributeValue("discount", "10%"));

        return
        [
            $"Books left: {doc.Root.Elements("book").Count()}",
            $"Discounted: {string.Join(", ", doc.Root.Elements("book").Where(b => b.Attribute("discount") is not null).Select(b => Text(b, "title")))}",
        ];
    }

    [Sample("Join XML with objects", Description = "Join elements to in-memory data on a shared key.")]
    public static IEnumerable<string> JoinWithObjects()
    {
        (string Author, string Country)[] authors = [("M. Rivera", "Spain"), ("S. Kim", "Korea"), ("L. Novak", "Czechia")];
        return Books().Join(authors, b => Text(b, "author"), a => a.Author, (b, a) => $"{Text(b, "title")}: {a.Country}");
    }

    [Sample("Query syntax over XML", Description = "let keeps the parsed price for both where and orderby.")]
    public static IEnumerable<string> QuerySyntax() =>
        from b in Books()
        let price = (decimal)b.Element("price")!
        where price > 20m
        orderby price
        select Text(b, "title");

    [Sample("Average per group", Description = "Aggregate typed values within each group.")]
    public static IEnumerable<string> AveragePerGroup() =>
        Books().GroupBy(b => Text(b, "genre"))
               .Select(g => FormattableString.Invariant($"{g.Key}: {Math.Round(g.Average(b => (decimal)b.Element("price")!), 2)}"));

    [Sample("Chained Elements()", Description = "Elements() works on sequences too - an XPath-like path without strings.")]
    public static IEnumerable<string> ChainedElements()
    {
        var doc = Load();
        return
        [
            $"Elements chain: {doc.Root!.Elements("book").Elements("tags").Elements("tag").Count()}",
            $"Descendants:    {doc.Descendants("tag").Count()}",
        ];
    }

    [Sample("Grouped XML", Description = "Nest a GroupBy inside functional construction.")]
    public static IEnumerable<string> GroupedXml()
    {
        var xml = new XElement("cities",
            Customers.GroupBy(c => c.City)
                     .Select(g => new XElement("city",
                         new XAttribute("name", g.Key),
                         g.Select(c => new XElement("customer", c.Name)))));
        return Lines(xml);
    }

    private static XDocument Load() => XDocument.Parse(LibraryXml);

    private static IEnumerable<XElement> Books() => Load().Root!.Elements("book");

    private static string Text(XElement book, string name) => book.Element(name)?.Value ?? string.Empty;

    private static IEnumerable<string> Lines(XElement xml) =>
        xml.ToString().Split('\n').Select(line => line.TrimEnd('\r'));
}
