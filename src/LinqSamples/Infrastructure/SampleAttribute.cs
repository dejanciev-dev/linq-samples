namespace LinqSamples.Infrastructure;

/// <summary>Marks a static, parameterless method as a runnable LINQ sample.</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class SampleAttribute(string title) : Attribute
{
    public string Title { get; } = title;

    /// <summary>A one-line explanation printed above the sample output.</summary>
    public string? Description { get; init; }
}

/// <summary>Groups a class of samples under a category name in the runner.</summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class SampleCategoryAttribute(string name, int order) : Attribute
{
    public string Name { get; } = name;
    public int Order { get; } = order;
}
