using System.Linq.Expressions;
using Komair.Specifications.Abstract;

namespace Komair.Specifications;

/// <summary>
/// A specification that is always satisfied.
/// </summary>
/// <typeparam name="T">The type being specified.</typeparam>
public class TrueSpecification<T> : SpecificationBase<T>
{
    private static readonly Lazy<TrueSpecification<T>> IdentityInstance = new(() => new TrueSpecification<T>());

    /// <summary>
    /// Gets the singleton instance that always evaluates to <see langword="true"/>.
    /// </summary>
    public static TrueSpecification<T> Identity => IdentityInstance.Value;

    private TrueSpecification() : base()
    {
    }

    /// <inheritdoc />
    public override Expression<Func<T, Boolean>> ToExpression()
    {
        return GetLambda(Expression.Constant(true));
    }
}
