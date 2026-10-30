using System.Reflection;

namespace LinqSamples.Infrastructure;

public sealed record SampleInfo(string Category, string Title, string? Description, MethodInfo Method)
{
    /// <summary>Runs the sample; async results (Task&lt;T&gt;, IAsyncEnumerable&lt;T&gt;) are awaited and materialized.</summary>
    public object? Run() => AsyncResults.Materialize(Method.Invoke(null, null));
}

/// <summary>Turns async sample results into plain values so the runner and tests can stay synchronous.</summary>
internal static class AsyncResults
{
    private static readonly MethodInfo DrainMethod =
        typeof(AsyncResults).GetMethod(nameof(Drain), BindingFlags.NonPublic | BindingFlags.Static)!;

    public static object? Materialize(object? result)
    {
        if (result is Task task)
        {
            task.GetAwaiter().GetResult();
            return task.GetType().IsGenericType ? task.GetType().GetProperty("Result")!.GetValue(task) : null;
        }

        var asyncSequence = result?.GetType().GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>));
        return asyncSequence is null
            ? result
            : DrainMethod.MakeGenericMethod(asyncSequence.GetGenericArguments()).Invoke(null, [result]);
    }

    private static List<T> Drain<T>(IAsyncEnumerable<T> source) =>
        source.ToListAsync().AsTask().GetAwaiter().GetResult();
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
