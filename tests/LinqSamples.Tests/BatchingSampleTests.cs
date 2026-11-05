namespace LinqSamples.Tests;

public class BatchingSampleTests
{
    [Fact] public void Batches() => Assert.Equal(["1,2,3,4", "5,6,7,8", "9,10"], BatchingSamples.Batches());
    [Fact] public void Page() => Assert.Equal([9, 10], BatchingSamples.Page());
    [Fact] public void Page_count() => Assert.Equal(3, BatchingSamples.PageCount());
    [Fact] public void Numbered_pages() => Assert.Equal(["1:1,2,3", "2:4,5,6", "3:7,8,9", "4:10"], BatchingSamples.NumberedPages());
    [Fact] public void Round_robin() => Assert.Equal(["1,4,7,10", "2,5,8", "3,6,9"], BatchingSamples.RoundRobin());
    [Fact] public void Interleave() => Assert.Equal([1, 2, 3, 4, 5, 6], BatchingSamples.Interleave());
    [Fact] public void Every_third() => Assert.Equal([1, 4, 7, 10], BatchingSamples.EveryThird());
    [Fact] public void Under_budget() => Assert.Equal([1, 2, 3, 4, 5], BatchingSamples.UnderBudget());
    [Fact] public void Last_page() => Assert.Equal("9,10", BatchingSamples.LastPage());
    [Fact] public void Padded() => Assert.Equal(["1,2,3,4", "5,6,7,8", "9,10,0,0"], BatchingSamples.PaddedBatches());
    [Fact] public void Batch_sums() => Assert.Equal([6, 15, 24, 10], BatchingSamples.BatchSums());
    [Fact] public void Contiguous() => Assert.Equal(["1,2,3,4", "5,6,7", "8,9,10"], BatchingSamples.ContiguousParts());
    [Fact] public void Alternate() => Assert.Equal([1, 2, 5, 6, 9, 10], BatchingSamples.AlternateBatches());
    [Fact] public void Ends() => Assert.Equal("1,2,3|9,10", BatchingSamples.Ends());
    [Fact] public void Weight_batches() => Assert.Equal(["1,2,3,4", "5,6", "7", "8", "9", "10"], BatchingSamples.WeightBatches());

}

public class BatchingExtraTests
{
    [Fact] public void Page_info() => Assert.Equal("page 2/3: 5,6,7,8", BatchingSamples.PageInfo());
    [Fact] public void Batch_offsets() => Assert.Equal(["0:4", "4:4", "8:2"], BatchingSamples.BatchOffsets());
    [Fact] public void Reversed_pages() => Assert.Equal(["9,10", "5,6,7,8", "1,2,3,4"], BatchingSamples.ReversedPages());
    [Fact] public void Take_range() => Assert.Equal([3, 4, 5], BatchingSamples.TakeRange());
}

public class BatchingMoreTests
{
    [Fact] public void Overlapping() => Assert.Equal(["1,2,3", "2,3,4", "3,4,5"], BatchingSamples.Overlapping());
    [Fact] public void Batch_counts() => Assert.Equal("2:5|3:4|5:2", BatchingSamples.BatchCounts());
    [Fact] public void Batch_heads() => Assert.Equal([1, 5, 9], BatchingSamples.BatchHeads());
    [Fact] public void Labelled() => Assert.Equal(["A:1,2,3,4,5", "B:6,7,8,9,10"], BatchingSamples.LabelledBatches());
}
