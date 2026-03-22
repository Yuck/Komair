using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.Abstract.Interfaces;

namespace Komair.Specifications.Internal;

internal class NotSpecification<T> : SpecificationBase<T>
{
    private readonly ISpecification<T> _specification;

    internal NotSpecification(ISpecification<T> specification) : base()
    {
        _specification = specification;
    }

    public override Expression<Func<T, Boolean>> ToExpression()
    {
        return GetLambda(Expression.Not(_specification.ToExpression().Body));
    }
}
