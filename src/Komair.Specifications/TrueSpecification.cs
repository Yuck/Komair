using System.Linq.Expressions;
using Komair.Specifications.Abstract;

namespace Komair.Specifications;

public class TrueSpecification<T> : SpecificationBase<T>
{
    private static readonly Lazy<TrueSpecification<T>> IdentityInstance = new(() => new TrueSpecification<T>());

    public static TrueSpecification<T> Identity => IdentityInstance.Value;

    private TrueSpecification() { }

    public override Expression<Func<T, Boolean>> ToExpression()
    {
        return GetLambda(Expression.Constant(true));
    }
}
