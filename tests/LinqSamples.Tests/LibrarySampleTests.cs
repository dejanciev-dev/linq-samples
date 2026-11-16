namespace LinqSamples.Tests;

public class LibrarySampleTests
{
    [Fact] public void Total_days() => Assert.Equal(87, LibrarySamples.TotalDays());
    [Fact] public void Loans_per_member() => Assert.Equal(["Ann:2", "Bob:2", "Cid:1", "Dee:1"], LibrarySamples.LoansPerMember());
    [Fact] public void Titles() => Assert.Equal(["Dune", "Emma", "Neuromancer", "Ulysses"], LibrarySamples.Titles());
    [Fact] public void Days_per_genre() => Assert.Equal(["Classic:47", "SciFi:40"], LibrarySamples.DaysPerGenre());
    [Fact] public void Longest_loan() => Assert.Equal("Dee", LibrarySamples.LongestLoan());
    [Fact] public void Popular_title() => Assert.Equal("Dune", LibrarySamples.PopularTitle());
    [Fact] public void SciFi_readers() => Assert.Equal(["Ann", "Bob", "Cid"], LibrarySamples.SciFiReaders());
    [Fact] public void Long_loans() => Assert.Equal(2, LibrarySamples.LongLoans());
    [Fact] public void Average_days() => Assert.Equal(14.5, LibrarySamples.AverageDays());
    [Fact] public void Genre_explorers() => Assert.Equal(["Ann", "Bob"], LibrarySamples.GenreExplorers());
}
