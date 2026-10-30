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
    public void SkipWhile_starts_at_the_first_failing_element() =>
        Assert.Equal([1, 7, 3, 8, 2, 9, 4], PartitioningSamples.SkipWhile());

    [Fact]
    public void Filter_by_list_acts_like_sql_in() =>
        Assert.Equal(["Bob", "Dan", "Eva"], FilteringSamples.FilterByList());

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
    public void UnionBy_prefers_the_updated_values()
    {
        var merged = SetSamples.UnionBy().ToList();
        Assert.Equal(8, merged.Count);
        Assert.Contains("2 Mouse: 20", merged);
        Assert.Equal("8 Webcam: 80", merged.Last());
    }

    [Fact]
    public void Max_of_empty_sequence_alternatives() =>
        Assert.Equal(
            ["Max():                  throws InvalidOperationException",
             "DefaultIfEmpty().Max(): 0",
             "Max(n => (int?)n):      null"],
            AggregationSamples.MaxOfEmpty());

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
    public void Captured_variable_is_read_at_enumeration() =>
        Assert.Equal(["Count: 2"], DeferredExecutionSamples.CapturedVariables());

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
    public void Window_yields_overlapping_windows()
    {
        Assert.Equal([[1, 2, 3], [2, 3, 4]], new[] { 1, 2, 3, 4 }.Window(3));
        Assert.Empty(new[] { 1, 2 }.Window(3));
        Assert.Equal(8, CustomOperatorSamples.MovingAverage().Count());
    }

    [Fact]
    public void Iterate_doubles_from_one() =>
        Assert.Equal([1, 2, 4, 8, 16, 32, 64, 128], CustomOperatorSamples.Iterate());

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
    public void Latest_order_per_customer() =>
        Assert.Equal(
            ["Alice: #9 on 2026-04-01", "Bob: #4 on 2026-02-14", "Carla: #10 on 2026-04-10", "Dan: #8 on 2026-03-21"],
            RecipeSamples.LatestOrderPerCustomer());

    [Fact]
    public void Percentage_of_total_revenue_by_category() =>
        Assert.Equal(["Electronics: 79.4%", "Furniture: 18.7%", "Stationery: 1.9%"], RecipeSamples.PercentageOfTotal());

    [Fact]
    public void Find_gaps() =>
        Assert.Equal([6], RecipeSamples.FindGaps());

    [Fact]
    public void Find_duplicates() =>
        Assert.Equal([3, 9], RecipeSamples.FindDuplicates());

    [Fact]
    public void Hierarchy_is_flattened_depth_first() =>
        Assert.Equal(["Grace", "  Linus", "    Ken", "    Barbara", "  Ada"], RecipeSamples.FlattenHierarchy());

    [Fact]
    public void Switch_expression_labels_prices() =>
        Assert.Equal(
            ["Laptop: premium", "Mouse: budget", "Desk: mid-range", "Chair: mid-range",
             "Notebook: budget", "Pen: budget", "Monitor: mid-range"],
            ProjectionSamples.SelectSwitch());

    [Fact]
    public void Anonymous_types_filter_on_computed_value() =>
        Assert.Equal(["Alice", "Carla"], ProjectionSamples.AnonymousTypes());

    [Fact]
    public void Largest_orders_by_line_total() =>
        Assert.Equal(["#1: 1200", "#6: 1200", "#4: 600"], OrderingSamples.OrderByComputedKey());

    [Fact]
    public void OrderBy_keeps_ties_in_original_order() =>
        Assert.Equal(["date", "apple", "Banana", "cherry", "avocado", "Apricot", "blueberry"], OrderingSamples.StableSort());

    [Fact]
    public void GroupBy_result_selector_keeps_first_seen_order() =>
        Assert.Equal(
            ["Completed: 1,5,8", "Shipped: 2,9", "Pending: 3,11", "Awaiting Payment: 4,10", "Cancelled: 6", "Declined: 7"],
            GroupingSamples.GroupByResultSelector());

    [Fact]
    public void Buckets_form_a_histogram() =>
        Assert.Equal(["1-3: 4", "4-6: 2", "7-9: 4"], GroupingSamples.Buckets());

    [Fact]
    public void Composite_key_join_matches_both_columns() =>
        Assert.Equal(["#2: 10%", "#7: 5%"], JoinSamples.CompositeKeyJoin());

    [Fact]
    public void Anti_join_finds_customers_without_orders() =>
        Assert.Equal(["Eva"], JoinSamples.AntiJoin());

    [Fact]
    public void SingleOrDefault_handles_zero_one_and_many() =>
        Assert.Equal(
            ["Paris:  (none)", "Vienna: Dan", "Berlin: throws InvalidOperationException"],
            ElementSamples.SingleOrDefault());

    [Fact]
    public void OrDefault_access_returns_fallbacks() =>
        Assert.Equal(["Last shipped order:  #9", "ElementAtOrDefault(100): 0"], ElementSamples.OrDefaultAccess());

    [Fact]
    public void ToDictionary_with_duplicates_needs_grouping() =>
        Assert.Equal(
            ["ToDictionary(c => c.City): throws ArgumentException", "Skopje: Alice, Carla", "Berlin: Bob, Eva", "Vienna: Dan"],
            GenerationSamples.ToDictionaryDuplicates());

    [Fact]
    public void Mondays_in_february_2026() =>
        Assert.Equal(["2026-02-02", "2026-02-09", "2026-02-16", "2026-02-23"], GenerationSamples.GenerateDates());

    [Fact]
    public void Query_syntax_orderby_matches_ThenBy() =>
        Assert.Equal(OrderingSamples.ThenBy(), QuerySyntaxSamples.OrderByMultipleKeys());

    [Fact]
    public void Query_syntax_join_into_matches_GroupJoin() =>
        Assert.Equal(JoinSamples.GroupJoin(), QuerySyntaxSamples.JoinInto());

    [Fact]
    public void Exceptions_are_deferred_to_enumeration() =>
        Assert.Equal(
            ["Defined:    no exception yet", "Enumerated: throws DivideByZeroException"],
            DeferredExecutionSamples.DeferredExceptions());

    [Fact]
    public void Aggregate_with_result_selector_finds_longest_word() =>
        Assert.Equal("BLUEBERRY", AggregationSamples.AggregateWithResultSelector());

    [Fact]
    public void Except_also_removes_duplicates() =>
        Assert.Equal([1, 2], SetSamples.SetOperatorsDeduplicate());

    [Fact]
    public void DistinctUntilChanged_collapses_runs()
    {
        Assert.Equal([1, 2, 3, 1], CustomOperatorSamples.DistinctUntilChanged());
        Assert.Empty(Array.Empty<int>().DistinctUntilChanged());
        Assert.Equal(["a", null, "b"], new[] { "a", null, null, "b" }.DistinctUntilChanged());
    }

    [Fact]
    public void Dense_rank_shares_ranks_for_ties()
    {
        var ranks = RecipeSamples.DenseRank().ToList();
        Assert.Equal("9 -> #1", ranks[2]);
        Assert.Equal("9 -> #1", ranks[8]);
        Assert.Equal("1 -> #8", ranks[3]);
    }

    [Fact]
    public void Compare_lists_reports_added_removed_unchanged() =>
        Assert.Equal(["Added:     Dan", "Removed:   Alice", "Unchanged: Bob, Carla"], RecipeSamples.CompareLists());

    [Fact]
    public void Property_pattern_filters_on_two_properties() =>
        Assert.Equal(["Chair"], FilteringSamples.PatternMatching());

    [Fact]
    public void Date_range_is_half_open() =>
        Assert.Equal([3, 4, 5], FilteringSamples.DateRange());

    [Fact]
    public void SelectMany_index_refers_to_the_outer_element() =>
        Assert.Equal(["line 1: to", "line 1: be", "line 2: or", "line 2: not"], ProjectionSamples.SelectManyWithIndex());

    [Fact]
    public void Lookup_dictionary_enriches_orders() =>
        Assert.Equal(["#1: 1 x Laptop", "#2: 2 x Mouse", "#3: 1 x Desk"], ProjectionSamples.EnrichWithLookup());

    [Fact]
    public void Nulls_are_sorted_last() =>
        Assert.Equal(["Ada", "Linus", "Barbara", "Ken", "Grace"], OrderingSamples.NullsLast());

    [Fact]
    public void Natural_sort_orders_numbers_by_value() =>
        Assert.Equal(
            ["Ordinal: file1.txt, file10.txt, file2.txt, file20.txt",
             "Natural: file1.txt, file2.txt, file10.txt, file20.txt"],
            OrderingSamples.NaturalSort());

    [Fact]
    public void Take_middle_range() =>
        Assert.Equal([9, 1, 7], PartitioningSamples.TakeMiddleRange());

    [Fact]
    public void Last_page_is_partial() =>
        Assert.Equal(["Monitor"], PartitioningSamples.LastPage());

    [Fact]
    public void Having_keeps_cities_with_several_customers() =>
        Assert.Equal(["Skopje: 2", "Berlin: 2"], GroupingSamples.Having());

    [Fact]
    public void Orders_by_day_of_week() =>
        Assert.Equal(
            ["Sunday: 1", "Monday: 2", "Tuesday: 2", "Wednesday: 1", "Friday: 1", "Saturday: 4"],
            GroupingSamples.ByDayOfWeek());

    [Fact]
    public void Full_outer_join_keeps_both_sides()
    {
        var rows = JoinSamples.FullOuterJoin().ToList();
        Assert.Equal(12, rows.Count);
        Assert.Equal(["Eva -> (none)", "(guest) -> #11"], rows.TakeLast(2));
    }

    [Fact]
    public void Non_equi_join_assigns_bands() =>
        Assert.Equal(
            ["Laptop: Large", "Mouse: Small", "Desk: Medium", "Chair: Medium", "Notebook: Small", "Pen: Small", "Monitor: Medium"],
            JoinSamples.NonEquiJoin());

    [Fact]
    public void Contains_respects_the_comparer() =>
        Assert.Equal(
            ["Contains(\"APPLE\"):              False", "Contains(\"APPLE\", IgnoreCase):  True"],
            ElementSamples.ContainsWithComparer());

    [Fact]
    public void Multiplication_table() =>
        Assert.Equal([" 1  2  3", " 2  4  6", " 3  6  9"], GenerationSamples.MultiplicationTable());

    [Fact]
    public void Shuffle_returns_distinct_products()
    {
        var picked = GenerationSamples.Shuffle().ToList();
        Assert.Equal(3, picked.Distinct().Count());
        Assert.All(picked, name => Assert.Contains(LinqSamples.Data.SampleData.Products, p => p.Name == name));
    }

    [Fact]
    public void Several_lets_filter_and_sort() =>
        Assert.Equal(["apple", "Apricot", "avocado"], QuerySyntaxSamples.SeveralLets());

    [Fact]
    public void Join_then_group_counts_orders_per_city() =>
        Assert.Equal(["Berlin: 2", "Skopje: 6", "Vienna: 2"], QuerySyntaxSamples.JoinThenGroup());

    [Fact]
    public void Elements_stream_through_the_pipeline() =>
        Assert.Equal(["where 5, select 5, where 3, where 9, select 9"], DeferredExecutionSamples.StreamingOrder());

    [Fact]
    public void Interleave_appends_the_longer_tail()
    {
        Assert.Equal([1, 2, 3, 4, 5, 6, 8], CustomOperatorSamples.Interleave());
        Assert.Equal([1, 2, 3], new[] { 1, 3 }.Interleave([2]));
    }

    [Fact]
    public void Median_of_even_count_averages_the_middle_pair() =>
        Assert.Equal(4.5, RecipeSamples.Median());
}
