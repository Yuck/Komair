using System.Linq.Expressions;
using Komair.Specifications.Abstract;

namespace Komair.Specifications.Internal;

internal class ExpressionSpecification<T>(Expression<Func<T, Boolean>> expression) : SpecificationBase<T>
{
    private readonly Expression<Func<T, Boolean>> _expression = expression ?? throw new ArgumentNullException(nameof(expression));

    public override Expression<Func<T, Boolean>> ToExpression() => _expression;
}
