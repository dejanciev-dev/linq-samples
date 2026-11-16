namespace LinqSamples.Tests;

public class StockSampleTests
{
    [Fact] public void Stock_per_sku() => Assert.Equal(["A:18", "B:9", "C:0"], StockSamples.StockPerSku());
    [Fact] public void Stock_per_sku_warehouse() => Assert.Equal(["A/W1:13", "A/W2:5", "B/W1:5", "B/W2:4", "C/W2:0"], StockSamples.StockPerSkuWarehouse());
    [Fact] public void Sold_out() => Assert.Equal(["C"], StockSamples.SoldOut());
    [Fact] public void Running_balance() => Assert.Equal([10, 7, 13], StockSamples.RunningBalance());
    [Fact] public void Busiest_day() => Assert.Equal(1, StockSamples.BusiestDay());
    [Fact] public void Largest_outflow() => Assert.Equal(["A:-3", "B:-2", "C:-9"], StockSamples.LargestOutflow());
    [Fact] public void In_both_warehouses() => Assert.Equal(["A", "B"], StockSamples.InBothWarehouses());
    [Fact] public void Skus_per_warehouse() => Assert.Equal(["W1:A,B", "W2:A,B,C"], StockSamples.SkusPerWarehouse());
    [Fact] public void Cumulative_net_per_day() => Assert.Equal([26, 28, 30, 21, 27], StockSamples.CumulativeNetPerDay());
    [Fact] public void Deltas() => Assert.Equal([-13, 8, 1], StockSamples.Deltas());
}
