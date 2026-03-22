using System.Linq.Expressions;
using Komair.Specifications.Abstract;

namespace Komair.Specifications.Internal;

internal class ExpressionSpecification<T>(Expression<Func<T, Boolean>> expression) : SpecificationBase<T>
{
    public override Expression<Func<T, Boolean>> ToExpression()
    {
        return expression;
    }
}
