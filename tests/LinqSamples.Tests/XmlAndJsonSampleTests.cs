namespace LinqSamples.Tests;

/// <summary>Pins the results of the LINQ to XML and LINQ over JSON samples.</summary>
public class XmlAndJsonSampleTests
{
    // LINQ to XML

    [Fact]
    public void Elements_read_titles() =>
        Assert.Equal(["LINQ Pocket Guide", "Async Patterns", "The Quiet Garden", "Data at Scale", "Mountain Trails"], XmlSamples.Elements());

    [Fact]
    public void Filter_by_attribute() =>
        Assert.Equal(["Async Patterns", "Data at Scale"], XmlSamples.FilterByAttribute());

    [Fact]
    public void Descendants_find_distinct_tags() =>
        Assert.Equal(["linq", "csharp", "async", "novel", "data", "sql", "hiking", "outdoors"], XmlSamples.Descendants());

    [Fact]
    public void Typed_values_sum_prices() =>
        Assert.Equal(158.24m, XmlSamples.TypedValues());

    [Fact]
    public void Optional_elements_become_null() =>
        Assert.Equal(["LINQ Pocket Guide: A Field Manual", "Async Patterns: (none)"], XmlSamples.OptionalElements());

    [Fact]
    public void Group_by_genre() =>
        Assert.Equal(["Programming: 3", "Fiction: 1", "Travel: 1"], XmlSamples.GroupByGenre());

    [Fact]
    public void Order_by_year_descending() =>
        Assert.Equal(
            ["2023 Data at Scale", "2021 Async Patterns", "2019 LINQ Pocket Guide", "2018 Mountain Trails", "2015 The Quiet Garden"],
            XmlSamples.OrderByAttribute());

    [Fact]
    public void Authors_with_several_books() =>
        Assert.Equal(["M. Rivera: LINQ Pocket Guide, Data at Scale"], XmlSamples.AuthorsWithSeveralBooks());

    [Fact]
    public void Ancestors_find_the_book() =>
        Assert.Equal("Data at Scale", XmlSamples.Ancestors());

    [Fact]
    public void Attributes_of_first_book() =>
        Assert.Equal(["id=b1", "year=2019", "available=true"], XmlSamples.Attributes());

    [Fact]
    public void Functional_construction_builds_xml() =>
        Assert.Equal(
            ["<customers>", "  <customer id=\"2\" city=\"Berlin\">Bob</customer>", "  <customer id=\"5\" city=\"Berlin\">Eva</customer>", "</customers>"],
            XmlSamples.FunctionalConstruction());

    [Fact]
    public void Xml_to_objects_finds_cheapest_books() =>
        Assert.Equal(["The Quiet Garden: 12.99", "Mountain Trails: 18.75"], XmlSamples.XmlToObjects());

    [Fact]
    public void Xml_to_xml_transforms_shape() =>
        Assert.Equal(
            ["<catalog>", "  <item title=\"The Quiet Garden\" genre=\"Fiction\" />", "  <item title=\"Mountain Trails\" genre=\"Travel\" />", "</catalog>"],
            XmlSamples.XmlToXml());

    [Fact]
    public void Namespaces_are_part_of_the_name() =>
        Assert.Equal(["Without namespace: 0 entries", "With namespace:    First, Second"], XmlSamples.Namespaces());

    [Fact]
    public void Modify_xml_removes_and_sets() =>
        Assert.Equal(["Books left: 4", "Discounted: LINQ Pocket Guide, Data at Scale"], XmlSamples.ModifyXml());

    [Fact]
    public void Join_xml_with_objects() =>
        Assert.Equal(
            ["LINQ Pocket Guide: Spain", "Async Patterns: Korea", "The Quiet Garden: Czechia", "Data at Scale: Spain"],
            XmlSamples.JoinWithObjects());

    [Fact]
    public void Query_syntax_over_xml() =>
        Assert.Equal(["LINQ Pocket Guide", "Async Patterns", "Data at Scale"], XmlSamples.QuerySyntax());

    [Fact]
    public void Average_price_per_genre() =>
        Assert.Equal(["Programming: 42.17", "Fiction: 12.99", "Travel: 18.75"], XmlSamples.AveragePerGroup());

    [Fact]
    public void Chained_elements_match_descendants() =>
        Assert.Equal(["Elements chain: 9", "Descendants:    9"], XmlSamples.ChainedElements());

    [Fact]
    public void Grouped_xml_nests_customers_by_city()
    {
        var lines = XmlSamples.GroupedXml().ToList();
        Assert.Equal("<cities>", lines[0]);
        Assert.Equal("  <city name=\"Skopje\">", lines[1]);
        Assert.Equal(3, lines.Count(l => l.Contains("<city ", StringComparison.Ordinal)));
        Assert.Equal(5, lines.Count(l => l.Contains("<customer>", StringComparison.Ordinal)));
    }

    // LINQ over JSON

    [Fact]
    public void Query_json_node() =>
        Assert.Equal(["Alice", "Bob", "Carla"], JsonSamples.QueryJsonNode());

    [Fact]
    public void Filter_json_array() =>
        Assert.Equal([1, 3], JsonSamples.FilterArray());

    [Fact]
    public void Flatten_nested_arrays() =>
        Assert.Equal(["1: LAP x1", "1: MOU x2", "2: DSK x1", "3: PEN x10"], JsonSamples.FlattenNested());

    [Fact]
    public void Sum_nested_values() =>
        Assert.Equal(1265m, JsonSamples.SumNested());

    [Fact]
    public void Optional_properties() =>
        Assert.Equal(["#1: (none)", "#2: (none)", "#3: SPRING", "#4: (none)"], JsonSamples.OptionalProperties());

    [Fact]
    public void Group_by_property() =>
        Assert.Equal(["Alice: 2", "Bob: 1", "Carla: 1"], JsonSamples.GroupByProperty());

    [Fact]
    public void Empty_arrays() =>
        Assert.Equal([4], JsonSamples.EmptyArrays());

    [Fact]
    public void Deserialize_then_query() =>
        Assert.Equal("Downtown: biggest order is #1 by Alice (1250)", JsonSamples.DeserializeThenQuery());

    [Fact]
    public void Serialize_query_result() =>
        Assert.Equal("""[{"id":2,"name":"Bob"},{"id":5,"name":"Eva"}]""", JsonSamples.SerializeQuery());

    [Fact]
    public void Build_json_with_nodes() =>
        Assert.Equal("""[{"name":"Desk","price":300},{"name":"Chair","price":150}]""", JsonSamples.BuildJson());

    [Fact]
    public void JsonDocument_counts_completed() =>
        Assert.Equal("Completed orders: 2", JsonSamples.JsonDocumentQuery());

    [Fact]
    public void Object_properties_in_order() =>
        Assert.Equal(["id", "customer", "status", "items"], JsonSamples.ObjectProperties());

    [Fact]
    public void Modify_json_adds_totals() =>
        Assert.Equal(["#1: 1250", "#2: 300", "#3: 15", "#4: 0"], JsonSamples.ModifyJson());

    [Fact]
    public void Quantity_per_customer() =>
        Assert.Equal(["Alice: 13", "Bob: 1"], JsonSamples.QuantityPerCustomer());

    [Fact]
    public void Json_lines_filter_errors() =>
        Assert.Equal(["disk full", "timeout"], JsonSamples.JsonLines());
}
