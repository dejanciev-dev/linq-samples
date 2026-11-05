namespace LinqSamples.Tests;

public class NumberSampleTests
{
    [Fact] public void Primes_up_to_30() => Assert.Equal([2, 3, 5, 7, 11, 13, 17, 19, 23, 29], NumberSamples.Primes());
    [Fact] public void Fibonacci_first_ten() => Assert.Equal([0, 1, 1, 2, 3, 5, 8, 13, 21, 34], NumberSamples.Fibonacci());
    [Fact] public void Factorial_of_ten() => Assert.Equal(3628800L, NumberSamples.Factorial());
    [Fact] public void Divisors_of_36() => Assert.Equal([1, 2, 3, 4, 6, 9, 12, 18, 36], NumberSamples.Divisors());
    [Fact] public void Perfect_numbers() => Assert.Equal([6, 28, 496], NumberSamples.PerfectNumbers());
    [Fact] public void Digit_sum() => Assert.Equal("9+8+7+6+5 = 35", NumberSamples.DigitSum());
    [Fact] public void Collatz_of_six() => Assert.Equal([6, 3, 10, 5, 16, 8, 4, 2, 1], NumberSamples.Collatz());
    [Fact] public void Gcd_of_list() => Assert.Equal(12, NumberSamples.Gcd());
    [Fact] public void Triples_to_20() => Assert.Equal(["3,4,5", "5,12,13", "6,8,10", "8,15,17", "9,12,15", "12,16,20"], NumberSamples.Triples());
    [Fact] public void Binary_of_37() => Assert.Equal("00100101", NumberSamples.Binary());
    [Fact] public void Triangular_numbers() => Assert.Equal([1, 3, 6, 10, 15, 21, 28, 36], NumberSamples.Triangular());
    [Fact] public void Armstrong_numbers() => Assert.Equal([153, 370, 371, 407], NumberSamples.Armstrong());
}
