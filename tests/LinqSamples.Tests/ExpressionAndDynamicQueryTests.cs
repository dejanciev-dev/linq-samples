using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LinqSamples.Samples.EfCore;
using EfProduct = LinqSamples.Samples.EfCore.Product;

namespace LinqSamples.Tests;

/// <summary>Pins the results of the expression-tree and dynamic-query samples.</summary>
public class ExpressionAndDynamicQueryTests
{
    // Expression trees

    [Fact]
    public void Func_vs_expression() =>
        Assert.Equal(
            ["Func result:     True", "Expression text: n => (n > 5)", "Body node type:  GreaterThan"],
            ExpressionTreeSamples.FuncVsExpression());

    [Fact]
    public void Inspect_lambda() =>
        Assert.Equal(
            ["Node:  AndAlso", "Left:  (p.Price > 100)", "Right: (p.Category == \"Furniture\")"],
            ExpressionTreeSamples.InspectLambda());

    [Fact]
    public void Compile_and_run() =>
        Assert.Equal(5, ExpressionTreeSamples.CompileAndRun());

    [Fact]
    public void Build_by_hand() =>
        Assert.Equal(["Tree:   p => (p.Price > 200)", "Result: Laptop, Desk, Monitor"], ExpressionTreeSamples.BuildByHand());

    [Fact]
    public void Collect_members() =>
        Assert.Equal(["Quantity", "Status"], ExpressionTreeSamples.CollectMembers());

    [Fact]
    public void Rewrite_and_to_or() =>
        Assert.Equal(["AND: Laptop, Monitor", "OR:  Laptop, Mouse, Desk, Monitor"], ExpressionTreeSamples.Rewrite());

    [Fact]
    public void Property_name_from_lambda() =>
        Assert.Equal(["City", "Date"], ExpressionTreeSamples.PropertyName());

    [Fact]
    public void Captured_variables_are_closure_fields() =>
        Assert.Equal(
            ["Right node: MemberAccess", "Member:     min", "Inner node: Constant"],
            ExpressionTreeSamples.CapturedVariables());

    [Fact]
    public void Evaluate_sub_expression() =>
        Assert.Equal(3, ExpressionTreeSamples.EvaluateSubExpression());

    [Fact]
    public void Block_and_assign() =>
        Assert.Equal(11, ExpressionTreeSamples.BlockAndAssign());

    [Fact]
    public void Conditional_expression() =>
        Assert.Equal(["odd", "even", "odd", "even"], ExpressionTreeSamples.Conditional());

    [Fact]
    public void Method_calls() =>
        Assert.Equal(["Tree:   c => c.Name.StartsWith(\"A\")", "Result: Alice"], ExpressionTreeSamples.MethodCalls());

    [Fact]
    public void Print_tree() =>
        Assert.Equal(
            ["GreaterThan", "  Multiply", "    MemberAccess (Price)", "      Parameter (p)", "    Constant (2)", "  Constant (100)"],
            ExpressionTreeSamples.PrintTree());

    [Fact]
    public void Queryable_expression() =>
        Assert.Equal(
            ["Root node: Call", "Method:    Where", "Predicate: p => (p.Price > 200)"],
            ExpressionTreeSamples.QueryableExpression());

    [Fact]
    public void Compile_creates_new_delegate() =>
        Assert.Equal(["Same delegate: False", "Same result:   True"], ExpressionTreeSamples.CompileCreatesNewDelegate());

    // Dynamic queries

    [Fact]
    public void Optional_filters() =>
        Assert.Equal(["Mouse", "Monitor"], DynamicQuerySamples.OptionalFilters());

    [Fact]
    public void Sort_by_property_name() =>
        Assert.Equal(["Chair", "Desk", "Laptop", "Monitor", "Mouse", "Notebook", "Pen"], DynamicQuerySamples.SortByPropertyName());

    [Fact]
    public void Multi_column_sort_matches_static_query() =>
        Assert.Equal(
            Products.OrderBy(p => p.Category).ThenByDescending(p => p.Price).Select(p => $"{p.Category} / {p.Name}"),
            DynamicQuerySamples.MultiColumnSort());

    [Fact]
    public void Filter_by_property_name() =>
        Assert.Equal(["Bob", "Eva"], DynamicQuerySamples.FilterByPropertyName());

    [Fact]
    public void Operator_from_string() =>
        Assert.Equal(["Laptop", "Desk", "Chair", "Monitor"], DynamicQuerySamples.OperatorFromString());

    [Fact]
    public void Search_all_text_columns() =>
        Assert.Equal(["Bob", "Eva"], DynamicQuerySamples.SearchAllTextColumns());

    [Fact]
    public void Predicate_and() =>
        Assert.Equal(["Desk", "Chair"], DynamicQuerySamples.PredicateAnd());

    [Fact]
    public void Predicate_or() =>
        Assert.Equal(["Laptop", "Notebook", "Pen"], DynamicQuerySamples.PredicateOr());

    [Fact]
    public void Predicate_not() =>
        Assert.Equal(["Desk", "Chair", "Notebook", "Pen"], DynamicQuerySamples.PredicateNot());

    [Fact]
    public void Or_together_a_list() =>
        Assert.Equal([2, 3, 9, 11], DynamicQuerySamples.OrTogetherAList());

    [Fact]
    public void Specification_pattern() =>
        Assert.Equal(["Matches:          Laptop, Monitor", "Mouse satisfies?  False"], DynamicQuerySamples.SpecificationPattern());

    [Fact]
    public void Dynamic_projection() =>
        Assert.Equal(["Alice, Skopje", "Bob, Berlin"], DynamicQuerySamples.DynamicProjection());

    [Fact]
    public void Dynamic_group_by() =>
        Assert.Equal(["Skopje: 2", "Berlin: 2", "Vienna: 1"], DynamicQuerySamples.DynamicGroupBy());

    [Fact]
    public void Filters_from_query_string() =>
        Assert.Equal(["Mouse", "Monitor"], DynamicQuerySamples.FiltersFromQueryString());

    [Fact]
    public void Inspect_composed_query() =>
        Assert.Equal("Where -> Where -> OrderBy", DynamicQuerySamples.InspectComposedQuery());

    [Fact]
    public void PredicateBuilder_output_translates_to_sql()
    {
        using var db = ShopDbContext.CreateSeeded();
        Expression<Func<EfProduct, bool>> stationery = p => p.Category == "Stationery";
        var query = db.Products.Where(stationery.Or(p => p.Price > 1000).Not().Not()).OrderBy(p => p.Id);

        Assert.Contains(" OR ", query.ToQueryString(), StringComparison.Ordinal);
        Assert.Equal(["Laptop", "Notebook", "Pen"], query.Select(p => p.Name).ToList());
    }

    private static IReadOnlyList<LinqSamples.Data.Product> Products => LinqSamples.Data.SampleData.Products;
}
