namespace LinqSamples.Tests;

/// <summary>Pins the results of the strings, regex and interview-puzzle samples.</summary>
public class TextAndPuzzleSampleTests
{
    // Strings & text

    [Fact]
    public void Reverse_words() =>
        Assert.Equal("readable code makes LINQ", TextSamples.ReverseWords());

    [Fact]
    public void Capitalize_words() =>
        Assert.Equal("Hello Linq World", TextSamples.CapitalizeWords());

    [Fact]
    public void Count_vowels() =>
        Assert.Equal(10, TextSamples.CountVowels());

    [Fact]
    public void Character_frequency() =>
        Assert.Equal(["i: 4", "s: 4", "p: 2", "m: 1"], TextSamples.CharacterFrequency());

    [Fact]
    public void Longest_word() =>
        Assert.Equal("blueberry", TextSamples.LongestWord());

    [Fact]
    public void Acronym() =>
        Assert.Equal("PNG", TextSamples.Acronym());

    [Fact]
    public void Pangram() =>
        Assert.Equal(["The quick brown fox jumps over the lazy dog: True", "Hello world: False"], TextSamples.Pangram());

    [Fact]
    public void Parse_csv() =>
        Assert.Equal(["#1 Alice (Skopje)", "#2 Bob (Berlin)"], TextSamples.ParseCsv());

    [Fact]
    public void Slugify() =>
        Assert.Equal("linq-in-net-10-what-s-new", TextSamples.Slugify());

    [Fact]
    public void Remove_accents() =>
        Assert.Equal("Creme Brulee a Skopje", TextSamples.RemoveAccents());

    [Fact]
    public void Words_by_first_letter() =>
        Assert.Equal(["a: apple, avocado, Apricot", "b: Banana, blueberry", "c: cherry", "d: date"], TextSamples.WordsByFirstLetter());

    [Fact]
    public void Join_with_and() =>
        Assert.Equal("Alice, Bob, Carla, Dan and Eva", TextSamples.JoinWithAnd());

    [Fact]
    public void Collapse_whitespace() =>
        Assert.Equal("too many spaces here", TextSamples.CollapseWhitespace());

    [Fact]
    public void Sort_by_last_name() =>
        Assert.Equal(["Grace Hopper", "Ada Lovelace", "Linus Torvalds", "Alan Turing"], TextSamples.SortByLastName());

    [Fact]
    public void Filter_log_lines() =>
        Assert.Equal(["disk full", "timeout"], TextSamples.FilterLogLines());

    // Regex + LINQ

    [Fact]
    public void Extract_emails() =>
        Assert.Equal(["alice@example.com", "bob@test.org"], RegexSamples.ExtractEmails());

    [Fact]
    public void Sum_numbers_in_text() =>
        Assert.Equal(22, RegexSamples.SumNumbers());

    [Fact]
    public void Named_groups() =>
        Assert.Equal(["2026-01-05: disk full", "2026-01-07: timeout"], RegexSamples.NamedGroups());

    [Fact]
    public void Validate_input() =>
        Assert.Equal(["SK-1234: valid", "sk-12: invalid", "BE-0001: valid", "XX-99999: invalid"], RegexSamples.ValidateInput());

    [Fact]
    public void Split_camel_case() =>
        Assert.Equal(["parse", "Http", "Request", "Header"], RegexSamples.SplitCamelCase());

    [Fact]
    public void Hashtags_are_normalized() =>
        Assert.Equal(["linq", "dotnet"], RegexSamples.Hashtags());

    [Fact]
    public void Key_value_pairs() =>
        Assert.Equal(["host: localhost", "port: 5432", "db: shop"], RegexSamples.KeyValuePairs());

    [Fact]
    public void Repeated_words() =>
        Assert.Equal(["is", "test", "the"], RegexSamples.RepeatedWords());

    // Interview puzzles

    [Fact]
    public void FizzBuzz()
    {
        var result = PuzzleSamples.FizzBuzz().ToList();
        Assert.Equal(["1", "2", "Fizz", "4", "Buzz"], result.Take(5));
        Assert.Equal("FizzBuzz", result[^1]);
    }

    [Fact]
    public void Palindromes() =>
        Assert.Equal(
            ["level: True", "LINQ: False", "Racecar: True", "A man, a plan, a canal: Panama: True"],
            PuzzleSamples.Palindromes());

    [Fact]
    public void Group_anagrams() =>
        Assert.Equal(["listen, silent, enlist, inlets", "google, gogole"], PuzzleSamples.GroupAnagrams());

    [Fact]
    public void Primes_up_to_fifty() =>
        Assert.Equal([2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47], PuzzleSamples.Primes());

    [Fact]
    public void Two_sum() =>
        Assert.Equal(["2 + 7", "1 + 8"], PuzzleSamples.TwoSum());

    [Fact]
    public void Missing_number() =>
        Assert.Equal(4, PuzzleSamples.MissingNumber());

    [Fact]
    public void First_unique_character() =>
        Assert.Equal('w', PuzzleSamples.FirstUniqueCharacter());

    [Fact]
    public void Digit_sum() =>
        Assert.Equal(35, PuzzleSamples.DigitSum());

    [Fact]
    public void Transpose() =>
        Assert.Equal(["1 4", "2 5", "3 6"], PuzzleSamples.Transpose());

    [Fact]
    public void Rotate() =>
        Assert.Equal([3, 4, 5, 1, 2], PuzzleSamples.Rotate());

    [Fact]
    public void Merge_sorted() =>
        Assert.Equal([1, 2, 3, 4, 7, 8, 9], PuzzleSamples.MergeSorted());

    [Fact]
    public void Balanced_brackets() =>
        Assert.Equal(["(a[b]{c}): True", "(]: False", "((x): False"], PuzzleSamples.BalancedBrackets());

    [Fact]
    public void Maximum_subarray() =>
        Assert.Equal(6, PuzzleSamples.MaximumSubarray());

    [Fact]
    public void Second_largest() =>
        Assert.Equal(8, PuzzleSamples.SecondLargest());

    [Fact]
    public void Longest_common_prefix() =>
        Assert.Equal("fl", PuzzleSamples.LongestCommonPrefix());

    [Fact]
    public void Most_frequent() =>
        Assert.Equal(3, PuzzleSamples.MostFrequent());

    [Fact]
    public void Pascals_triangle() =>
        Assert.Equal(["1", "1 1", "1 2 1", "1 3 3 1", "1 4 6 4 1"], PuzzleSamples.PascalsTriangle());
}
