# LINQ Samples

[![CI](https://github.com/dejanciev-dev/linq-samples/actions/workflows/ci.yml/badge.svg)](https://github.com/dejanciev-dev/linq-samples/actions/workflows/ci.yml)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)
![C# 14](https://img.shields.io/badge/C%23-14-239120?logo=csharp)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A hands-on collection of **150 runnable LINQ examples** in C#, from the basics (`Where`, `Select`, `OrderBy`) to the newest operators in .NET 9 and .NET 10 (`CountBy`, `AggregateBy`, `Index`, `LeftJoin`, `RightJoin`, `Sequence`), plus custom operators written with **C# 14 extension members** and **EF Core** samples that show the SQL each query becomes.

Every sample is a small, self-contained method that works on the same tiny in-memory data set, so the output is easy to check by eye. Every sample is also covered by unit tests.

```text
> Order by specific values
  Sort by a custom, business-defined sequence instead of alphabetically.
  #4 Awaiting Payment
  #10 Awaiting Payment
  #3 Pending
  ...
  #8 Completed
```

## Quick start

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
git clone https://github.com/dejanciev-dev/linq-samples.git
cd linq-samples

dotnet run --project src/LinqSamples.App               # run every sample
dotnet run --project src/LinqSamples.App -- --list     # list all samples
dotnet run --project src/LinqSamples.App -- joins      # run samples matching "joins"

dotnet test                                            # run the tests
```

## Samples

| # | Category | Highlights | Source |
|---|----------|------------|--------|
| 1 | Filtering | `Where`, indexed `Where`, `OfType`, filtering nulls, `IN (...)` with `Contains`, property patterns, date ranges | [FilteringSamples.cs](src/LinqSamples/Samples/FilteringSamples.cs) |
| 2 | Projection | `Select`, tuples, anonymous types, switch expressions, lookup dictionaries, `SelectMany` (with index), cross product, 3-way `Zip`, `Cast` | [ProjectionSamples.cs](src/LinqSamples/Samples/ProjectionSamples.cs) |
| 3 | Ordering | `OrderBy`/`ThenBy`, `Order`, custom comparers, computed keys, stable sorting, nulls last, natural sort, **order by specific values** | [OrderingSamples.cs](src/LinqSamples/Samples/OrderingSamples.cs) |
| 4 | Partitioning | `Take`/`Skip`, ranges `Take(^3..)`, `TakeWhile`/`SkipWhile`, `Chunk`, paging, last page | [PartitioningSamples.cs](src/LinqSamples/Samples/PartitioningSamples.cs) |
| 5 | Grouping | `GroupBy`, result selectors, buckets, `HAVING`, date parts, composite keys, `CountBy`, `AggregateBy`, `ToLookup` | [GroupingSamples.cs](src/LinqSamples/Samples/GroupingSamples.cs) |
| 6 | Joins | inner, `GroupJoin`, `LeftJoin`, `RightJoin`, classic left join, full outer join, anti join, non-equi join, composite keys, multi-table, self join | [JoinSamples.cs](src/LinqSamples/Samples/JoinSamples.cs) |
| 7 | Set operations | `Distinct(By)`, `Union(By)`, `Intersect(By)`, `Except(By)`, `SequenceEqual` | [SetSamples.cs](src/LinqSamples/Samples/SetSamples.cs) |
| 8 | Aggregation | `Sum`/`Min`/`Max`/`Average`, `MinBy`/`MaxBy`, `Aggregate` (with seed and result selector), running totals, empty-sequence pitfalls | [AggregationSamples.cs](src/LinqSamples/Samples/AggregationSamples.cs) |
| 9 | Elements & quantifiers | `First`/`Single`, `SingleOrDefault`, `FirstOrDefault(default)`, `ElementAtOrDefault`, `ElementAt(^1)`, `Any`/`All`, `Contains` with a comparer, `TryGetNonEnumeratedCount` | [ElementSamples.cs](src/LinqSamples/Samples/ElementSamples.cs) |
| 10 | Generation & conversion | `Range`, `Repeat`, `Sequence`, `Index`, `ToDictionary` (and duplicate keys), `ToHashSet`, generating dates, multiplication table, `Shuffle` | [GenerationSamples.cs](src/LinqSamples/Samples/GenerationSamples.cs) |
| 11 | Query syntax | `let` (several), `group … into`, join then group, `join … into`, multi-key `orderby`, multiple `from`, side by side with method syntax | [QuerySyntaxSamples.cs](src/LinqSamples/Samples/QuerySyntaxSamples.cs) |
| 12 | Deferred execution | lazy evaluation, streaming order, snapshots, multiple enumeration, captured variables, deferred exceptions, short-circuiting, infinite sequences | [DeferredExecutionSamples.cs](src/LinqSamples/Samples/DeferredExecutionSamples.cs) |
| 13 | Custom operators (C# 14) | `WhereIf`, `Pairwise`, `Scan`, `Window`, `DistinctUntilChanged`, `Interleave`, an extension *property* and a *static* extension member built with `extension(...)` blocks | [CustomOperators.cs](src/LinqSamples/Samples/CustomOperators.cs) |
| 14 | Real-world recipes | top N per group, latest per group, revenue report, percentage of total, pivot table, duplicates, gaps, dense rank, median, list diff, word frequency, tree flattening, batching | [RecipeSamples.cs](src/LinqSamples/Samples/RecipeSamples.cs) |
| 15 | EF Core (IQueryable) | `ToQueryString`, navigation joins, `Include`, `LeftJoin` to SQL, untranslatable code, `AsNoTracking`, `ExecuteUpdate`/`ExecuteDelete` | [EfCoreSamples.cs](src/LinqSamples/Samples/EfCore/EfCoreSamples.cs) |

## What's new in LINQ (by .NET version)

| .NET | Additions shown in this repo |
|------|------------------------------|
| 6 | `Chunk`, `DistinctBy`/`IntersectBy`/`ExceptBy`, `MinBy`/`MaxBy`, `Take(Range)`, `ElementAt(Index)`, `FirstOrDefault(default)`, 3-way `Zip`, `TryGetNonEnumeratedCount` |
| 7 | `Order` |
| 9 | `CountBy`, `AggregateBy`, `Index` |
| 10 | `LeftJoin`, `RightJoin`, `Sequence`, `Shuffle` |

## Example: C# 14 extension members

```csharp
public static class CustomOperators
{
    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<T> WhereIf(bool condition, Func<T, bool> predicate) =>
            condition ? source.Where(predicate) : source;

        public bool IsEmpty => !source.Any();   // extension property
    }
}

var berliners = Customers.WhereIf(cityFilter is not null, c => c.City == cityFilter);
```

## Benchmarks

The [`benchmarks/`](benchmarks/LinqSamples.Benchmarks) project uses [BenchmarkDotNet](https://benchmarkdotnet.org/) to measure the performance questions that come up in code reviews:

| Benchmark | Question |
|-----------|----------|
| [AnyVsCount](benchmarks/LinqSamples.Benchmarks/AnyVsCountBenchmarks.cs) | `Any(pred)` vs `Count(pred) > 0`: how much does stopping early save? |
| [LinqVsLoop](benchmarks/LinqSamples.Benchmarks/LinqVsLoopBenchmarks.cs) | What is the overhead of a `Where`/`Select`/`Sum` pipeline compared with `foreach` and `for`? |
| [Contains](benchmarks/LinqSamples.Benchmarks/ContainsBenchmarks.cs) | `List.Contains` vs `HashSet.Contains` inside `Where` |
| [MultipleEnumeration](benchmarks/LinqSamples.Benchmarks/MultipleEnumerationBenchmarks.cs) | Enumerate a lazy query twice, or materialize it once with `ToList()`? |
| [TopOne](benchmarks/LinqSamples.Benchmarks/TopOneBenchmarks.cs) | `MaxBy` vs `OrderByDescending().First()` vs sorting with `ToList()` vs a loop |
| [Join](benchmarks/LinqSamples.Benchmarks/JoinBenchmarks.cs) | .NET 10 `LeftJoin` vs `GroupJoin` + `SelectMany` + `DefaultIfEmpty` |
| [CountBy](benchmarks/LinqSamples.Benchmarks/CountByBenchmarks.cs) | .NET 9 `CountBy` vs `GroupBy().Count()` vs a dictionary loop |

```bash
# Benchmarks must run in Release
dotnet run -c Release --project benchmarks/LinqSamples.Benchmarks                         # pick interactively
dotnet run -c Release --project benchmarks/LinqSamples.Benchmarks -- --filter '*Contains*'
dotnet run -c Release --project benchmarks/LinqSamples.Benchmarks -- --filter '*' --job short
```

For a comparison to be fair, every variant must compute the same answer. `BenchmarkConsistencyTests` runs each variant for every `[Params]` value and fails if any result differs.

## Project structure

```text
├── benchmarks/
│   └── LinqSamples.Benchmarks/   # BenchmarkDotNet performance comparisons
├── src/
│   ├── LinqSamples/              # the samples (class library)
│   │   ├── Data/                 # shared in-memory data: customers, products, orders, employees
│   │   ├── Infrastructure/       # [Sample] attributes, discovery, output formatting
│   │   └── Samples/              # one file per category
│   │       └── EfCore/           # EF Core samples on an in-memory SQLite database
│   └── LinqSamples.App/          # console runner
├── tests/
│   └── LinqSamples.Tests/        # xUnit tests, one smoke test per sample plus result checks
├── Directory.Build.props         # net10.0, C# latest, nullable, warnings as errors
└── Directory.Packages.props      # central package management
```

## Adding a sample

1. Add a `public static` method to a class in `src/LinqSamples/Samples/` and mark it with `[Sample("Title", Description = "...")]`.
2. Return whatever the query produces. The runner finds and prints it automatically.
3. Optionally pin its result in `tests/LinqSamples.Tests/SampleResultTests.cs`.

New categories only need a class marked with `[SampleCategory("Name", order)]`.

## Further reading

- [LINQ overview (Microsoft Learn)](https://learn.microsoft.com/dotnet/csharp/linq/)
- [Standard query operators](https://learn.microsoft.com/dotnet/csharp/linq/standard-query-operators/)
- [What's new in C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14)

## License

This project is licensed under the [MIT License](LICENSE).
