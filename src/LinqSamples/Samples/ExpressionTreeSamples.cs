using System.Linq.Expressions;

namespace LinqSamples.Samples;

/// <summary>
/// A lambda assigned to <c>Expression&lt;TDelegate&gt;</c> becomes data - a tree you can inspect,
/// rewrite or compile. This is how IQueryable providers such as EF Core turn C# into SQL.
/// </summary>
[SampleCategory("Expression trees", 23)]
public static class ExpressionTreeSamples
{
    [Sample("Func vs Expression", Description = "The same lambda as compiled code or as an inspectable tree.")]
    public static IEnumerable<string> FuncVsExpression()
    {
        Func<int, bool> isBig = n => n > 5;
        Expression<Func<int, bool>> isBigExpression = n => n > 5;
        return
        [
            $"Func result:     {isBig(7)}",
            $"Expression text: {isBigExpression}",
            $"Body node type:  {isBigExpression.Body.NodeType}",
        ];
    }

    [Sample("Inspect a lambda", Description = "Take a condition apart into its operator and operands.")]
    public static IEnumerable<string> InspectLambda()
    {
        Expression<Func<Product, bool>> filter = p => p.Price > 100m && p.Category == "Furniture";
        var body = (BinaryExpression)filter.Body;
        return [$"Node:  {body.NodeType}", $"Left:  {body.Left}", $"Right: {body.Right}"];
    }

    [Sample("Compile and run", Description = "Compile() turns the tree into a delegate.")]
    public static int CompileAndRun()
    {
        Expression<Func<int, int, int>> add = (a, b) => a + b;
        return add.Compile()(2, 3);
    }

    [Sample("Build an expression by hand", Description = "The factory methods on Expression build the same tree the compiler would.")]
    public static IEnumerable<string> BuildByHand()
    {
        var p = Expression.Parameter(typeof(Product), "p");
        var body = Expression.GreaterThan(Expression.Property(p, nameof(Product.Price)), Expression.Constant(200m));
        var lambda = Expression.Lambda<Func<Product, bool>>(body, p);
        return [$"Tree:   {lambda}", $"Result: {string.Join(", ", Products.Where(lambda.Compile()).Select(x => x.Name))}"];
    }

    [Sample("ExpressionVisitor: collect members", Description = "Walk the tree and record every property it reads.")]
    public static IEnumerable<string> CollectMembers()
    {
        Expression<Func<Order, bool>> filter = o => o.Quantity > 1 && o.Status == "Shipped";
        var collector = new MemberCollector();
        collector.Visit(filter);
        return collector.Names;
    }

    [Sample("ExpressionVisitor: rewrite", Description = "Return a modified copy of the tree - here every && becomes ||.")]
    public static IEnumerable<string> Rewrite()
    {
        Expression<Func<Product, bool>> filter = p => p.Price > 200m && p.Category == "Electronics";
        var rewritten = (Expression<Func<Product, bool>>)new AndAlsoToOrElse().Visit(filter);
        return
        [
            $"AND: {string.Join(", ", Products.Where(filter.Compile()).Select(p => p.Name))}",
            $"OR:  {string.Join(", ", Products.Where(rewritten.Compile()).Select(p => p.Name))}",
        ];
    }

    [Sample("Property name from a lambda", Description = "A strongly typed alternative to magic strings, as used by mapping libraries.")]
    public static IEnumerable<string> PropertyName() =>
        [NameOf<Customer, string>(c => c.City), NameOf<Order, DateOnly>(o => o.Date)];

    [Sample("Captured variables", Description = "A local captured by the lambda shows up as a field access on a closure object.")]
    public static IEnumerable<string> CapturedVariables()
    {
        var min = 3;
        Expression<Func<Order, bool>> filter = o => o.Quantity > min;
        var right = (MemberExpression)((BinaryExpression)filter.Body).Right;
        return [$"Right node: {right.NodeType}", $"Member:     {right.Member.Name}", $"Inner node: {right.Expression!.NodeType}"];
    }

    [Sample("Evaluate a sub-expression", Description = "Compile just one branch to get its current value - what query providers do with captured variables.")]
    public static int EvaluateSubExpression()
    {
        var min = 3;
        Expression<Func<Order, bool>> filter = o => o.Quantity > min;
        var right = ((BinaryExpression)filter.Body).Right;
        return Expression.Lambda<Func<int>>(right).Compile()();
    }

    [Sample("Block and Assign", Description = "Statement trees: variables and several steps - x => { var y = x * 2; return y + 1; }.")]
    public static int BlockAndAssign()
    {
        var x = Expression.Parameter(typeof(int), "x");
        var y = Expression.Variable(typeof(int), "y");
        var block = Expression.Block(
            [y],
            Expression.Assign(y, Expression.Multiply(x, Expression.Constant(2))),
            Expression.Add(y, Expression.Constant(1)));
        return Expression.Lambda<Func<int, int>>(block, x).Compile()(5);
    }

    [Sample("Conditional expression", Description = "Expression.Condition is the tree form of ?:.")]
    public static IEnumerable<string> Conditional()
    {
        var n = Expression.Parameter(typeof(int), "n");
        var isEven = Expression.Equal(Expression.Modulo(n, Expression.Constant(2)), Expression.Constant(0));
        var label = Expression.Lambda<Func<int, string>>(
            Expression.Condition(isEven, Expression.Constant("even"), Expression.Constant("odd")), n).Compile();
        return Enumerable.Range(1, 4).Select(label);
    }

    [Sample("Method calls", Description = "Expression.Call finds the method by name and argument types.")]
    public static IEnumerable<string> MethodCalls()
    {
        var c = Expression.Parameter(typeof(Customer), "c");
        var startsWith = Expression.Call(Expression.Property(c, nameof(Customer.Name)), nameof(string.StartsWith), null, Expression.Constant("A"));
        var lambda = Expression.Lambda<Func<Customer, bool>>(startsWith, c);
        return [$"Tree:   {lambda}", $"Result: {string.Join(", ", Customers.Where(lambda.Compile()).Select(x => x.Name))}"];
    }

    [Sample("Print the tree", Description = "A visitor that prints one node per line, indented by depth.")]
    public static IEnumerable<string> PrintTree()
    {
        Expression<Func<Product, bool>> filter = p => p.Price * 2 > 100;
        var printer = new TreePrinter();
        printer.Visit(filter.Body);
        return printer.Lines;
    }

    [Sample("The expression behind IQueryable", Description = "Each Queryable operator adds a method-call node; the lambda is stored quoted, not compiled.")]
    public static IEnumerable<string> QueryableExpression()
    {
        var query = Products.AsQueryable().Where(p => p.Price > 200m);
        var call = (MethodCallExpression)query.Expression;
        return [$"Root node: {call.NodeType}", $"Method:    {call.Method.Name}", $"Predicate: {call.Arguments[1]}"];
    }

    [Sample("Compile() creates a new delegate", Description = "Compiling is expensive - cache the delegate instead of compiling per call.")]
    public static IEnumerable<string> CompileCreatesNewDelegate()
    {
        Expression<Func<int, int>> square = n => n * n;
        var first = square.Compile();
        var second = square.Compile();
        return [$"Same delegate: {ReferenceEquals(first, second)}", $"Same result:   {first(4) == second(4)}"];
    }

    private static string NameOf<T, TProperty>(Expression<Func<T, TProperty>> selector) =>
        ((MemberExpression)selector.Body).Member.Name;

    private sealed class MemberCollector : ExpressionVisitor
    {
        public List<string> Names { get; } = [];

        protected override Expression VisitMember(MemberExpression node)
        {
            Names.Add(node.Member.Name);
            return base.VisitMember(node);
        }
    }

    private sealed class AndAlsoToOrElse : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node) =>
            node.NodeType == ExpressionType.AndAlso
                ? Expression.OrElse(Visit(node.Left), Visit(node.Right))
                : base.VisitBinary(node);
    }

    private sealed class TreePrinter : ExpressionVisitor
    {
        private int _depth;

        public List<string> Lines { get; } = [];

        public override Expression? Visit(Expression? node)
        {
            if (node is null)
            {
                return null;
            }

            Lines.Add(new string(' ', _depth * 2) + Describe(node));
            _depth++;
            var result = base.Visit(node);
            _depth--;
            return result;
        }

        private static string Describe(Expression node) => node switch
        {
            MemberExpression m => $"{m.NodeType} ({m.Member.Name})",
            ConstantExpression c => $"{c.NodeType} ({c.Value})",
            ParameterExpression p => $"{p.NodeType} ({p.Name})",
            _ => node.NodeType.ToString(),
        };
    }
}
