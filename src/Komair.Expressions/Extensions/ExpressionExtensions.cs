using System.Linq.Expressions;
using Komair.Expressions.Exceptions;

namespace Komair.Expressions.Extensions;

/// <summary>
/// Extension methods for <see cref="Expression"/>.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Returns the distinct <see cref="ParameterExpression"/> nodes referenced by this expression tree.
    /// </summary>
    /// <param name="expression">The expression tree.</param>
    /// <returns>Distinct parameters referenced by the expression.</returns>
    /// <exception cref="UnsupportedExpressionException">The expression kind is not supported.</exception>
    public static IReadOnlyCollection<ParameterExpression> GetParameterList(this Expression expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return expression switch
        {
            BinaryExpression binary => binary.GetParameterList(),
            ConstantExpression => [],
            MemberExpression member => member.GetParameterList(),
            MethodCallExpression call => call.GetParameterList(),
            ParameterExpression parameter => [parameter],
            UnaryExpression unary => unary.Operand.GetParameterList(),
            _ => throw new UnsupportedExpressionException(expression.NodeType)
        };
    }
}