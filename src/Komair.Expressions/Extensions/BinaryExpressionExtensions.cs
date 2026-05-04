using System.Linq.Expressions;

namespace Komair.Expressions.Extensions;

/// <summary>
/// Extension methods for <see cref="BinaryExpression"/>.
/// </summary>
public static class BinaryExpressionExtensions
{
    /// <summary>
    /// Returns the distinct <see cref="ParameterExpression"/> nodes referenced by this binary expression.
    /// </summary>
    /// <param name="expression">The binary expression.</param>
    /// <returns>Distinct parameters referenced by the left and right operands.</returns>
    public static IReadOnlyCollection<ParameterExpression> GetParameterList(this BinaryExpression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var result = new List<ParameterExpression>();

        result.AddRange(expression.Left.GetParameterList());
        result.AddRange(expression.Right.GetParameterList());

        return [.. result.GroupBy(t => t.Name).Select(t => t.First())];
    }
}
