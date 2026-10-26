using LinqSamples.EfCore;

namespace LinqSamples.Tests;

public class EfCoreSampleTests
{
    [Fact]
    public void LeftJoin_is_translated_to_sql_left_join()
    {
        var output = EfCoreSamples.LeftJoin().ToList();

        Assert.Contains(output, line => line.Contains("LEFT JOIN"));
        Assert.Contains(output, line => line.Contains("Name = Eva, OrderId = "));
    }

    [Fact]
    public void GroupBy_is_translated_to_sql_group_by() =>
        Assert.Contains(EfCoreSamples.GroupBy(), line => line.Contains("GROUP BY"));

    [Fact]
    public void Paging_uses_limit_and_offset()
    {
        var output = EfCoreSamples.Paging().ToList();

        Assert.Contains(output, line => line.Contains("LIMIT"));
        Assert.Equal(["  Chair", "  Notebook", "  Pen"], output.SkipWhile(l => l != "Result:").Skip(1));
    }

    [Fact]
    public void Untranslatable_method_throws_and_AsEnumerable_fixes_it() =>
        Assert.Equal(
            ["Where(c => IsVip(c.Name)) -> InvalidOperationException: the LINQ expression could not be translated",
             "AsEnumerable() first      -> Alice"],
            EfCoreSamples.ClientEvaluation());

    [Fact]
    public void IEnumerable_loads_every_row() =>
        Assert.Equal(
            ["IQueryable:  3 matches, 3 rows loaded", "IEnumerable: 3 matches, 10 rows loaded"],
            EfCoreSamples.QueryableVsEnumerable());

    [Fact]
    public void AsNoTracking_tracks_nothing() =>
        Assert.Equal(["Tracked entities:   7", "With AsNoTracking:  0"], EfCoreSamples.AsNoTracking());

    [Fact]
    public void Bulk_operations_report_affected_rows() =>
        Assert.Equal(
            ["Updated Pending -> Cancelled: 1", "Deleted Cancelled:            2", "Orders left:                  8"],
            EfCoreSamples.BulkOperations());
}
