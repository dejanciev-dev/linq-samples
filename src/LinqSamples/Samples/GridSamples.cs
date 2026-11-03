namespace LinqSamples.Samples;

/// <summary>Matrices and grids: flattening, transposing, rotating, neighbours and cellular automata with LINQ.</summary>
[SampleCategory("Matrices & grids", 34)]
public static class GridSamples
{
    private static readonly int[][] Grid = [[1, 2, 3], [4, 5, 6], [7, 8, 9]];

    private static string Row(IEnumerable<int> row) => string.Join(" ", row);

    [Sample("Flatten a matrix", Description = "SelectMany turns rows into one sequence.")]
    public static IEnumerable<int> Flatten() => Grid.SelectMany(r => r);

    [Sample("Chunk into a matrix", Description = "Chunk is the inverse of flattening.")]
    public static IEnumerable<string> ChunkToMatrix() =>
        Enumerable.Range(1, 9).Chunk(3).Select(Row);

    [Sample("Transpose", Description = "Swap rows and columns.")]
    public static IEnumerable<string> Transpose() =>
        Enumerable.Range(0, Grid[0].Length).Select(c => Row(Grid.Select(r => r[c])));

    [Sample("Rotate 90° clockwise", Description = "Read each column bottom-to-top.")]
    public static IEnumerable<string> Rotate() =>
        Enumerable.Range(0, Grid[0].Length).Select(c => Row(Grid.Reverse().Select(r => r[c])));

    [Sample("Row and column sums", Description = "Aggregate across either axis.")]
    public static IEnumerable<string> RowAndColumnSums()
    {
        var rows = Grid.Select(r => r.Sum());
        var cols = Enumerable.Range(0, Grid[0].Length).Select(c => Grid.Sum(r => r[c]));
        return [$"Rows: {Row(rows)}", $"Columns: {Row(cols)}"];
    }

    [Sample("Diagonals", Description = "Select with an index picks the main and anti diagonal.")]
    public static IEnumerable<string> Diagonals() =>
    [
        $"Main: {Row(Grid.Select((r, i) => r[i]))}",
        $"Anti: {Row(Grid.Select((r, i) => r[^(i + 1)]))}",
    ];

    [Sample("Find coordinates", Description = "Flatten with row and column indexes to locate cells.")]
    public static IEnumerable<string> FindCoordinates() =>
        Grid.SelectMany((r, y) => r.Select((v, x) => (x, y, v)))
            .Where(c => c.v % 4 == 0)
            .Select(c => $"{c.v} at ({c.x},{c.y})");

    [Sample("Best row", Description = "MaxBy on row sums keeps the row index.")]
    public static string BestRow()
    {
        var best = Grid.Select((r, i) => (Index: i, Sum: r.Sum())).MaxBy(t => t.Sum);
        return $"Row {best.Index} with sum {best.Sum}";
    }

    [Sample("Neighbours of a cell", Description = "Offsets via SelectMany, bounds-checked with Where.")]
    public static IEnumerable<int> Neighbours()
    {
        const int cx = 1, cy = 1;
        return (from dy in Enumerable.Range(-1, 3)
                from dx in Enumerable.Range(-1, 3)
                where (dx, dy) != (0, 0)
                let x = cx + dx
                let y = cy + dy
                where y >= 0 && y < Grid.Length && x >= 0 && x < Grid[y].Length
                select Grid[y][x]).Order();
    }

    [Sample("Spiral order", Description = "Take the first row, rotate the rest counter-clockwise, repeat.")]
    public static IEnumerable<int> Spiral()
    {
        var rows = Grid.ToList();
        while (rows.Count > 0)
        {
            foreach (var v in rows[0])
            {
                yield return v;
            }

            var rest = rows.Skip(1).ToList();
            rows = rest.Count == 0
                ? []
                : Enumerable.Range(0, rest[0].Length).Reverse().Select(c => rest.Select(r => r[c]).ToArray()).ToList();
        }
    }

    [Sample("Matrix multiplication", Description = "Row-by-column dot products with Zip and Sum.")]
    public static IEnumerable<string> Multiply()
    {
        int[][] a = [[1, 2], [3, 4]];
        int[][] b = [[5, 6], [7, 8]];
        return a.Select(row => Row(Enumerable.Range(0, b[0].Length)
            .Select(c => row.Zip(b, (x, bRow) => x * bRow[c]).Sum())));
    }

    [Sample("Symmetric matrix?", Description = "All over every (row, column) pair.")]
    public static string IsSymmetric()
    {
        int[][] m = [[1, 7, 3], [7, 4, 5], [3, 5, 6]];
        bool Symmetric(int[][] g) =>
            Enumerable.Range(0, g.Length).All(r => Enumerable.Range(0, r).All(c => g[r][c] == g[c][r]));
        return $"m: {Symmetric(m)}, grid: {Symmetric(Grid)}";
    }

    [Sample("Multidimensional array", Description = "int[,] is not IEnumerable<int[]>; Cast<int>() flattens it.")]
    public static string Rectangular()
    {
        int[,] m = { { 1, 2 }, { 3, 4 }, { 5, 6 } };
        return $"Count {m.Cast<int>().Count()}, sum {m.Cast<int>().Sum()}, max {m.Cast<int>().Max()}";
    }

    [Sample("Game of Life step", Description = "Count neighbours with SelectMany + GroupBy; a blinker flips orientation.")]
    public static IEnumerable<string> LifeStep()
    {
        HashSet<(int X, int Y)> alive = [(1, 0), (1, 1), (1, 2)];
        var next = alive
            .SelectMany(c => from dx in Enumerable.Range(-1, 3)
                             from dy in Enumerable.Range(-1, 3)
                             where (dx, dy) != (0, 0)
                             select (X: c.X + dx, Y: c.Y + dy))
            .CountBy(c => c)
            .Where(kv => kv.Value == 3 || (kv.Value == 2 && alive.Contains(kv.Key)))
            .Select(kv => kv.Key)
            .OrderBy(c => c.Y).ThenBy(c => c.X);
        return next.Select(c => $"({c.X},{c.Y})");
    }
}
