using System.Linq.Expressions;

namespace Komair.Expressions.Extensions;

public static class MethodCallExpressionExtensions
{
    public static IReadOnlyCollection<ParameterExpression> GetParameterList(this MethodCallExpression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var result = new List<ParameterExpression>();

        if (expression.Object is not null)
            result.AddRange(expression.Object.GetParameterList());

        foreach (var argument in expression.Arguments)
            result.AddRange(argument.GetParameterList());

        return [.. result.GroupBy(t => t.Name).Select(t => t.First())];
    }
}
