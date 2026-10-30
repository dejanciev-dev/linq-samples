using System.Globalization;
using System.Linq.Expressions;

namespace LinqSamples.Samples;

/// <summary>
/// Building queries at runtime - from user input, query strings or configuration - while keeping
/// them as expression trees, so the same code works for in-memory data and for EF Core.
/// </summary>
[SampleCategory("Dynamic queries", 24)]
public static class DynamicQuerySamples
{
    [Sample("Optional filters", Description = "Compose IQueryable step by step; only the filters that are set are added.")]
    public static IEnumerable<string> OptionalFilters()
    {
        string? category = "Electronics";
        decimal? maxPrice = 300m;
        string? nameContains = null;

        var query = Products.AsQueryable();
        if (category is not null)
        {
            query = query.Where(p => p.Category == category);
        }

        if (maxPrice is not null)
        {
            query = query.Where(p => p.Price <= maxPrice);
        }

        if (nameContains is not null)
        {
            query = query.Where(p => p.Name.Contains(nameContains));
        }

        return query.Select(p => p.Name);
    }

    [Sample("Sort by property name", Description = "Build the key selector from a string and call Queryable.OrderBy through the tree.")]
    public static IEnumerable<string> SortByPropertyName() =>
        OrderByProperty(Products.AsQueryable(), "Name", descending: false).Select(p => p.Name);

    [Sample("Multi-column dynamic sort", Description = "Parse a sort spec like \"Category, Price desc\" into OrderBy/ThenBy calls.")]
    public static IEnumerable<string> MultiColumnSort()
    {
        const string spec = "Category, Price desc";
        var query = Products.AsQueryable();
        var first = true;
        foreach (var part in spec.Split(',', StringSplitOptions.TrimEntries))
        {
            var tokens = part.Split(' ');
            var descending = tokens is [_, "desc"];
            query = OrderByProperty(query, tokens[0], descending, thenBy: !first);
            first = false;
        }

        return query.Select(p => $"{p.Category} / {p.Name}");
    }

    [Sample("Filter by property name", Description = "Equality on a property chosen at runtime.")]
    public static IEnumerable<string> FilterByPropertyName() =>
        Customers.AsQueryable().Where(Condition<Customer>("City", "==", "Berlin")).Select(c => c.Name);

    [Sample("Operator from a string", Description = "Map \">\", \"<=\" and friends to Expression factory methods.")]
    public static IEnumerable<string> OperatorFromString() =>
        Products.AsQueryable().Where(Condition<Product>("Price", ">", 100)).Select(p => p.Name);

    [Sample("Search all text columns", Description = "OR a Contains call over every string property.")]
    public static IEnumerable<string> SearchAllTextColumns()
    {
        const string term = "er";
        var c = Expression.Parameter(typeof(Customer), "c");
        var contains = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!;
        var body = typeof(Customer).GetProperties()
                                   .Where(property => property.PropertyType == typeof(string))
                                   .Select(property => (Expression)Expression.Call(Expression.Property(c, property), contains, Expression.Constant(term)))
                                   .Aggregate(Expression.OrElse);
        return Customers.AsQueryable().Where(Expression.Lambda<Func<Customer, bool>>(body, c)).Select(x => x.Name);
    }

    [Sample("PredicateBuilder: And", Description = "Combine two lambdas into one by sharing a single parameter.")]
    public static IEnumerable<string> PredicateAnd()
    {
        Expression<Func<Product, bool>> pricey = p => p.Price > 100m;
        return Products.AsQueryable().Where(pricey.And(p => p.Category == "Furniture")).Select(p => p.Name);
    }

    [Sample("PredicateBuilder: Or", Description = "Either condition may match.")]
    public static IEnumerable<string> PredicateOr()
    {
        Expression<Func<Product, bool>> stationery = p => p.Category == "Stationery";
        return Products.AsQueryable().Where(stationery.Or(p => p.Price > 1000m)).Select(p => p.Name);
    }

    [Sample("PredicateBuilder: Not", Description = "Negate an existing predicate.")]
    public static IEnumerable<string> PredicateNot()
    {
        Expression<Func<Product, bool>> electronics = p => p.Category == "Electronics";
        return Products.AsQueryable().Where(electronics.Not()).Select(p => p.Name);
    }

    [Sample("OR together a list", Description = "Start from 'never' and Or in one condition per value.")]
    public static IEnumerable<int> OrTogetherAList()
    {
        string[] statuses = ["Pending", "Shipped"];
        var predicate = statuses.Aggregate(PredicateBuilder.Never<Order>(), (acc, status) => acc.Or(o => o.Status == status));
        return Orders.AsQueryable().Where(predicate).Select(o => o.Id);
    }

    [Sample("Specification pattern", Description = "Named, reusable, combinable business rules backed by expressions.")]
    public static IEnumerable<string> SpecificationPattern()
    {
        var expensive = new Specification<Product>(p => p.Price > 200m);
        var electronics = new Specification<Product>(p => p.Category == "Electronics");
        var rule = expensive.And(electronics);
        return
        [
            $"Matches:          {string.Join(", ", Products.AsQueryable().Where(rule.Criteria).Select(p => p.Name))}",
            $"Mouse satisfies?  {rule.IsSatisfiedBy(Products.First(p => p.Name == "Mouse"))}",
        ];
    }

    [Sample("Dynamic projection", Description = "Select only the columns named at runtime.")]
    public static IEnumerable<string> DynamicProjection()
    {
        string[] columns = ["Name", "City"];
        var c = Expression.Parameter(typeof(Customer), "c");
        var values = Expression.NewArrayInit(typeof(object),
            columns.Select(column => Expression.Convert(Expression.Property(c, column), typeof(object))));
        var selector = Expression.Lambda<Func<Customer, object[]>>(values, c).Compile();
        return Customers.Take(2).Select(selector).Select(row => string.Join(", ", row));
    }

    [Sample("Dynamic GroupBy key", Description = "Group by a property chosen at runtime.")]
    public static IEnumerable<string> DynamicGroupBy()
    {
        const string groupBy = "City";
        var c = Expression.Parameter(typeof(Customer), "c");
        var key = Expression.Lambda<Func<Customer, object>>(Expression.Convert(Expression.Property(c, groupBy), typeof(object)), c);
        return Customers.AsQueryable().GroupBy(key).Select(g => $"{g.Key}: {g.Count()}");
    }

    [Sample("Filters from a query string", Description = "Parse key=value pairs and And together the matching predicates.")]
    public static IEnumerable<string> FiltersFromQueryString()
    {
        const string queryString = "category=Electronics&maxPrice=300";
        var parameters = queryString.Split('&')
                                    .Select(pair => pair.Split('='))
                                    .ToDictionary(kv => kv[0], kv => kv[1]);

        var predicate = PredicateBuilder.Always<Product>();
        if (parameters.TryGetValue("category", out var category))
        {
            predicate = predicate.And(p => p.Category == category);
        }

        if (parameters.TryGetValue("maxPrice", out var max))
        {
            var maxPrice = decimal.Parse(max, CultureInfo.InvariantCulture);
            predicate = predicate.And(p => p.Price <= maxPrice);
        }

        return Products.AsQueryable().Where(predicate).Select(p => p.Name);
    }

    [Sample("Inspect a composed query", Description = "Walk the method-call chain of an IQueryable to see what was added.")]
    public static string InspectComposedQuery()
    {
        var query = Products.AsQueryable()
                            .Where(p => p.Price > 10m)
                            .Where(p => p.Category != "Furniture")
                            .OrderBy(p => p.Name);

        List<string> operators = [];
        var node = query.Expression;
        while (node is MethodCallExpression call)
        {
            operators.Add(call.Method.Name);
            node = call.Arguments[0];
        }

        operators.Reverse();
        return string.Join(" -> ", operators);
    }

    private static IOrderedQueryable<T> OrderByProperty<T>(IQueryable<T> source, string property, bool descending, bool thenBy = false)
    {
        var x = Expression.Parameter(typeof(T), "x");
        var member = Expression.Property(x, property);
        var method = (thenBy, descending) switch
        {
            (false, false) => nameof(Queryable.OrderBy),
            (false, true) => nameof(Queryable.OrderByDescending),
            (true, false) => nameof(Queryable.ThenBy),
            (true, true) => nameof(Queryable.ThenByDescending),
        };
        var call = Expression.Call(typeof(Queryable), method, [typeof(T), member.Type],
                                   source.Expression, Expression.Quote(Expression.Lambda(member, x)));
        return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
    }

    private static Expression<Func<T, bool>> Condition<T>(string property, string op, object value)
    {
        var x = Expression.Parameter(typeof(T), "x");
        var member = Expression.Property(x, property);
        var constant = Expression.Constant(Convert.ChangeType(value, member.Type, CultureInfo.InvariantCulture), member.Type);
        Expression body = op switch
        {
            "==" => Expression.Equal(member, constant),
            "!=" => Expression.NotEqual(member, constant),
            ">" => Expression.GreaterThan(member, constant),
            ">=" => Expression.GreaterThanOrEqual(member, constant),
            "<" => Expression.LessThan(member, constant),
            "<=" => Expression.LessThanOrEqual(member, constant),
            _ => throw new ArgumentException($"Unknown operator '{op}'.", nameof(op)),
        };
        return Expression.Lambda<Func<T, bool>>(body, x);
    }
}

/// <summary>Combines predicate expressions so they stay translatable by IQueryable providers.</summary>
public static class PredicateBuilder
{
    /// <summary>A predicate that matches everything - the seed for And chains.</summary>
    public static Expression<Func<T, bool>> Always<T>() => _ => true;

    /// <summary>A predicate that matches nothing - the seed for Or chains.</summary>
    public static Expression<Func<T, bool>> Never<T>() => _ => false;

    extension<T>(Expression<Func<T, bool>> left)
    {
        public Expression<Func<T, bool>> And(Expression<Func<T, bool>> right) => Combine(left, right, Expression.AndAlso);

        public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> right) => Combine(left, right, Expression.OrElse);

        public Expression<Func<T, bool>> Not() => Expression.Lambda<Func<T, bool>>(Expression.Not(left.Body), left.Parameters);
    }

    private static Expression<Func<T, bool>> Combine<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        Func<Expression, Expression, BinaryExpression> merge)
    {
        // Point the right lambda's parameter at the left one so both bodies share a single parameter.
        var rightBody = new ReplaceParameter(right.Parameters[0], left.Parameters[0]).Visit(right.Body);
        return Expression.Lambda<Func<T, bool>>(merge(left.Body, rightBody), left.Parameters);
    }

    private sealed class ReplaceParameter(ParameterExpression from, Expression to) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node) =>
            node == from ? to : base.VisitParameter(node);
    }
}

/// <summary>A named business rule that can be combined with others and used in queries.</summary>
public sealed class Specification<T>(Expression<Func<T, bool>> criteria)
{
    private readonly Lazy<Func<T, bool>> _compiled = new(criteria.Compile);

    public Expression<Func<T, bool>> Criteria { get; } = criteria;

    public Specification<T> And(Specification<T> other) => new(Criteria.And(other.Criteria));

    public bool IsSatisfiedBy(T item) => _compiled.Value(item);
}
