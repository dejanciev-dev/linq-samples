namespace LinqSamples.Tests;

public class WindowSampleTests
{
    [Fact] public void Window_sums() => Assert.Equal([13, 12, 9, 6, 12, 11, 10, 11], WindowSamples.WindowSums());
    [Fact] public void Moving_average() => Assert.Equal(8, WindowSamples.MovingAverage().Count());
    [Fact] public void Differences() => Assert.Equal([2, 0, -3, 0, 0, 6, -7, 0, 8], WindowSamples.Differences());
    [Fact] public void Rle_encode() => Assert.Equal(["3x1", "5x2", "2x3", "8x1", "1x2", "9x1"], WindowSamples.RunLengthEncode());
    [Fact] public void Rle_decode() => Assert.Equal([7, 7, 4, 4, 4], WindowSamples.RunLengthDecode());
    [Fact] public void Longest_streak() => Assert.Equal(3, WindowSamples.LongestStreak());
    [Fact] public void Dedupe() => Assert.Equal([3, 5, 2, 8, 1, 9], WindowSamples.Dedupe());
    [Fact] public void Rising() => Assert.Equal([1, 6, 9], WindowSamples.RisingIndexes());
    [Fact] public void Split_on_zero() => Assert.Equal(["1,2", "3", "4,5,6"], WindowSamples.SplitOnZero());
    [Fact] public void Best_window() => Assert.Equal(0, WindowSamples.BestWindowStart());
}
