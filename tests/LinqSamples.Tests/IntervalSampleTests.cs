namespace LinqSamples.Tests;

public class IntervalSampleTests
{
    [Fact] public void Overlapping_pairs() => Assert.Equal(["(9, 11)-(10, 12)", "(14, 16)-(15, 17)"], IntervalSamples.OverlappingPairs());
    [Fact] public void Merged() => Assert.Equal([(9, 12), (13, 17)], IntervalSamples.Merge());
    [Fact] public void Busy_hours() => Assert.Equal(7, IntervalSamples.BusyHours());
    [Fact] public void Free_slots() => Assert.Equal([(12, 13)], IntervalSamples.FreeSlots());
    [Fact] public void Max_concurrent() => Assert.Equal(2, IntervalSamples.MaxConcurrent());
    [Fact] public void Containing_point() => Assert.Equal(2, IntervalSamples.ContainingPoint());
    [Fact] public void Non_overlapping() => Assert.Equal([(9, 11), (13, 14), (14, 16)], IntervalSamples.MaxNonOverlapping());
    [Fact] public void Consecutive_runs() => Assert.Equal(["1-3", "7-8", "10"], IntervalSamples.ConsecutiveRuns());
    [Fact] public void Buckets() => Assert.Equal(["0-9: 2", "10-19: 2", "20-29: 3"], IntervalSamples.Buckets());
    [Fact] public void Range_chunks() => Assert.Equal(["1-4", "5-8", "9-10"], IntervalSamples.RangeChunks());
}
