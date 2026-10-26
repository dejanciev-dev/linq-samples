using System.Reflection;

namespace LinqSamples.Infrastructure;

public sealed record SampleInfo(string Category, string Title, string? Description, MethodInfo Method)
{
    public object? Run() => Method.Invoke(null, null);
}

public sealed record SampleCategory(string Name, int Order, IReadOnlyList<SampleInfo> Samples);

/// <summary>Discovers every <see cref="SampleAttribute"/> method in this assembly.</summary>
public static class SampleCatalog
{
    public static IReadOnlyList<SampleCategory> Categories { get; } = Discover();

    private static List<SampleCategory> Discover() =>
        (from type in typeof(SampleCatalog).Assembly.GetTypes()
         let category = type.GetCustomAttribute<SampleCategoryAttribute>()
         where category is not null
         orderby category.Order
         let samples = (from method in type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                        let sample = method.GetCustomAttribute<SampleAttribute>()
                        where sample is not null
                        orderby method.MetadataToken // declaration order
                        select new SampleInfo(category.Name, sample.Title, sample.Description, method))
                       .ToList()
         select new SampleCategory(category.Name, category.Order, samples))
        .ToList();
}
