namespace LinqSamples.Tests;

/// <summary>Pins the expected result of key samples so they double as executable documentation.</summary>
public class SampleResultTests
{
    [Fact]
    public void Where_filters_by_price() =>
        Assert.Equal(["Laptop", "Desk", "Monitor"], FilteringSamples.Where().Select(p => p.Name));

    [Fact]
    public void OfType_keeps_only_strings() =>
        Assert.Equal(["one", "two", "three"], FilteringSamples.OfType());

    [Fact]
    public void Zip_combines_three_sequences() =>
        Assert.Equal(["Alice: 90 (A)", "Bob: 75 (C)", "Carla: 82 (B)"], ProjectionSamples.Zip());

    [Fact]
    public void Order_by_specific_values_follows_the_preference_list() =>
        Assert.Equal(
            ["#4 Awaiting Payment", "#10 Awaiting Payment", "#3 Pending", "#11 Pending", "#7 Declined",
             "#6 Cancelled", "#2 Shipped", "#9 Shipped", "#1 Completed", "#5 Completed", "#8 Completed"],
            OrderingSamples.OrderBySpecificValues());

    [Fact]
    public void Unknown_values_are_sorted_last() =>
        Assert.Equal(
            ["#2 Shipped", "#9 Shipped", "#1 Completed", "#5 Completed", "#8 Completed"],
            OrderingSamples.OrderBySpecificValuesUnknownLast().Take(5));

    [Fact]
    public void Chunk_splits_into_batches_of_four() =>
        Assert.Equal([4, 4, 2], PartitioningSamples.Chunk().Select(c => c.Length));

    [Fact]
    public void Paging_returns_the_second_page() =>
        Assert.Equal(["Chair", "Notebook", "Pen"], PartitioningSamples.Paging());

    [Fact]
    public void CountBy_counts_statuses() =>
        Assert.Equal(new KeyValuePair<string, int>("Completed", 3), GroupingSamples.CountBy().First());

    [Fact]
    public void AggregateBy_sums_quantity_per_customer() =>
        Assert.Equal([5, 5, 12, 21, 5], GroupingSamples.AggregateBy().Select(kv => kv.Value));

    [Fact]
    public void LeftJoin_keeps_customers_without_orders() =>
        Assert.Equal("Eva -> (none)", JoinSamples.LeftJoin().Last());

    [Fact]
    public void LeftJoin_matches_the_classic_pattern() =>
        Assert.Equal(JoinSamples.LeftJoinClassic(), JoinSamples.LeftJoin());

    [Fact]
    public void RightJoin_keeps_orders_without_customers() =>
        Assert.Equal("#11 (guest)", JoinSamples.RightJoin().Last());

    [Fact]
    public void ExceptBy_finds_products_never_ordered() =>
        Assert.Equal(["Monitor"], SetSamples.ExceptBy());

    [Fact]
    public void Revenue_of_completed_orders() =>
        Assert.Equal(1255m, AggregationSamples.Revenue());

    [Fact]
    public void Factorial_of_ten() =>
        Assert.Equal(3_628_800L, AggregationSamples.Factorial());

    [Fact]
    public void FirstOrDefault_uses_the_supplied_default() =>
        Assert.Equal("Nobody", ElementSamples.FirstOrDefaultWithDefault());

    [Fact]
    public void Sequence_steps_inclusively() =>
        Assert.Equal([0, 5, 10, 15, 20], GenerationSamples.Sequence());

    [Fact]
    public void Query_and_method_syntax_are_equivalent() =>
        Assert.Equal(QuerySyntaxSamples.Basic(), QuerySyntaxSamples.BasicMethodSyntax());

    [Fact]
    public void Deferred_query_sees_later_changes() =>
        Assert.Equal(["Count: 3"], DeferredExecutionSamples.SeesLaterChanges());

    [Fact]
    public void First_short_circuits() =>
        Assert.Equal("Visited: 5,3,9", DeferredExecutionSamples.ShortCircuit().Last());

    [Fact]
    public void Scan_matches_running_total() =>
        Assert.Equal(AggregationSamples.RunningTotal(), CustomOperatorSamples.Scan());

    [Fact]
    public void Pairwise_on_short_sequences()
    {
        Assert.Empty(Array.Empty<int>().Pairwise());
        Assert.Empty(new[] { 1 }.Pairwise());
        Assert.Equal([(1, 2), (2, 3)], new[] { 1, 2, 3 }.Pairwise());
    }

    [Fact]
    public void WhereIf_only_filters_when_condition_is_true()
    {
        int[] source = [1, 2, 3];
        Assert.Equal([1, 2, 3], source.WhereIf(false, n => n > 1));
        Assert.Equal([2, 3], source.WhereIf(true, n => n > 1));
    }

    [Fact]
    public void Revenue_report_ranks_customers() =>
        Assert.Equal(["1. Alice: 1550", "2. Carla: 30", "3. Dan: 25"], RecipeSamples.RevenueReport());

    [Fact]
    public void Find_duplicates() =>
        Assert.Equal([3, 9], RecipeSamples.FindDuplicates());

    [Fact]
    public void Hierarchy_is_flattened_depth_first() =>
        Assert.Equal(["Grace", "  Linus", "    Ken", "    Barbara", "  Ada"], RecipeSamples.FlattenHierarchy());
}
