namespace LinqSamples.Tests;

public class LibrarySampleTests
{
    [Fact] public void Titles_by_author() => Assert.Equal(["Austen:Emma,Persuasion", "Gibson:Count Zero,Neuromancer", "Herbert:Children of Dune,Dune"], LibrarySamples.TitlesByAuthor());
    [Fact] public void Available() => Assert.Equal(["Children of Dune", "Emma", "Neuromancer"], LibrarySamples.Available());
    [Fact] public void Longest() => Assert.Equal("Dune", LibrarySamples.Longest());
    [Fact] public void Pages_per_author() => Assert.Equal(["Austen:723", "Gibson:591", "Herbert:1056"], LibrarySamples.PagesPerAuthor());
    [Fact] public void Oldest() => Assert.Equal("Emma", LibrarySamples.Oldest());
    [Fact] public void Per_decade() => Assert.Equal(["1810:2", "1960:1", "1970:1", "1980:2"], LibrarySamples.PerDecade());
    [Fact] public void Pages_on_loan() => Assert.Equal(1181, LibrarySamples.PagesOnLoan());
    [Fact] public void Average_pages() => Assert.Equal(395.0, LibrarySamples.AveragePages());
}
