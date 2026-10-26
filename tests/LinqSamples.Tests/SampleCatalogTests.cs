using LinqSamples.Infrastructure;

namespace LinqSamples.Tests;

public class SampleCatalogTests
{
    public static TheoryData<string, string> AllSamples()
    {
        var data = new TheoryData<string, string>();
        foreach (var sample in SampleCatalog.Categories.SelectMany(c => c.Samples))
        {
            data.Add(sample.Category, sample.Title);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(AllSamples))]
    public void Every_sample_runs_and_produces_output(string category, string title)
    {
        var sample = SampleCatalog.Categories
            .Single(c => c.Name == category)
            .Samples.Single(s => s.Title == title);

        var output = ResultFormatter.Format(sample.Run());

        Assert.False(string.IsNullOrWhiteSpace(output));
    }

    [Fact]
    public void Categories_are_discovered_in_order()
    {
        var orders = SampleCatalog.Categories.Select(c => c.Order).ToList();

        Assert.Equal(15, orders.Count);
        Assert.Equal(orders.Order(), orders);
    }
}
