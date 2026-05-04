using System.Linq.Expressions;
using Komair.Specifications.Abstract.Interfaces;
using Komair.Specifications.Internal;
using Komair.Specifications.Internal.ExpressionTrees;

namespace Komair.Specifications.Abstract;

/// <summary>
/// Base class for specifications that compile <see cref="ISpecification{T}.ToExpression"/> to a delegate for <see cref="ISpecification{T}.IsSatisfiedBy"/>.
/// </summary>
/// <typeparam name="T">The type being specified.</typeparam>
public abstract class SpecificationBase<T> : ISpecification<T>
{
    private readonly Lazy<Func<T, Boolean>> _predicate;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationBase{T}"/> class.
    /// </summary>
    protected SpecificationBase()
    {
        _predicate = new Lazy<Func<T, Boolean>>(() => GetLambda(ToExpression()).Compile());
    }

    /// <inheritdoc />
    public ISpecification<T> And(params ISpecification<T>[] specifications)
    {
        return Fold(specifications, this, static (accumulated, next) => new AndSpecification<T>(accumulated, next));
    }

    /// <inheritdoc />
    public Boolean IsSatisfiedBy(T t)
    {
        return _predicate.Value(t);
    }

    /// <inheritdoc />
    public ISpecification<T> Not()
    {
        return new NotSpecification<T>(this);
    }

    /// <inheritdoc />
    public ISpecification<T> Or(params ISpecification<T>[] specifications)
    {
        return Fold(specifications, this, static (accumulated, next) => new OrSpecification<T>(accumulated, next));
    }

    /// <inheritdoc />
    public abstract Expression<Func<T, Boolean>> ToExpression();

    /// <inheritdoc />
    public Expression<Func<T, Boolean>> Where(Expression<Func<T, Boolean>> predicate)
    {
        return new AndSpecification<T>(this, new ExpressionSpecification<T>(predicate)).ToExpression();
    }

    /// <summary>
    /// Rewrites <paramref name="expression"/> into a lambda of <see cref="Func{T, Boolean}"/> over <typeparamref name="T"/>.
    /// </summary>
    /// <param name="expression">The expression body to wrap.</param>
    /// <returns>A lambda with a single parameter of type <typeparamref name="T"/>.</returns>
    protected static Expression<Func<T, Boolean>> GetLambda(Expression expression)
    {
        var parameters = Expression.Parameter(typeof(T));
        var body = new ParameterReplacer(parameters).Visit(expression);
        var simplified = body is Expression<Func<T, Boolean>> lambda ? lambda : Expression.Lambda<Func<T, Boolean>>(body, parameters);

        return simplified;
    }

    private static ISpecification<T> Fold(IEnumerable<ISpecification<T>> specifications, ISpecification<T> identity, Func<ISpecification<T>, ISpecification<T>, ISpecification<T>> combine)
    {
        return specifications.Aggregate(identity, combine);
    }
}
