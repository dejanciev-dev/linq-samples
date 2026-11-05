namespace LinqSamples.Samples;

/// <summary>Money, interest and simple finance calculations with LINQ.</summary>
[SampleCategory("Money & finance", 43)]
public static class FinanceSamples
{
    [Sample("Compound interest schedule", Description = "Scan accumulates the balance year by year.")]
    public static IEnumerable<decimal> Compound() =>
        Enumerable.Range(1, 3).Scan(1000m, (bal, _) => Math.Round(bal * 1.05m, 2));

    [Sample("Loan balance after payments", Description = "Aggregate applies interest then subtracts each payment.")]
    public static decimal LoanBalance() =>
        Enumerable.Repeat(100m, 3).Aggregate(1000m, (bal, pay) => Math.Round(bal * 1.01m - pay, 2));

    [Sample("Net total of transactions", Description = "Sum signed amounts.")]
    public static decimal Net() => new[] { 100m, -30m, -20m, 50m }.Sum();

    [Sample("Deposits vs withdrawals", Description = "Partition by sign with GroupBy.")]
    public static string Split()
    {
        decimal[] tx = [100m, -30m, -20m, 50m];
        return $"in={tx.Where(t => t > 0).Sum()} out={-tx.Where(t => t < 0).Sum()}";
    }

    [Sample("Lowest balance reached", Description = "Running total, then Min.")]
    public static decimal LowestBalance() =>
        new[] { 50m, -80m, 20m, -10m }.Scan(0m, (a, b) => a + b).Min();

    [Sample("First overdraft index", Description = "Find the first running balance below zero.")]
    public static int FirstOverdraft() =>
        new[] { 50m, -80m, 20m }.Scan(0m, (a, b) => a + b).Select((b, i) => (b, i)).First(x => x.b < 0).i;

    [Sample("Split bill evenly", Description = "Distribute cents so the parts add up exactly.")]
    public static IEnumerable<decimal> SplitBill()
    {
        const int cents = 1000, people = 3;
        return Enumerable.Range(0, people).Select(i => (cents / people + (i < cents % people ? 1 : 0)) / 100m);
    }

    [Sample("Percent of total", Description = "Each amount as a percent of the total.")]
    public static IEnumerable<decimal> Shares()
    {
        decimal[] a = [25m, 25m, 50m];
        return a.Select(x => x / a.Sum() * 100);
    }

    [Sample("Weighted portfolio return", Description = "Sum of weight times return.")]
    public static decimal Portfolio() =>
        new[] { (w: 0.5m, r: 0.10m), (w: 0.3m, r: 0.04m), (w: 0.2m, r: -0.05m) }.Sum(x => x.w * x.r);

    [Sample("Round to nearest 5", Description = "Select with arithmetic rounding.")]
    public static IEnumerable<int> RoundTo5() => new[] { 12, 13, 18, 22 }.Select(x => (int)Math.Round(x / 5.0) * 5);

    [Sample("Largest single expense", Description = "MaxBy on a named tuple list.")]
    public static string? BiggestExpense() =>
        new[] { (name: "rent", amt: 900m), (name: "food", amt: 300m) }.MaxBy(e => e.amt).name;

    [Sample("Total by category", Description = "GroupBy then Sum per group.")]
    public static string ByCategory() =>
        string.Join(", ", new[] { ("food", 10m), ("fuel", 40m), ("food", 5m) }
            .GroupBy(t => t.Item1, t => t.Item2).OrderBy(g => g.Key).Select(g => $"{g.Key}={g.Sum()}"));

    [Sample("Average transaction", Description = "Average of signed amounts.")]
    public static decimal AverageTx() => new[] { 100m, -30m, -20m, 50m }.Average();

    [Sample("Running balance", Description = "Scan yields the balance after each transaction.")]
    public static IEnumerable<decimal> Running() => new[] { 100m, -30m, -20m, 50m }.Scan(0m, (b, t) => b + t);

    [Sample("Largest withdrawal", Description = "Min of the negative amounts.")]
    public static decimal LargestWithdrawal() => new[] { 100m, -30m, -20m, 50m }.Where(t => t < 0).Min();

    [Sample("Share of two amounts", Description = "Each amount divided by their total.")]
    public static IEnumerable<decimal> Percent() { decimal[] a = [25m, 75m]; return a.Select(x => x * 100 / a.Sum()); }

    [Sample("Average monthly spend", Description = "Average over decimal amounts stays exact.")]
    public static decimal AverageSpend() => new[] { 120m, 80m, 100m }.Average();

    [Sample("Round half away from zero", Description = "Math.Round with MidpointRounding avoids banker rounding surprises.")]
    public static IEnumerable<decimal> RoundMoney() =>
        new[] { 0.125m, 0.135m, 2.5m }.Select(x => Math.Round(x, 2, MidpointRounding.AwayFromZero));

    [Sample("Add tax to each line", Description = "Sum with a selector applies a rate while totalling.")]
    public static decimal WithTax() => new[] { 100m, 200m }.Sum(x => x * 1.2m);
}
