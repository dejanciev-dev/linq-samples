namespace LinqSamples.Samples;

[SampleCategory("Partitioning", 4)]
public static class PartitioningSamples
{
    [Sample("Take / Skip")]
    public static IEnumerable<int> TakeSkip() =>
        Numbers.Skip(2).Take(3);

    [Sample("Take with a range (.NET 6+)", Description = "Ranges work with Take, including from-the-end indexes.")]
    public static IEnumerable<int> TakeRange() =>
        Numbers.Take(^3..);

    [Sample("TakeLast / SkipLast")]
    public static IEnumerable<int> TakeLast() =>
        Numbers.SkipLast(1).TakeLast(3);

    [Sample("TakeWhile", Description = "Stop at the first element that fails the predicate (SkipWhile is the mirror image).")]
    public static IEnumerable<int> TakeWhile() =>
        Numbers.TakeWhile(n => n != 1);

    [Sample("SkipWhile", Description = "Skip elements until the predicate first fails, then keep everything after it.")]
    public static IEnumerable<int> SkipWhile() =>
        Numbers.SkipWhile(n => n != 1);

    [Sample("Chunk (.NET 6+)", Description = "Split a sequence into fixed-size batches.")]
    public static IEnumerable<int[]> Chunk() =>
        Numbers.Chunk(4);

    [Sample("Paging", Description = "The classic Skip/Take page helper.")]
    public static IEnumerable<string> Paging()
    {
        const int pageSize = 3;
        const int page = 2; // 1-based
        return Products.OrderBy(p => p.Id)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .Select(p => p.Name);
    }
}
