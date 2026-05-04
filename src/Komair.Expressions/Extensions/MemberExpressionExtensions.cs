using System.Linq.Expressions;

namespace Komair.Expressions.Extensions;

/// <summary>
/// Extension methods for <see cref="MemberExpression"/>.
/// </summary>
public static class MemberExpressionExtensions
{
    /// <summary>
    /// Returns the distinct <see cref="ParameterExpression"/> nodes referenced by this member expression.
    /// </summary>
    /// <param name="expression">The member expression.</param>
    /// <returns>Parameters from the inner expression, or an empty collection when there is no inner expression.</returns>
    public static IReadOnlyCollection<ParameterExpression> GetParameterList(this MemberExpression expression)
    {
        return expression is { Expression: { } inner }
            ? inner.GetParameterList()
            : [];
    }
}
