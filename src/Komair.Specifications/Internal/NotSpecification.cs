using System.Linq.Expressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.Abstract.Interfaces;

namespace Komair.Specifications.Internal;

internal class NotSpecification<T>(ISpecification<T> specification) : SpecificationBase<T>
{
    private readonly ISpecification<T> _specification = specification;

    public override Expression<Func<T, Boolean>> ToExpression() => GetLambda(Expression.Not(_specification.ToExpression().Body));
}
