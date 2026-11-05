namespace LinqSamples.Tests;

public class FinanceSampleTests
{
    [Fact] public void Compound() => Assert.Equal([1050m, 1102.50m, 1157.62m], FinanceSamples.Compound());
    [Fact] public void Loan_balance() => Assert.Equal(727.29m, FinanceSamples.LoanBalance());
    [Fact] public void Net() => Assert.Equal(100m, FinanceSamples.Net());
    [Fact] public void Split() => Assert.Equal("in=150 out=50", FinanceSamples.Split());
    [Fact] public void Lowest_balance() => Assert.Equal(-30m, FinanceSamples.LowestBalance());
    [Fact] public void First_overdraft() => Assert.Equal(1, FinanceSamples.FirstOverdraft());
    [Fact] public void Split_bill() => Assert.Equal([3.34m, 3.33m, 3.33m], FinanceSamples.SplitBill());
    [Fact] public void Shares() => Assert.Equal([25m, 25m, 50m], FinanceSamples.Shares());
    [Fact] public void Portfolio() => Assert.Equal(0.052m, FinanceSamples.Portfolio());
    [Fact] public void Round_to_5() => Assert.Equal([10, 15, 20, 20], FinanceSamples.RoundTo5());
    [Fact] public void Biggest_expense() => Assert.Equal("rent", FinanceSamples.BiggestExpense());
    [Fact] public void By_category() => Assert.Equal("food=15, fuel=40", FinanceSamples.ByCategory());
}

public class FinanceMoreTests
{
    [Fact] public void Average_tx() => Assert.Equal(25m, FinanceSamples.AverageTx());
    [Fact] public void Running() => Assert.Equal([100m, 70m, 50m, 100m], FinanceSamples.Running());
    [Fact] public void Largest_withdrawal() => Assert.Equal(-30m, FinanceSamples.LargestWithdrawal());
    [Fact] public void Percent() => Assert.Equal([25m, 75m], FinanceSamples.Percent());
}
