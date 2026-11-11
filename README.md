# LINQ Samples

[![CI](https://github.com/dejanciev-dev/linq-samples/actions/workflows/ci.yml/badge.svg)](https://github.com/dejanciev-dev/linq-samples/actions/workflows/ci.yml)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet)
![C# 14](https://img.shields.io/badge/C%23-14-239120?logo=csharp)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A hands-on collection of **893 runnable LINQ examples** in C#, from the basics (`Where`, `Select`, `OrderBy`) to the newest operators in .NET 9 and .NET 10 (`CountBy`, `AggregateBy`, `Index`, `LeftJoin`, `RightJoin`, `Sequence`), plus custom operators written with **C# 14 extension members**, **EF Core** samples that show the SQL each query becomes, LINQ applied to text, regex and classic interview puzzles, async streams (`IAsyncEnumerable`), parallel queries (PLINQ), querying XML and JSON, expression trees, queries built at runtime, trees and graphs, combinatorics, time series, statistics, dictionaries, matrices, number theory, intervals, sliding windows, rankings, batching, data cleaning, equality checks, money calculations and algorithm compositions - plus a gallery of common pitfalls shown wrong vs right.

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
| 14 | Real-world recipes | top N per group, latest per group, revenue report, percentage of total, pivot table, duplicates, gaps, dense rank, median, list diff, word frequency, tree flattening, batching | [KitchenSamples.cs](src/LinqSamples/Samples/KitchenSamples.cs) |
| 15 | EF Core (IQueryable) | `ToQueryString`, navigation joins, `Include`, `LeftJoin` to SQL, untranslatable code, `AsNoTracking`, `ExecuteUpdate`/`ExecuteDelete` | [EfCoreSamples.cs](src/LinqSamples/Samples/EfCore/EfCoreSamples.cs) |
| 16 | Strings & text | reverse words, capitalize, character frequency, CSV parsing, slugify, remove accents, natural-language lists | [TextSamples.cs](src/LinqSamples/Samples/TextSamples.cs) |
| 17 | Regex + LINQ | `Matches` into LINQ, named groups into tuples, validation, hashtags, key=value parsing, repeated words (source-generated regexes) | [RegexSamples.cs](src/LinqSamples/Samples/RegexSamples.cs) |
| 18 | Interview puzzles | FizzBuzz, palindromes, anagrams, primes, two sum, balanced brackets, maximum subarray, Pascal's triangle | [PuzzleSamples.cs](src/LinqSamples/Samples/PuzzleSamples.cs) |
| 19 | Async LINQ (IAsyncEnumerable) | .NET 10 built-in async operators, async predicates and projections, `CountAsync`/`FirstAsync`, cancellation, `Chunk` on streams, `Task.WhenAll`, throttling | [AsyncSamples.cs](src/LinqSamples/Samples/AsyncSamples.cs) |
| 20 | Parallel LINQ (PLINQ) | `AsParallel`, `AsOrdered`, parallel `Aggregate`, `WithDegreeOfParallelism`, `ForAll`, `AggregateException`, `WithCancellation`, `AsSequential`, merge options | [ParallelSamples.cs](src/LinqSamples/Samples/ParallelSamples.cs) |
| 21 | LINQ to XML | `Elements`/`Descendants`/`Ancestors`, typed casts, optional elements, namespaces, functional construction, XML-to-XML transforms, modifying documents | [XmlSamples.cs](src/LinqSamples/Samples/XmlSamples.cs) |
| 22 | LINQ over JSON | `JsonNode` queries, flattening nested arrays, optional properties, `JsonDocument`, deserialize-then-query, serializing query results, building and modifying JSON, JSON Lines | [JsonSamples.cs](src/LinqSamples/Samples/JsonSamples.cs) |
| 23 | Expression trees | `Func` vs `Expression`, inspecting and printing trees, building by hand, `ExpressionVisitor` (collect, rewrite), captured variables, `Block`/`Condition`/`Call`, the tree behind `IQueryable` | [ExpressionTreeSamples.cs](src/LinqSamples/Samples/ExpressionTreeSamples.cs) |
| 24 | Dynamic queries | optional filters, sorting and filtering by property name, operators from strings, `PredicateBuilder` (`And`/`Or`/`Not`), specification pattern, dynamic projection and grouping, query-string filters | [DynamicQuerySamples.cs](src/LinqSamples/Samples/DynamicQuerySamples.cs) |
| 25 | EF Core: query translation | string and date functions, `EF.Functions.Like`, `CASE`, `COALESCE`, `EXISTS`/`NOT EXISTS`, `UNION`/`UNION ALL`/`EXCEPT`, top 1 per group, nested collections, keyset pagination, `EF.Constant`, `TagWith` | [EfTranslationSamples.cs](src/LinqSamples/Samples/EfCore/EfTranslationSamples.cs) |
| 26 | EF Core: loading, tracking & saving | split queries, the N+1 problem, filtered `Include`, `ThenInclude`, `Find`, identity resolution, change tracking, transactions, global query filters, compiled queries, `FromSql`, `SqlQuery`, async and streaming | [EfBehaviorSamples.cs](src/LinqSamples/Samples/EfCore/EfBehaviorSamples.cs) |
| 27 | More custom operators | `Lag`/`Lead`, `Partition`, `SplitWhen`, `MinMax`, run-length encoding, `Pad`, `FindIndex`, `AtLeast`, `Choose`, `Memoize`, `ZipLongest`, `MaxByAll`, `Cycle` | [MoreOperatorSamples.cs](src/LinqSamples/Samples/MoreOperatorSamples.cs) |
| 28 | Trees & graphs | tree depth, ancestors, level order, root-to-leaf paths, file-tree totals, BFS/DFS, shortest path, connected components, topological sort, cycle detection | [GraphSamples.cs](src/LinqSamples/Samples/GraphSamples.cs) |
| 29 | Combinatorics | subsets, permutations, combinations, n-way Cartesian product, subset sum, coin change, dice sums, derangements, Gray code, integer partitions | [CombinatoricsSamples.cs](src/LinqSamples/Samples/CombinatoricsSamples.cs) |
| 30 | Pitfalls (wrong vs right) | `Count() > 0` vs `Any()`, `First` hiding duplicates, `OrderBy().OrderBy()`, unenumerated `Select`, `for`-loop closures, `List.Contains` in loops, `Distinct` on classes, `Sum` overflow, discarded `Reverse()`, disposed resources | [PitfallSamples.cs](src/LinqSamples/Samples/PitfallSamples.cs) |
| 31 | Dates & time series | ISO weeks, filling missing days with `LeftJoin`, rolling totals, day-over-day and week-over-week change, business days, streaks, overlapping and merged time ranges | [DateSamples.cs](src/LinqSamples/Samples/DateSamples.cs) |
| 32 | Statistics | mean/median/mode, standard deviation, percentiles, IQR outliers, z-scores, normalization, correlation, linear regression, histogram, CDF, geometric mean, RMSE | [StatisticsSamples.cs](src/LinqSamples/Samples/StatisticsSamples.cs) |
| 33 | Dictionaries & lookups | `ToDictionary`, `ToLookup`, filtering and inverting dictionaries, merge and diff, top N per key, `CountBy` frequencies, safe lookups, `HashSet` algebra, case-insensitive keys | [DictionarySamples.cs](src/LinqSamples/Samples/DictionarySamples.cs) |
| 34 | Matrices & grids | flatten, `Chunk`, transpose, rotate, row/column sums, diagonals, coordinates, neighbours, spiral order, matrix multiplication, `int[,]`, Game of Life step | [GridSamples.cs](src/LinqSamples/Samples/GridSamples.cs) |
| 35 | Numbers & sequences | primes, Fibonacci, factorial, divisors, perfect and Armstrong numbers, digit sum, Collatz, GCD, Pythagorean triples, binary, triangular numbers | [NumberSamples.cs](src/LinqSamples/Samples/NumberSamples.cs) |
| 36 | Intervals & scheduling | overlaps, merge intervals, free slots, sweep-line concurrency, activity selection, consecutive runs, buckets, range chunks | [IntervalSamples.cs](src/LinqSamples/Samples/IntervalSamples.cs) |
| 37 | Streaks & sliding windows | window sums, moving average, pairwise differences, run-length encode/decode, longest streak, dedupe, rising points, split on separator, best window | [WindowSamples.cs](src/LinqSamples/Samples/WindowSamples.cs) |
| 38 | Ranking & leaderboards | leaderboards, competition and dense rank, top N with ties, percentile rank, median, rank changes, weighted average | [RankingSamples.cs](src/LinqSamples/Samples/RankingSamples.cs) |
| 39 | Advanced compositions | pivot tables, running balance, sessionization, Cartesian product of N lists, power set, N-Queens, Levenshtein distance, longest common prefix | [AdvancedCompositionSamples.cs](src/LinqSamples/Samples/AdvancedCompositionSamples.cs) |
| 40 | Batching & pagination | `Chunk` batches, paging, page count, numbered pages, round-robin split, interleave, every nth item, budgeted `TakeWhile`, `SkipLast`/`TakeLast`, weight-capped batches, last/padded pages, contiguous split | [BatchingSamples.cs](src/LinqSamples/Samples/BatchingSamples.cs) |
| 41 | Data cleaning & validation | trim and drop blanks, case-insensitive distinct, title case, safe parsing, bad-input report, duplicate and missing ids, clamping | [CleaningSamples.cs](src/LinqSamples/Samples/CleaningSamples.cs) |
| 42 | Equality & comparison | `SequenceEqual`, comparers, `SetEquals`, subset, disjoint, `DistinctBy`, `MaxBy`, Hamming distance, symmetric difference | [EqualitySamples.cs](src/LinqSamples/Samples/EqualitySamples.cs) |
| 43 | Money & finance | compound interest, loan balance, net total, deposits vs withdrawals, lowest balance, first overdraft, bill splitting, shares, portfolio return, grouping by category | [FinanceSamples.cs](src/LinqSamples/Samples/FinanceSamples.cs) |
| 44 | Inventory & stock | stock value, low stock, most valuable line, reorder quantities, ranking by value, stock buckets, average price, units, value share | [InventorySamples.cs](src/LinqSamples/Samples/InventorySamples.cs) |
| 45 | Log analysis | count by level, errors, noisiest source, most frequent error, errors per minute, gaps between events, recovery after an error, error rate, levels per source | [LogSamples.cs](src/LinqSamples/Samples/LogSamples.cs) |
| 46 | Shopping cart | subtotal, item count, per-category totals, biggest line, category discount, buy 3 get 1 free, free-shipping check, cheapest per category, bulk lines, expand to units, capped coupon, top category by units | [CartSamples.cs](src/LinqSamples/Samples/CartSamples.cs) |
| 47 | Student grades | average per student, top student, passing, best per subject, letter grade counts, honor roll, both subjects, better at art, ranking, total points, median score, spread per subject, lowest score | [GradeSamples.cs](src/LinqSamples/Samples/GradeSamples.cs) |
| 48 | Library books | titles by author, available titles, longest book, pages per author, oldest book, books per decade, pages on loan, average pages | [LibrarySamples.cs](src/LinqSamples/Samples/LibrarySamples.cs) |
| 49 | Weather week | hottest day, coldest night, average rain, dry days, rainy day count, narrowest range, warmer than yesterday, total rain, above-average highs, weekend average | [WeatherSamples.cs](src/LinqSamples/Samples/WeatherSamples.cs) |
| 50 | Movies | top rated, per genre, best per genre, short movies, total minutes, longest, by year, newest per genre, high scores, per decade | [MovieSamples.cs](src/LinqSamples/Samples/MovieSamples.cs) |
| 51 | Employees | payroll, average per department, highest paid, veterans, raise, headcount, top earner, rich departments, ranking, pay gap | [EmployeeSamples.cs](src/LinqSamples/Samples/EmployeeSamples.cs) |
| 52 | Playlist | total seconds, most played, plays per artist, longest per artist, short tracks, top two, listening time, multi-track artists, length, titles | [PlaylistSamples.cs](src/LinqSamples/Samples/PlaylistSamples.cs) |
| 53 | Customer orders | paid revenue, unpaid, per customer, best customer, largest, average paid, unpaid customers, always paid, running total, big spenders | [OrderSamples.cs](src/LinqSamples/Samples/OrderSamples.cs) |
| 54 | League table | points table, leader, total goals, draws, biggest win, goals scored, goal difference, unbeaten, home wins, clean sheets | [LeagueSamples.cs](src/LinqSamples/Samples/LeagueSamples.cs) |
| 55 | Flights | cheapest, from an airport, departures, airports, longest, total hours, itineraries via join, cheapest itinerary, busiest destination, return flights, average price, short-haul, arrivals, fare per origin, price per minute, dead ends, two hops, price bands, round trip | [FlightSamples.cs](src/LinqSamples/Samples/FlightSamples.cs) |
| 56 | Contacts | duplicate phones, cities, oldest, same age, per city, average age, sort by city then name, initials, lookup with missing key, age decades, youngest, average age, names per city, over thirty, city of oldest, phone prefixes, unique owners, paging, age spread | [ContactSamples.cs](src/LinqSamples/Samples/ContactSamples.cs) |
| 57 | To-do list | open by priority, hours remaining, percent done, hours per priority, next job, urgent check, long titles, id pairs, done vs open, longest title | [JobSamples.cs](src/LinqSamples/Samples/JobSamples.cs) |
| 58 | Used cars | per brand, lowest mileage, average km, oldest, red cars, colour count, km per brand, total age, high mileage, newest per colour | [CarSamples.cs](src/LinqSamples/Samples/CarSamples.cs) |
| 59 | Hotel bookings | total nights, revenue, longest stay, repeat guests, nights per room, revenue per month, best room, average rate, long stays, guest spend ranking | [HotelSamples.cs](src/LinqSamples/Samples/HotelSamples.cs) |
| 60 | Kitchen costs | dishes, cost per dish, most expensive, shared ingredients, dishes with cheese, grams per item, count per dish, single-ingredient dishes, heaviest line, total cost | [KitchenSamples.cs](src/LinqSamples/Samples/KitchenSamples.cs) |

## What's new in LINQ (by .NET version)

| .NET | Additions shown in this repo |
|------|------------------------------|
| 6 | `Chunk`, `DistinctBy`/`IntersectBy`/`ExceptBy`, `MinBy`/`MaxBy`, `Take(Range)`, `ElementAt(Index)`, `FirstOrDefault(default)`, 3-way `Zip`, `TryGetNonEnumeratedCount` |
| 7 | `Order` |
| 9 | `CountBy`, `AggregateBy`, `Index` |
| 10 | `LeftJoin`, `RightJoin`, `Sequence`, `Shuffle`, LINQ for `IAsyncEnumerable` (System.Linq.AsyncEnumerable) |

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
│   │       └── EfCore/           # EF Core samples on an in-memory SQLite database (SQL is logged so samples can count round trips)
│   └── LinqSamples.App/          # console runner
├── tests/
│   └── LinqSamples.Tests/        # xUnit tests, one smoke test per sample plus result checks
├── Directory.Build.props         # net10.0, C# latest, nullable, warnings as errors
└── Directory.Packages.props      # central package management
```

## Adding a sample

1. Add a `public static` method to a class in `src/LinqSamples/Samples/` and mark it with `[Sample("Title", Description = "...")]`.
2. Return whatever the query produces. The runner finds and prints it automatically; `Task<T>` and `IAsyncEnumerable<T>` results are awaited first.
3. Optionally pin its result in `tests/LinqSamples.Tests/SampleResultTests.cs`.

New categories only need a class marked with `[SampleCategory("Name", order)]`.

## Further reading

- [LINQ overview (Microsoft Learn)](https://learn.microsoft.com/dotnet/csharp/linq/)
- [Standard query operators](https://learn.microsoft.com/dotnet/csharp/linq/standard-query-operators/)
- [What's new in C# 14](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14)

## License

This project is licensed under the [MIT License](LICENSE).
