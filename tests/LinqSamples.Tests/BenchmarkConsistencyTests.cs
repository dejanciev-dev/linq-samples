using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace LinqSamples.Tests;

/// <summary>
/// A benchmark comparison is only fair when every variant computes the same answer.
/// These tests run each benchmark once, for every [Params] value, and compare the results.
/// </summary>
public class BenchmarkConsistencyTests
{
    public static TheoryData<string> BenchmarkClasses()
    {
        var data = new TheoryData<string>();
        foreach (var type in BenchmarkTypes())
        {
            data.Add(type.Name);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(BenchmarkClasses))]
    public void All_variants_return_the_same_result(string className)
    {
        var type = BenchmarkTypes().Single(t => t.Name == className);
        var paramsProperty = type.GetProperties().Single(p => p.GetCustomAttribute<ParamsAttribute>() is not null);
        var setup = type.GetMethods().Single(m => m.GetCustomAttribute<GlobalSetupAttribute>() is not null);
        var benchmarks = type.GetMethods().Where(m => m.GetCustomAttribute<BenchmarkAttribute>() is not null).ToList();

        Assert.True(benchmarks.Count >= 2);

        foreach (var value in paramsProperty.GetCustomAttribute<ParamsAttribute>()!.Values)
        {
            var instance = Activator.CreateInstance(type)!;
            paramsProperty.SetValue(instance, value);
            setup.Invoke(instance, null);

            var results = benchmarks.Select(b => (b.Name, Result: b.Invoke(instance, null))).ToList();
            foreach (var (name, result) in results.Skip(1))
            {
                Assert.True(Equals(results[0].Result, result),
                    $"{className}.{name} returned {result}, expected {results[0].Result} (N = {value})");
            }
        }
    }

    private static IEnumerable<Type> BenchmarkTypes() =>
        typeof(LinqSamples.Benchmarks.AnyVsCountBenchmarks).Assembly.GetTypes()
            .Where(t => t.GetMethods().Any(m => m.GetCustomAttribute<BenchmarkAttribute>() is not null));
}
