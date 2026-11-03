namespace LinqSamples.Tests;

public class GridSampleTests
{
    [Fact]
    public void Transpose_swaps_axes() =>
        Assert.Equal(["1 4 7", "2 5 8", "3 6 9"], GridSamples.Transpose());

    [Fact]
    public void Rotate_clockwise() =>
        Assert.Equal(["7 4 1", "8 5 2", "9 6 3"], GridSamples.Rotate());

    [Fact]
    public void Diagonals_are_picked() =>
        Assert.Equal(["Main: 1 5 9", "Anti: 3 5 7"], GridSamples.Diagonals());

    [Fact]
    public void Spiral_order() =>
        Assert.Equal([1, 2, 3, 6, 9, 8, 7, 4, 5], GridSamples.Spiral());

    [Fact]
    public void Neighbours_of_centre() =>
        Assert.Equal([1, 2, 3, 4, 6, 7, 8, 9], GridSamples.Neighbours());

    [Fact]
    public void Multiply_matrices() =>
        Assert.Equal(["19 22", "43 50"], GridSamples.Multiply());

    [Fact]
    public void Blinker_flips() =>
        Assert.Equal(["(0,1)", "(1,1)", "(2,1)"], GridSamples.LifeStep());

    [Fact]
    public void Symmetry() =>
        Assert.Equal("m: True, grid: False", GridSamples.IsSymmetric());
}
