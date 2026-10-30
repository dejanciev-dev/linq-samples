using LinqSamples.Samples.EfCore;

namespace LinqSamples.Tests;

/// <summary>Pins the SQL shape and results of the EF Core translation and behavior samples.</summary>
public class EfCoreDeepDiveTests
{
    // Query translation

    [Fact]
    public void String_methods_use_like_and_instr() =>
        AssertSqlAndResult(EfTranslationSamples.StringMethods(), ["LIKE 'Al%'", "instr("], ["Alice", "Dan"]);

    [Fact]
    public void Like_pattern() =>
        AssertSqlAndResult(EfTranslationSamples.Like(), ["LIKE '%o%'"], ["Laptop", "Mouse", "Notebook", "Monitor"]);

    [Fact]
    public void String_projection_runs_in_sql() =>
        AssertSqlAndResult(EfTranslationSamples.StringProjection(), ["upper(substr("], ["ALI", "BOB", "CAR", "DAN", "EVA"]);

    [Fact]
    public void Date_parts_group_by_month() =>
        AssertSqlAndResult(
            EfTranslationSamples.DateParts(),
            ["strftime('%m'", "GROUP BY"],
            ["{ Month = 1, Orders = 2 }", "{ Month = 2, Orders = 3 }", "{ Month = 3, Orders = 3 }", "{ Month = 4, Orders = 2 }"]);

    [Fact]
    public void Conditional_becomes_case() =>
        AssertSqlAndResult(
            EfTranslationSamples.ConditionalToCase(),
            ["CASE", "WHEN \"o\".\"Quantity\" >= 5"],
            ["{ Id = 1, Size = single }", "{ Id = 2, Size = single }", "{ Id = 3, Size = single }", "{ Id = 4, Size = single }", "{ Id = 5, Size = bulk }"]);

    [Fact]
    public void Null_safe_aggregates_use_coalesce() =>
        AssertSqlAndResult(
            EfTranslationSamples.NullSafeAggregates(),
            ["COALESCE(", "MAX("],
            ["{ Name = Alice, Largest = 2 }", "{ Name = Bob, Largest = 4 }", "{ Name = Carla, Largest = 10 }", "{ Name = Dan, Largest = 20 }", "{ Name = Eva, Largest = 0 }"]);

    [Fact]
    public void Any_becomes_exists() =>
        AssertSqlAndResult(EfTranslationSamples.AnyToExists(), ["WHERE EXISTS ("], ["Carla"]);

    [Fact]
    public void All_becomes_not_exists_and_includes_empty() =>
        AssertSqlAndResult(EfTranslationSamples.AllToNotExists(), ["WHERE NOT EXISTS ("], ["Eva"]);

    [Fact]
    public void Count_distinct_is_one_query() =>
        AssertSqlAndResult(EfTranslationSamples.CountDistinct(), ["SELECT COUNT(*)", "SELECT DISTINCT"], ["4"]);

    [Fact]
    public void SelectMany_navigation_joins() =>
        AssertSqlAndResult(EfTranslationSamples.SelectManyNavigation(), ["INNER JOIN"], ["1", "2", "5", "6", "9", "10"]);

    [Fact]
    public void Union_removes_duplicates() =>
        AssertSqlAndResult(EfTranslationSamples.Union(), ["UNION"], ["Alice", "Carla", "Dan"]);

    [Fact]
    public void Concat_keeps_duplicates() =>
        AssertSqlAndResult(EfTranslationSamples.Concat(), ["UNION ALL"], ["Alice", "Carla", "Carla", "Dan"]);

    [Fact]
    public void Except_finds_customers_without_orders() =>
        AssertSqlAndResult(EfTranslationSamples.Except(), ["EXCEPT"], ["Eva"]);

    [Fact]
    public void Top_per_group() =>
        AssertSqlAndResult(
            EfTranslationSamples.TopPerGroup(),
            ["GROUP BY", "LIMIT 1"],
            ["{ Category = Electronics, Top = Laptop }", "{ Category = Furniture, Top = Desk }", "{ Category = Stationery, Top = Notebook }"]);

    [Fact]
    public void Nested_collections_load_in_one_query() =>
        AssertSqlAndResult(EfTranslationSamples.NestedCollections(), ["LEFT JOIN"], ["Bob: [3,4]", "Eva: []"]);

    [Fact]
    public void GroupBy_navigation_adds_join() =>
        AssertSqlAndResult(
            EfTranslationSamples.GroupByNavigation(),
            ["INNER JOIN", "GROUP BY \"c\".\"City\""],
            ["{ City = Berlin, Orders = 2, Quantity = 5 }", "{ City = Skopje, Orders = 6, Quantity = 17 }", "{ City = Vienna, Orders = 2, Quantity = 21 }"]);

    [Fact]
    public void Keyset_pagination_filters_on_the_key() =>
        AssertSqlAndResult(EfTranslationSamples.KeysetPagination(), ["WHERE \"p\".\"Id\" > @lastSeenId"], ["Chair", "Notebook"]);

    [Fact]
    public void Parameters_vs_constants() =>
        Assert.Equal(
            ["Variable:    WHERE \"c\".\"City\" = @city", "EF.Constant: WHERE \"c\".\"City\" = 'Berlin'"],
            EfTranslationSamples.ParametersVsConstants());

    [Fact]
    public void Math_functions_run_in_sql() =>
        AssertSqlAndResult(EfTranslationSamples.MathFunctions(), ["round("], ["{ Name = Desk, Third = 100 }", "{ Name = Chair, Third = 50 }"]);

    [Fact]
    public void TagWith_adds_a_comment() =>
        AssertSqlAndResult(EfTranslationSamples.TagWith(), ["-- Dashboard: expensive products"], ["Laptop", "Desk", "Monitor"]);

    // Loading, tracking & saving

    [Fact]
    public void Split_query_uses_two_commands() =>
        Assert.Equal(["Single query: 1 SQL command", "Split query:  2 SQL commands"], EfBehaviorSamples.SplitQuery());

    [Fact]
    public void N_plus_one_vs_include() =>
        Assert.Equal(["Load per customer: 6 SQL commands", "Include:           1 SQL command"], EfBehaviorSamples.NPlusOne());

    [Fact]
    public void Filtered_include() =>
        AssertSqlAndResult(
            EfBehaviorSamples.FilteredInclude(),
            ["WHERE \"o\".\"Status\" = 'Completed'"],
            ["Alice: 1 completed", "Bob: 0 completed", "Carla: 1 completed", "Dan: 1 completed", "Eva: 0 completed"]);

    [Fact]
    public void ThenInclude_loads_products() =>
        AssertSqlAndResult(EfBehaviorSamples.ThenInclude(), ["INNER JOIN \"Products\""], ["Bob: Desk, Chair", "Eva:"]);

    [Fact]
    public void Query_navigation_counts_without_loading() =>
        Assert.Equal(["Orders counted in SQL: 3", "Orders loaded:         0"], EfBehaviorSamples.QueryNavigation());

    [Fact]
    public void Find_hits_the_tracker() =>
        Assert.Equal(["Find twice:           1 SQL command", "FirstOrDefault twice: 2 SQL commands"], EfBehaviorSamples.FindUsesTracker());

    [Fact]
    public void Identity_resolution() =>
        Assert.Equal(["Tracked, same instance:   True", "AsNoTracking, same:       False"], EfBehaviorSamples.IdentityResolution());

    [Fact]
    public void Change_tracking_updates_only_the_changed_column()
    {
        var output = EfBehaviorSamples.ChangeTracking().ToList();
        Assert.Equal(["State after edit:  Modified", "Modified columns:  Price", "Rows saved:        1"], output.Take(3));
        Assert.Contains(output, line => line.Contains("UPDATE \"Products\" SET \"Price\" = @p0", StringComparison.Ordinal));
    }

    [Fact]
    public void Add_gets_a_generated_key() =>
        Assert.Equal(["Before save: Added", "After save:  Unchanged, Id = 8"], EfBehaviorSamples.AddEntity());

    [Fact]
    public void Remove_deletes_the_row() =>
        Assert.Equal(["State:         Deleted", "Products left: 6"], EfBehaviorSamples.RemoveEntity());

    [Fact]
    public void Transaction_rollback_restores_rows() =>
        Assert.Equal(
            ["Deleted in transaction: 10", "Count inside:           0", "Count after rollback:   10"],
            EfBehaviorSamples.Transactions());

    [Fact]
    public void Global_query_filter_hides_cancelled_orders() =>
        Assert.Equal(
            ["Generated:          WHERE \"o\".\"Status\" <> 'Cancelled'", "Orders (filtered):  9", "IgnoreQueryFilters: 10"],
            EfBehaviorSamples.GlobalQueryFilters());

    [Fact]
    public void Compiled_query() =>
        Assert.Equal(["Alice", "Carla"], EfBehaviorSamples.CompiledQuery());

    [Fact]
    public void FromSql_is_parameterized_and_composable() =>
        AssertSqlAndResult(EfBehaviorSamples.FromSqlComposed(), ["SELECT * FROM Products WHERE Price > @p0", "ORDER BY"], ["Chair", "Desk"]);

    [Fact]
    public void SqlQuery_scalar() =>
        AssertSqlAndResult(EfBehaviorSamples.SqlQueryScalar(), ["SELECT COUNT(*) AS Value FROM Orders"], ["10"]);

    [Fact]
    public async Task Async_queries() =>
        Assert.Equal(["Orders:        10", "Any declined?  True", "Top two:       Laptop, Desk"], await EfBehaviorSamples.AsyncQueries());

    [Fact]
    public async Task Streaming_reads_every_row() =>
        Assert.Equal(["Laptop", "Mouse", "Desk", "Chair", "Notebook", "Pen", "Monitor"], await EfBehaviorSamples.Streaming());

    private static void AssertSqlAndResult(IEnumerable<string> output, string[] sqlFragments, string[] expectedRows)
    {
        var lines = output.ToList();
        var resultIndex = lines.IndexOf("Result:");
        var sql = string.Join("\n", lines.Take(resultIndex));

        Assert.All(sqlFragments, fragment => Assert.Contains(fragment, sql, StringComparison.Ordinal));
        Assert.Equal(expectedRows, lines.Skip(resultIndex + 1).Select(line => line.Trim()));
    }
}
