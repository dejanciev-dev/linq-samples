namespace LinqSamples.Samples;

/// <summary>Number theory and classic sequences with LINQ.</summary>
[SampleCategory("Numbers & sequences", 35)]
public static class NumberSamples
{
    private static bool IsPrime(int n) => n > 1 && Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(d => n % d != 0);

    [Sample("Primes", Description = "Trial division with All.")]
    public static IEnumerable<int> Primes() => Enumerable.Range(1, 30).Where(IsPrime);

    [Sample("Fibonacci", Description = "An iterator block yields an infinite sequence; Take limits it.")]
    public static IEnumerable<int> Fibonacci() => Fib().Take(10);

    private static IEnumerable<int> Fib()
    {
        int a = 0, b = 1;
        while (true) { yield return a; (a, b) = (b, a + b); }
    }

    [Sample("Factorial", Description = "Aggregate multiplies a running product.")]
    public static long Factorial() => Enumerable.Range(1, 10).Aggregate(1L, (acc, n) => acc * n);

    [Sample("Divisors", Description = "Filter the range by divisibility.")]
    public static IEnumerable<int> Divisors() => Enumerable.Range(1, 36).Where(d => 36 % d == 0);

    [Sample("Perfect numbers", Description = "Sum of proper divisors equals the number.")]
    public static IEnumerable<int> PerfectNumbers() =>
        Enumerable.Range(2, 500).Where(n => Enumerable.Range(1, n - 1).Where(d => n % d == 0).Sum() == n);

    [Sample("Digits and digit sum", Description = "Treat the string as a sequence of chars.")]
    public static string DigitSum()
    {
        var digits = 98765.ToString().Select(c => c - '0');
        return $"{string.Join("+", digits)} = {digits.Sum()}";
    }

    [Sample("Collatz sequence", Description = "An iterator generates the chain until it reaches 1.")]
    public static IEnumerable<int> Collatz() =>
        CollatzGen(6);

    private static IEnumerable<int> CollatzGen(int n)
    {
        yield return n;
        while (n != 1) { n = n % 2 == 0 ? n / 2 : 3 * n + 1; yield return n; }
    }

    [Sample("GCD of a list", Description = "Aggregate folds a binary function over the list.")]
    public static int Gcd() =>
        new[] { 48, 36, 120 }.Aggregate((a, b) => { while (b != 0) (a, b) = (b, a % b); return a; });

    [Sample("Pythagorean triples", Description = "Nested from clauses with a filter.")]
    public static IEnumerable<string> Triples() =>
        from a in Enumerable.Range(1, 20)
        from b in Enumerable.Range(a, 20 - a + 1)
        from c in Enumerable.Range(b, 20 - b + 1)
        where a * a + b * b == c * c
        select $"{a},{b},{c}";

    [Sample("Binary representation", Description = "Shift and mask each bit, most significant first.")]
    public static string Binary() =>
        string.Concat(Enumerable.Range(0, 8).Select(i => (37 >> i) & 1).Reverse());

    [Sample("Triangular numbers", Description = "Closed form n(n+1)/2 over a Range.")]
    public static IEnumerable<int> Triangular() => Enumerable.Range(1, 8).Select(n => n * (n + 1) / 2);

    [Sample("Armstrong numbers", Description = "Sum of digit cubes equals the number.")]
    public static IEnumerable<int> Armstrong() =>
        Enumerable.Range(100, 900).Where(n => n.ToString().Sum(c => Math.Pow(c - '0', 3)) == n);
}
