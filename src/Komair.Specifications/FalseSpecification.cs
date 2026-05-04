using System.Linq.Expressions;
using Komair.Specifications.Abstract;

namespace Komair.Specifications;

/// <summary>
/// A specification that is never satisfied.
/// </summary>
/// <typeparam name="T">The type being specified.</typeparam>
public class FalseSpecification<T> : SpecificationBase<T>
{
    private static readonly Lazy<FalseSpecification<T>> IdentityInstance = new(() => new FalseSpecification<T>());

    /// <summary>
    /// Gets the singleton instance equivalent to the negation of <see cref="TrueSpecification{T}.Identity"/>.
    /// </summary>
    public static FalseSpecification<T> Identity => IdentityInstance.Value;

    private FalseSpecification() : base()
    {
    }

    /// <inheritdoc />
    public override Expression<Func<T, Boolean>> ToExpression()
    {
        return TrueSpecification<T>.Identity.Not().ToExpression();
    }
}
