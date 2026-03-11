using System.Linq.Expressions;

namespace Komair.Expressions.Extensions;

public static class BinaryExpressionExtensions
{
    public static IReadOnlyCollection<ParameterExpression> GetParameterList(this BinaryExpression expression)
    {
        if (expression is null)
            return [];

        var result = new List<ParameterExpression>();

        result.AddRange(expression.Left.GetParameterList());
        result.AddRange(expression.Right.GetParameterList());

        return [.. result.GroupBy(t => t.Name).Select(t => t.First())];
    }
}
