using System.Linq.Expressions;
using Komair.Expressions.Exceptions;

namespace Komair.Expressions.Extensions;

public static class ExpressionExtensions
{
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