namespace LinqSamples.Tests;

/// <summary>Pins the results of the extra operators, tree/graph and combinatorics samples.</summary>
public class OperatorGraphCombinatoricsTests
{
    // More custom operators

    [Fact]
    public void Lag_pairs_with_previous() =>
        Assert.Equal(
            ["5 (previous 0)", "3 (previous 5)", "9 (previous 3)", "1 (previous 9)", "7 (previous 1)"],
            MoreOperatorSamples.Lag());

    [Fact]
    public void Lead_pairs_with_next() =>
        Assert.Equal(["5 (next 3)", "3 (next 9)", "9 (next 1)", "1 (next 7)", "7 (next 0)"], MoreOperatorSamples.Lead());

    [Fact]
    public void Lag_with_larger_offset()
    {
        Assert.Equal([(1, 0), (2, 0), (3, 1), (4, 2)], new[] { 1, 2, 3, 4 }.Lag(2, 0));
        Assert.Equal([(1, 3), (2, 4), (3, 0), (4, 0)], new[] { 1, 2, 3, 4 }.Lead(2, 0));
    }

    [Fact]
    public void Partition_splits_in_one_pass() =>
        Assert.Equal(["Even: 8, 2, 4", "Odd:  5, 3, 9, 1, 7, 3, 9"], MoreOperatorSamples.Partition());

    [Fact]
    public void SplitWhen_builds_ranges() =>
        Assert.Equal("1-3, 7-8, 10", MoreOperatorSamples.SplitWhen());

    [Fact]
    public void TakeEvery_third() =>
        Assert.Equal([1, 4, 7, 10], MoreOperatorSamples.TakeEvery());

    [Fact]
    public void MinMax_in_one_pass()
    {
        Assert.Equal("Min: 1, Max: 9", MoreOperatorSamples.MinMax());
        Assert.Throws<InvalidOperationException>(() => Array.Empty<int>().MinMax());
    }

    [Fact]
    public void Run_length_round_trips()
    {
        Assert.Equal(["Encoded: a3b1c2d4", "Decoded: aaabccdddd"], MoreOperatorSamples.RunLength());
        Assert.Empty(Array.Empty<int>().RunLength());
    }

    [Fact]
    public void Pad_fills_to_width()
    {
        Assert.Equal([1, 2, 0, 0], MoreOperatorSamples.Pad());
        Assert.Equal([1, 2, 3], new[] { 1, 2, 3 }.Pad(2, 0));
    }

    [Fact]
    public void FindIndex_returns_minus_one_when_missing() =>
        Assert.Equal(["First > 8 at index:   2", "First > 100 at index: -1"], MoreOperatorSamples.FindIndex());

    [Fact]
    public void InsertAt_middle_and_end()
    {
        Assert.Equal([1, 2, 3, 4], MoreOperatorSamples.InsertAt());
        Assert.Equal([1, 2, 3], new[] { 1, 2 }.InsertAt(2, 3));
        Assert.Equal([0, 1], new[] { 1 }.InsertAt(0, 0));
    }

    [Fact]
    public void AtLeast_short_circuits() =>
        Assert.Equal(["Numbers has at least 11:   False", "Infinite has at least 5:   True"], MoreOperatorSamples.AtLeast());

    [Fact]
    public void Choose_keeps_parsed_values() =>
        Assert.Equal([1, 3], MoreOperatorSamples.Choose());

    [Fact]
    public void Memoize_runs_the_source_once() =>
        Assert.Equal(["Plain query: 20 evaluations", "Memoized:    10 evaluations"], MoreOperatorSamples.Memoize());

    [Fact]
    public void Memoize_supports_partial_then_full_enumeration()
    {
        var pulls = 0;
        var cached = Enumerable.Range(1, 5).Select(n => { pulls++; return n; }).Memoize();
        Assert.Equal([1, 2], cached.Take(2));
        Assert.Equal([1, 2, 3, 4, 5], cached);
        Assert.Equal(5, pulls);
    }

    [Fact]
    public void ZipLongest_pads_the_shorter_side() =>
        Assert.Equal(["(1, 10)", "(2, 0)", "(3, 0)"], MoreOperatorSamples.ZipLongest());

    [Fact]
    public void MaxByAll_returns_ties() =>
        Assert.Equal(["MaxBy:    #9", "MaxByAll: #9, #10, #11"], MoreOperatorSamples.MaxByAll());

    [Fact]
    public void Cycle_repeats_and_handles_empty()
    {
        Assert.Equal("RGBRGBR", MoreOperatorSamples.Cycle());
        Assert.Empty(Array.Empty<int>().Cycle());
    }

    // Trees & graphs

    [Fact]
    public void Tree_depth() =>
        Assert.Equal(3, GraphSamples.TreeDepth());

    [Fact]
    public void Chain_of_managers() =>
        Assert.Equal("Ken -> Linus -> Grace", GraphSamples.ChainOfManagers());

    [Fact]
    public void Count_all_reports() =>
        Assert.Equal(["Grace: 4", "Linus: 2", "Ada: 0", "Ken: 0", "Barbara: 0"], GraphSamples.CountAllReports());

    [Fact]
    public void Leaves() =>
        Assert.Equal(["Ada", "Ken", "Barbara"], GraphSamples.Leaves());

    [Fact]
    public void Level_order() =>
        Assert.Equal(["0: Grace", "1: Linus, Ada", "2: Ken, Barbara"], GraphSamples.LevelOrder());

    [Fact]
    public void Root_to_leaf_paths() =>
        Assert.Equal(["Grace/Linus/Ken", "Grace/Linus/Barbara", "Grace/Ada"], GraphSamples.RootToLeafPaths());

    [Fact]
    public void File_tree_totals() =>
        Assert.Equal(["Files:      4", "Total size: 38", "Largest:    b.cs (20)"], GraphSamples.FileTreeTotals());

    [Fact]
    public void Breadth_first_search() =>
        Assert.Equal(["Skopje", "Belgrade", "Sofia", "Budapest", "Bucharest", "Vienna"], GraphSamples.BreadthFirstSearch());

    [Fact]
    public void Depth_first_search() =>
        Assert.Equal(["Skopje", "Belgrade", "Budapest", "Vienna", "Bucharest", "Sofia"], GraphSamples.DepthFirstSearch());

    [Fact]
    public void Shortest_path() =>
        Assert.Equal("Skopje -> Belgrade -> Budapest -> Vienna", GraphSamples.ShortestPath());

    [Fact]
    public void Connected_components() =>
        Assert.Equal(["Skopje, Belgrade, Sofia, Budapest, Bucharest, Vienna", "Lisbon, Madrid"], GraphSamples.ConnectedComponents());

    [Fact]
    public void Topological_sort_in_stages() =>
        Assert.Equal(["1: restore", "2: compile, docs", "3: test", "4: pack"], GraphSamples.TopologicalSort());

    [Fact]
    public void Cycle_detection() =>
        Assert.Equal(["Ran:         d", "Cycle among: a, b, c"], GraphSamples.CycleDetection());

    // Combinatorics

    [Fact]
    public void Subsets() =>
        Assert.Equal(["{}", "{a}", "{b}", "{a,b}", "{c}", "{a,c}", "{b,c}", "{a,b,c}"], CombinatoricsSamples.Subsets());

    [Fact]
    public void Permutations() =>
        Assert.Equal(["123", "132", "213", "231", "312", "321"], CombinatoricsSamples.Permutations());

    [Fact]
    public void Combinations() =>
        Assert.Equal(["ab", "ac", "ad", "bc", "bd", "cd"], CombinatoricsSamples.Combinations());

    [Fact]
    public void Cartesian_product() =>
        Assert.Equal(["S-Red-Cotton", "S-Blue-Cotton", "M-Red-Cotton", "M-Blue-Cotton"], CombinatoricsSamples.CartesianProduct());

    [Fact]
    public void Subset_sum() =>
        Assert.Equal(["4 + 5", "3 + 4 + 2"], CombinatoricsSamples.SubsetSum());

    [Fact]
    public void Coin_change() =>
        Assert.Equal(10L, CombinatoricsSamples.CoinChange());

    [Fact]
    public void Unordered_pairs() =>
        Assert.Equal(["Pairs: 10", "Alice-Bob", "Alice-Carla", "Alice-Dan"], CombinatoricsSamples.UnorderedPairs());

    [Fact]
    public void Dice_sums()
    {
        var sums = CombinatoricsSamples.DiceSums().ToList();
        Assert.Equal(11, sums.Count);
        Assert.Equal("7: 6", sums[5]);
    }

    [Fact]
    public void Derangements_of_four() =>
        Assert.Equal(9, CombinatoricsSamples.Derangements());

    [Fact]
    public void Distinct_anagrams() =>
        Assert.Equal(["aab", "aba", "baa"], CombinatoricsSamples.DistinctAnagrams());

    [Fact]
    public void Gray_code() =>
        Assert.Equal(["000", "001", "011", "010", "110", "111", "101", "100"], CombinatoricsSamples.GrayCode());

    [Fact]
    public void Integer_partitions_of_five() =>
        Assert.Equal(["5", "4+1", "3+2", "3+1+1", "2+2+1", "2+1+1+1", "1+1+1+1+1"], CombinatoricsSamples.IntegerPartitions());
}
