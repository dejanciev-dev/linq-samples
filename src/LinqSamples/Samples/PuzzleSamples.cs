namespace LinqSamples.Samples;

/// <summary>Classic coding-interview problems, each solved with a short LINQ pipeline.</summary>
[SampleCategory("Interview puzzles", 18)]
public static class PuzzleSamples
{
    [Sample("FizzBuzz", Description = "A tuple switch over the two remainders.")]
    public static IEnumerable<string> FizzBuzz() =>
        Enumerable.Range(1, 15).Select(n => (n % 3, n % 5) switch
        {
            (0, 0) => "FizzBuzz",
            (0, _) => "Fizz",
            (_, 0) => "Buzz",
            _ => n.ToString(),
        });

    [Sample("Palindromes", Description = "Compare letters from both ends, ignoring case and punctuation.")]
    public static IEnumerable<string> Palindromes()
    {
        static bool IsPalindrome(string s)
        {
            var letters = s.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray();
            return Enumerable.Range(0, letters.Length / 2).All(i => letters[i] == letters[^(i + 1)]);
        }

        string[] candidates = ["level", "LINQ", "Racecar", "A man, a plan, a canal: Panama"];
        return candidates.Select(s => $"{s}: {IsPalindrome(s)}");
    }

    [Sample("Group anagrams", Description = "Words with the same sorted letters are anagrams.")]
    public static IEnumerable<string> GroupAnagrams()
    {
        string[] words = ["listen", "silent", "enlist", "google", "banana", "inlets", "gogole"];
        return words.GroupBy(w => string.Concat(w.Order()))
                    .Where(g => g.Count() > 1)
                    .Select(g => string.Join(", ", g));
    }

    [Sample("Primes", Description = "Trial division up to the square root, as a query.")]
    public static IEnumerable<int> Primes() =>
        Enumerable.Range(2, 49).Where(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(d => n % d != 0));

    [Sample("Two sum", Description = "All pairs of distinct positions that add up to a target.")]
    public static IEnumerable<string> TwoSum()
    {
        int[] values = [2, 7, 11, 15, 1, 8];
        const int target = 9;
        return from a in values.Index()
               from b in values.Index()
               where a.Index < b.Index && a.Item + b.Item == target
               select $"{a.Item} + {b.Item}";
    }

    [Sample("Missing number", Description = "Expected sum minus actual sum.")]
    public static int MissingNumber()
    {
        int[] values = [3, 0, 1, 5, 2];
        return Enumerable.Range(0, values.Length + 1).Sum() - values.Sum();
    }

    [Sample("First non-repeating character", Description = "GroupBy keeps first-seen order, so the first single group wins.")]
    public static char FirstUniqueCharacter() =>
        "swiss".GroupBy(c => c).First(g => g.Count() == 1).Key;

    [Sample("Digit sum", Description = "A number's digits are the characters of its string.")]
    public static int DigitSum() =>
        98765.ToString().Sum(c => c - '0');

    [Sample("Transpose a matrix", Description = "Rows become columns.")]
    public static IEnumerable<string> Transpose()
    {
        int[][] matrix = [[1, 2, 3], [4, 5, 6]];
        return Enumerable.Range(0, matrix[0].Length)
                         .Select(col => string.Join(" ", matrix.Select(row => row[col])));
    }

    [Sample("Rotate an array", Description = "Rotate left by k with Skip and Take.")]
    public static IEnumerable<int> Rotate()
    {
        int[] values = [1, 2, 3, 4, 5];
        const int k = 2;
        return values.Skip(k).Concat(values.Take(k));
    }

    [Sample("Merge sorted lists", Description = "Concat and Order - simple, though a hand-written merge is O(n).")]
    public static IEnumerable<int> MergeSorted()
    {
        int[] first = [1, 4, 7];
        int[] second = [2, 3, 8, 9];
        return first.Concat(second).Order();
    }

    [Sample("Balanced brackets", Description = "Aggregate with a string as the stack; \"!\" marks a mismatch.")]
    public static IEnumerable<string> BalancedBrackets()
    {
        static string Push(string stack, char c) => stack + c;
        static string Pop(string stack, char opener) => stack.EndsWith(opener) ? stack[..^1] : "!";

        static bool IsBalanced(string s) =>
            s.Aggregate("", (stack, c) => stack == "!" ? stack : c switch
            {
                '(' or '[' or '{' => Push(stack, c),
                ')' => Pop(stack, '('),
                ']' => Pop(stack, '['),
                '}' => Pop(stack, '{'),
                _ => stack,
            }) == "";

        string[] inputs = ["(a[b]{c})", "(]", "((x)"];
        return inputs.Select(s => $"{s}: {IsBalanced(s)}");
    }

    [Sample("Maximum subarray", Description = "Kadane's algorithm as a single Aggregate over a tuple.")]
    public static int MaximumSubarray()
    {
        int[] values = [-2, 1, -3, 4, -1, 2, 1, -5, 4];
        return values.Aggregate((Best: int.MinValue, Current: 0), (acc, n) =>
        {
            var current = Math.Max(n, acc.Current + n);
            return (Math.Max(acc.Best, current), current);
        }).Best;
    }

    [Sample("Second largest", Description = "Distinct, sort descending, skip one.")]
    public static int SecondLargest() =>
        Numbers.Distinct().OrderDescending().Skip(1).First();

    [Sample("Longest common prefix", Description = "Count leading positions where every word agrees.")]
    public static string LongestCommonPrefix()
    {
        string[] words = ["flower", "flow", "flight"];
        var length = Enumerable.Range(0, words.Min(w => w.Length))
                               .TakeWhile(i => words.All(w => w[i] == words[0][i]))
                               .Count();
        return words[0][..length];
    }

    [Sample("Most frequent element", Description = "CountBy, then MaxBy on the count.")]
    public static int MostFrequent()
    {
        int[] values = [1, 3, 2, 3, 4, 3, 2];
        return values.CountBy(n => n).MaxBy(kv => kv.Value).Key;
    }

    [Sample("Pascal's triangle", Description = "Each row from the previous one with Zip - using the static Iterate extension.")]
    public static IEnumerable<string> PascalsTriangle() =>
        IEnumerable<int[]>.Iterate([1], row => [1, .. row.Zip(row.Skip(1), (a, b) => a + b), 1])
                          .Take(5)
                          .Select(row => string.Join(" ", row));
}
