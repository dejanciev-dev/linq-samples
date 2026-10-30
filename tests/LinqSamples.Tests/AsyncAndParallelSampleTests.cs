namespace LinqSamples.Tests;

/// <summary>Pins the results of the async (IAsyncEnumerable) and PLINQ samples.</summary>
public class AsyncAndParallelSampleTests
{
    // Async LINQ

    [Fact]
    public async Task Await_foreach_reads_every_order() =>
        Assert.Equal(Enumerable.Range(1, 11), await AsyncSamples.AwaitForeach());

    [Fact]
    public async Task Where_select_on_a_stream() =>
        Assert.Equal(["#1", "#5", "#8"], await AsyncSamples.WhereSelect().ToListAsync());

    [Fact]
    public async Task Async_predicate_skips_sold_out_laptops() =>
        Assert.Equal([2, 3, 4, 5, 7, 8, 9, 10, 11], await AsyncSamples.AsyncPredicate().ToListAsync());

    [Fact]
    public async Task Async_projection_looks_up_names() =>
        Assert.Equal(["#1 Alice", "#2 Alice", "#3 Bob"], await AsyncSamples.AsyncProjection().ToListAsync());

    [Fact]
    public async Task Async_aggregates() =>
        Assert.Equal(
            ["Count:          11", "Total quantity: 48", "Any declined?   True"],
            await AsyncSamples.Aggregates());

    [Fact]
    public async Task First_async_with_and_without_a_match() =>
        Assert.Equal(["First pending: #3", "First lost:    (none)"], await AsyncSamples.FirstAsync());

    [Fact]
    public async Task Take_only_pulls_what_it_needs() =>
        Assert.Equal("Took 2, produced 2", await AsyncSamples.TakeStopsTheStream());

    [Fact]
    public async Task Cancellation_stops_after_three_items() =>
        Assert.Equal(["Received: 3", "Stopped:  OperationCanceledException"], await AsyncSamples.Cancellation());

    [Fact]
    public async Task GroupBy_on_a_stream() =>
        Assert.Equal(
            ["Completed: 3", "Shipped: 2", "Pending: 2", "Awaiting Payment: 2", "Cancelled: 1", "Declined: 1"],
            await AsyncSamples.GroupBy().ToListAsync());

    [Fact]
    public async Task Chunk_a_stream() =>
        Assert.Equal(["1,2,3,4", "5,6,7,8", "9,10,11"], await AsyncSamples.Chunk().ToListAsync());

    [Fact]
    public async Task ToAsyncEnumerable_keeps_order() =>
        Assert.Equal([9, 7, 8, 9], await AsyncSamples.ToAsyncEnumerable().ToListAsync());

    [Fact]
    public async Task AsyncEnumerable_range() =>
        Assert.Equal([1, 4, 9, 16, 25], await AsyncSamples.Range().ToListAsync());

    [Fact]
    public async Task Zip_two_streams() =>
        Assert.Equal(["1. gold", "2. silver", "3. bronze"], await AsyncSamples.Zip().ToListAsync());

    [Fact]
    public async Task Distinct_on_a_stream() =>
        Assert.Equal(
            ["Completed", "Shipped", "Pending", "Awaiting Payment", "Cancelled", "Declined"],
            await AsyncSamples.Distinct().ToListAsync());

    [Fact]
    public async Task WhenAll_keeps_source_order() =>
        Assert.Equal(["Alice: 3", "Bob: 2", "Carla: 3", "Dan: 2", "Eva: 0"], await AsyncSamples.WhenAll());

    [Fact]
    public async Task Throttle_processes_batches_of_two() =>
        Assert.Equal(
            ["Batch 1: Alice=3, Bob=2", "Batch 2: Carla=3, Dan=2", "Batch 3: Eva=0"],
            await AsyncSamples.Throttle());

    // PLINQ

    [Fact]
    public void AsParallel_counts_multiples_of_seven() =>
        Assert.Equal(142, ParallelSamples.AsParallel());

    [Fact]
    public void Parallel_results_have_the_same_elements() =>
        Assert.Equal("Same elements: True", ParallelSamples.OrderNotGuaranteed().First());

    [Fact]
    public void AsOrdered_keeps_source_order() =>
        Assert.Equal([1, 4, 9, 16, 25], ParallelSamples.AsOrdered());

    [Fact]
    public void Parallel_sum() =>
        Assert.Equal(500_000_500_000L, ParallelSamples.Sum());

    [Fact]
    public void Parallel_aggregate_finds_min_and_max() =>
        Assert.Equal("Min: 1, Max: 100", ParallelSamples.Aggregate());

    [Fact]
    public void Degree_of_parallelism_is_respected() =>
        Assert.Equal("At most 2 threads: True", ParallelSamples.DegreeOfParallelism());

    [Fact]
    public void ForAll_processes_every_item() =>
        Assert.Equal(Enumerable.Range(1, 10).Select(n => n * n), ParallelSamples.ForAll());

    [Fact]
    public void Exceptions_are_wrapped_in_AggregateException() =>
        Assert.Equal("AggregateException: bad item 5", ParallelSamples.Exceptions());

    [Fact]
    public void Cancelled_query_throws() =>
        Assert.Equal("Cancelled: OperationCanceledException", ParallelSamples.Cancellation());

    [Fact]
    public void AsSequential_indexes_in_order() =>
        Assert.Equal(["0: 2", "1: 4", "2: 6", "3: 8", "4: 10"], ParallelSamples.AsSequential());

    [Fact]
    public void ForceParallelism_then_sort() =>
        Assert.Equal(Enumerable.Range(1, 10).Select(n => n * 10), ParallelSamples.ForceParallelism());

    [Fact]
    public void Parallel_groupby_sorted() =>
        Assert.Equal(
            ["Awaiting Payment: 2", "Cancelled: 1", "Completed: 3", "Declined: 1", "Pending: 2", "Shipped: 2"],
            ParallelSamples.GroupBy());

    [Fact]
    public void Merge_options_then_sort() =>
        Assert.Equal(Enumerable.Range(1, 10).Select(n => n * 10), ParallelSamples.MergeOptions());

    [Fact]
    public void Primes_below_one_hundred_thousand() =>
        Assert.Equal(9592, ParallelSamples.CountPrimes());
}
