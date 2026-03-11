using System.Linq.Expressions;

namespace Komair.Specifications.Internal.ExpressionTrees;

internal class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameter;

    internal ParameterReplacer(ParameterExpression parameter)
    {
        _parameter = parameter;
    }

    protected override Expression VisitParameter(ParameterExpression parameter) => base.VisitParameter(_parameter);
}
