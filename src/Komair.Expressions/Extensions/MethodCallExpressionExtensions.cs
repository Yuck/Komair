using System.Linq.Expressions;

namespace Komair.Expressions.Extensions;

/// <summary>
/// Extension methods for <see cref="MethodCallExpression"/>.
/// </summary>
public static class MethodCallExpressionExtensions
{
    /// <summary>
    /// Returns the distinct <see cref="ParameterExpression"/> nodes referenced by this method call.
    /// </summary>
    /// <param name="expression">The method call expression.</param>
    /// <returns>Distinct parameters from the instance (if any) and arguments.</returns>
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
