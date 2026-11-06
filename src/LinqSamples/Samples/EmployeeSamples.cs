namespace LinqSamples.Samples;

/// <summary>Payroll and department statistics.</summary>
[SampleCategory("Employees", 51)]
public static class EmployeeSamples
{
    private sealed record Emp(string Name, string Dept, int Salary, int Years);

    private static readonly Emp[] Staff =
    [
        new("Ana", "eng", 5000, 5), new("Ben", "eng", 4000, 2), new("Cat", "ops", 3000, 7),
        new("Dan", "ops", 3500, 3), new("Eve", "hr", 3200, 4), new("Fay", "eng", 6000, 9),
    ];

    [Sample("Payroll", Description = "Sum of all salaries.")]
    public static int Payroll() => Staff.Sum(e => e.Salary);

    [Sample("Average salary per department", Description = "GroupBy department and average.")]
    public static IEnumerable<string> AveragePerDept() =>
        Staff.GroupBy(e => e.Dept).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Average(e => e.Salary)}");

    [Sample("Highest paid", Description = "MaxBy salary.")]
    public static string HighestPaid() => Staff.MaxBy(e => e.Salary)!.Name;

    [Sample("Veterans", Description = "Five or more years, sorted by name.")]
    public static IEnumerable<string> Veterans() => Staff.Where(e => e.Years >= 5).Select(e => e.Name).Order();

    [Sample("Payroll after veteran raise", Description = "Add 10% to veterans inside the Sum.")]
    public static int PayrollAfterRaise() => Staff.Sum(e => e.Years >= 5 ? e.Salary + e.Salary / 10 : e.Salary);

    [Sample("Headcount per department", Description = "CountBy department.")]
    public static IEnumerable<string> Headcount() => Staff.CountBy(e => e.Dept).OrderBy(c => c.Key).Select(c => $"{c.Key}:{c.Value}");

    [Sample("Top earner per department", Description = "GroupBy department then MaxBy salary.")]
    public static IEnumerable<string> TopPerDept() =>
        Staff.GroupBy(e => e.Dept).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.MaxBy(e => e.Salary)!.Name}");

    [Sample("Departments above 3240 average", Description = "Filter groups on their average.")]
    public static IEnumerable<string> RichDepartments() =>
        Staff.GroupBy(e => e.Dept).Where(g => g.Average(e => e.Salary) > 3240).Select(g => g.Key).Order();

    [Sample("Salary ranking", Description = "OrderByDescending then Select with an index.")]
    public static IEnumerable<string> Ranking() =>
        Staff.OrderByDescending(e => e.Salary).Select((e, i) => $"{i + 1}.{e.Name}");

    [Sample("Pay gap per department", Description = "Max minus min salary in each group.")]
    public static IEnumerable<string> PayGap() =>
        Staff.GroupBy(e => e.Dept).OrderBy(g => g.Key).Select(g => $"{g.Key}:{g.Max(e => e.Salary) - g.Min(e => e.Salary)}");
}
