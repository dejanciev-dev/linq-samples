namespace LinqSamples.Tests;

public class MoreSampleTests
{
    [Fact] public void Page_per_item() => Assert.Equal(["1:1", "1:2", "1:3", "1:4", "2:5", "2:6"], BatchingSamples.PagePerItem());
    [Fact] public void Heaviest_batch() => Assert.Equal(26, BatchingSamples.HeaviestBatch());
    [Fact] public void Middle_pages() => Assert.Equal([4, 5, 6, 7, 8, 9], BatchingSamples.MiddlePages());
    [Fact] public void Collapse_spaces() => Assert.Equal("a b c", CleaningSamples.CollapseSingleSpaces());
    [Fact] public void Digits_only() => Assert.Equal("5551234567", CleaningSamples.PhoneDigits());
    [Fact] public void Nulls_to_default() => Assert.Equal(["a", "n/a", "c"], CleaningSamples.NullsToDefault());
    [Fact] public void Anagrams() => Assert.Equal("True", EqualitySamples.Anagrams());
    [Fact] public void Common_count() => Assert.Equal(2, EqualitySamples.CommonCount());
    [Fact] public void Same_multiset() => Assert.Equal("True", EqualitySamples.SameMultiset());
    [Fact] public void Average_spend() => Assert.Equal(100m, FinanceSamples.AverageSpend());
    [Fact] public void Round_money() => Assert.Equal([0.13m, 0.14m, 2.5m], FinanceSamples.RoundMoney());
    [Fact] public void With_tax() => Assert.Equal(360m, FinanceSamples.WithTax());
    [Fact] public void Total_value() => Assert.Equal(101.5m, InventorySamples.TotalValue());
    [Fact] public void Low_stock() => Assert.Equal(["gear", "axle"], InventorySamples.LowStock());
    [Fact] public void Most_valuable() => Assert.Equal("axle", InventorySamples.MostValuable());
    [Fact] public void Reorder() => Assert.Equal(["gear:5", "axle:16"], InventorySamples.Reorder());
    [Fact] public void By_value() => Assert.Equal(["axle", "gear", "bolt", "nut"], InventorySamples.ByValue());
    [Fact] public void Out_of_stock() => Assert.False(InventorySamples.OutOfStock());
    [Fact] public void Buckets() => Assert.Equal(["high:bolt", "medium:nut", "low:gear,axle"], InventorySamples.Buckets());
}
