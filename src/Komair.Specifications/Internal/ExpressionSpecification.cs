using System.Linq.Expressions;
using Komair.Specifications.Abstract;

namespace Komair.Specifications.Internal;

internal class ExpressionSpecification<T> : SpecificationBase<T>
{
    private readonly Expression<Func<T, Boolean>> _expression;

    internal ExpressionSpecification(Expression<Func<T, Boolean>> expression) : base()
    {
        _expression = expression;
    }

    public override Expression<Func<T, Boolean>> ToExpression()
    {
        return _expression;
    }
}
