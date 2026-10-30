namespace LinqSamples.Samples;

/// <summary>
/// Recursive structures with LINQ: the org chart in <c>Employees</c>, a road map between cities,
/// a build-task dependency graph and a small file tree.
/// </summary>
[SampleCategory("Trees & graphs", 28)]
public static class GraphSamples
{
    private static readonly (string From, string To)[] Roads =
    [
        ("Skopje", "Belgrade"), ("Skopje", "Sofia"), ("Belgrade", "Budapest"), ("Sofia", "Bucharest"),
        ("Budapest", "Vienna"), ("Bucharest", "Budapest"), ("Lisbon", "Madrid"),
    ];

    [Sample("Tree depth", Description = "Recursion plus Max - how many levels does the org chart have?")]
    public static int TreeDepth()
    {
        static int Depth(int? managerId) =>
            Employees.Where(e => e.ManagerId == managerId).Select(e => 1 + Depth(e.Id)).DefaultIfEmpty(0).Max();

        return Depth(null);
    }

    [Sample("Chain of managers", Description = "Walk up the tree with the static Iterate extension until there is no parent.")]
    public static string ChainOfManagers()
    {
        var byId = Employees.ToDictionary(e => e.Id);
        var chain = IEnumerable<Employee?>.Iterate(byId[4], e => e?.ManagerId is int id ? byId[id] : null)
                                          .TakeWhile(e => e is not null)
                                          .Select(e => e!.Name);
        return string.Join(" -> ", chain);
    }

    [Sample("Count all reports", Description = "Direct and indirect reports per employee, recursively.")]
    public static IEnumerable<string> CountAllReports()
    {
        static int Reports(int id) =>
            Employees.Where(e => e.ManagerId == id).Sum(e => 1 + Reports(e.Id));

        return Employees.Select(e => $"{e.Name}: {Reports(e.Id)}");
    }

    [Sample("Leaves", Description = "Nodes without children - employees nobody reports to.")]
    public static IEnumerable<string> Leaves() =>
        Employees.Where(e => !Employees.Any(r => r.ManagerId == e.Id)).Select(e => e.Name);

    [Sample("Level order", Description = "Breadth-first, one level at a time: each level is the children of the previous one.")]
    public static IEnumerable<string> LevelOrder()
    {
        var roots = Employees.Where(e => e.ManagerId is null).ToList();
        return IEnumerable<List<Employee>>.Iterate(roots, level => Employees.Where(e => level.Any(m => m.Id == e.ManagerId)).ToList())
                                          .TakeWhile(level => level.Count > 0)
                                          .Select((level, depth) => $"{depth}: {string.Join(", ", level.Select(e => e.Name))}");
    }

    [Sample("Root-to-leaf paths", Description = "Every path from the top of the tree to a leaf.")]
    public static IEnumerable<string> RootToLeafPaths()
    {
        IEnumerable<string> Paths(Employee employee, string prefix)
        {
            var path = prefix + employee.Name;
            var reports = Employees.Where(e => e.ManagerId == employee.Id).ToList();
            return reports.Count == 0 ? [path] : reports.SelectMany(r => Paths(r, path + "/"));
        }

        return Employees.Where(e => e.ManagerId is null).SelectMany(root => Paths(root, ""));
    }

    [Sample("File tree totals", Description = "Flatten a nested tree with a recursive SelectMany, then aggregate.")]
    public static IEnumerable<string> FileTreeTotals()
    {
        FileNode tree = new("repo", 0,
        [
            new("src", 0,
            [
                new("a.cs", 10, []),
                new("b.cs", 20, []),
                new("lib", 0, [new("c.cs", 5, [])]),
            ]),
            new("readme.md", 3, []),
        ]);

        static IEnumerable<FileNode> All(FileNode node) => node.Children.SelectMany(All).Prepend(node);

        var files = All(tree).Where(n => n.Children.Count == 0).ToList();
        var largest = files.MaxBy(f => f.Size)!;
        return [$"Files:      {files.Count}", $"Total size: {files.Sum(f => f.Size)}", $"Largest:    {largest.Name} ({largest.Size})"];
    }

    [Sample("Breadth-first search", Description = "Visit cities nearest-first with a queue; neighbors come from a lookup.")]
    public static IEnumerable<string> BreadthFirstSearch() =>
        BreadthFirst(RoadMap(), "Skopje");

    [Sample("Depth-first search", Description = "Follow one road as far as it goes before backtracking.")]
    public static IEnumerable<string> DepthFirstSearch()
    {
        var map = RoadMap();
        List<string> visited = [];
        void Visit(string city)
        {
            visited.Add(city);
            foreach (var next in map[city])
            {
                if (!visited.Contains(next))
                {
                    Visit(next);
                }
            }
        }

        Visit("Skopje");
        return visited;
    }

    [Sample("Shortest path", Description = "BFS remembers each city's parent; walk the parents back from the target.")]
    public static string ShortestPath()
    {
        var map = RoadMap();
        var parents = new Dictionary<string, string?> { ["Skopje"] = null };
        var queue = new Queue<string>(["Skopje"]);
        while (queue.TryDequeue(out var city) && city != "Vienna")
        {
            foreach (var next in map[city].Where(n => !parents.ContainsKey(n)))
            {
                parents[next] = city;
                queue.Enqueue(next);
            }
        }

        var path = IEnumerable<string?>.Iterate("Vienna", c => parents[c!]).TakeWhile(c => c is not null).Reverse();
        return string.Join(" -> ", path);
    }

    [Sample("Connected components", Description = "Repeat BFS from any city not reached yet.")]
    public static IEnumerable<string> ConnectedComponents()
    {
        var map = RoadMap();
        var remaining = map.Select(g => g.Key).ToList();
        List<string> components = [];
        while (remaining.Count > 0)
        {
            var component = BreadthFirst(map, remaining[0]);
            remaining.RemoveAll(component.Contains);
            components.Add(string.Join(", ", component));
        }

        return components;
    }

    [Sample("Topological sort", Description = "Run each build step once its dependencies are done; steps in the same stage could run in parallel.")]
    public static IEnumerable<string> TopologicalSort()
    {
        var tasks = new Dictionary<string, string[]>
        {
            ["restore"] = [],
            ["compile"] = ["restore"],
            ["test"] = ["compile"],
            ["pack"] = ["compile", "test"],
            ["docs"] = ["restore"],
        };
        return Stages(tasks).Stages.Select((stage, i) => $"{i + 1}: {string.Join(", ", stage)}");
    }

    [Sample("Cycle detection", Description = "If no step is ever ready, the remaining steps depend on each other.")]
    public static IEnumerable<string> CycleDetection()
    {
        var tasks = new Dictionary<string, string[]>
        {
            ["a"] = ["c"],
            ["b"] = ["a"],
            ["c"] = ["b"],
            ["d"] = [],
        };
        var (stages, blocked) = Stages(tasks);
        return [$"Ran:         {string.Join(", ", stages.SelectMany(s => s))}", $"Cycle among: {string.Join(", ", blocked)}"];
    }

    private static ILookup<string, string> RoadMap() =>
        Roads.SelectMany(r => new[] { r, (From: r.To, To: r.From) }).ToLookup(r => r.From, r => r.To);

    private static List<string> BreadthFirst(ILookup<string, string> map, string start)
    {
        List<string> visited = [start];
        var queue = new Queue<string>([start]);
        while (queue.TryDequeue(out var city))
        {
            foreach (var next in map[city].Where(n => !visited.Contains(n)).ToList())
            {
                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        return visited;
    }

    private static (List<List<string>> Stages, List<string> Blocked) Stages(Dictionary<string, string[]> dependencies)
    {
        List<string> done = [];
        List<List<string>> stages = [];
        while (true)
        {
            var ready = dependencies.Keys.Where(t => !done.Contains(t) && dependencies[t].All(done.Contains)).ToList();
            if (ready.Count == 0)
            {
                break;
            }

            stages.Add(ready);
            done.AddRange(ready);
        }

        return (stages, dependencies.Keys.Except(done).ToList());
    }
}

public sealed record FileNode(string Name, int Size, IReadOnlyList<FileNode> Children);
