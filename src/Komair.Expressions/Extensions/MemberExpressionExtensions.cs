using System.Linq.Expressions;

namespace Komair.Expressions.Extensions;

public static class MemberExpressionExtensions
{
    public static IReadOnlyCollection<ParameterExpression> GetParameterList(this MemberExpression expression)
    {
        return expression is { Expression: { } inner }
            ? inner.GetParameterList()
            : [];
    }
}
