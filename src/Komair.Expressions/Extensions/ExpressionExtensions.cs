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
            BlockExpression block => block.Expressions.SelectMany(t => t.GetParameterList()).Distinct().ToArray(),
            ConditionalExpression conditional => conditional.Test.GetParameterList().Concat(conditional.IfTrue.GetParameterList()).Concat(conditional.IfFalse.GetParameterList()).Distinct().ToArray(),
            ConstantExpression => [],
            InvocationExpression invocation => invocation.Expression.GetParameterList().Concat(invocation.Arguments.SelectMany(t => t.GetParameterList())).Distinct().ToArray(),
            LambdaExpression lambda => lambda.Body.GetParameterList(),
            ListInitExpression listInit => listInit.NewExpression.GetParameterList().Concat(listInit.Initializers.SelectMany(t => t.Arguments).SelectMany(t => t.GetParameterList())).Distinct().ToArray(),
            MemberExpression member => member.GetParameterList(),
            MemberInitExpression memberInit => memberInit.NewExpression.GetParameterList().Concat(memberInit.Bindings.OfType<MemberAssignment>().Select(t => t.Expression).SelectMany(t => t.GetParameterList())).Distinct().ToArray(),
            MethodCallExpression call => call.GetParameterList(),
            NewExpression newExpression => newExpression.Arguments.SelectMany(t => t.GetParameterList()).Distinct().ToArray(),
            ParameterExpression parameter => [parameter],
            UnaryExpression { NodeType: ExpressionType.Quote } unary => unary.Operand.GetParameterList(),
            UnaryExpression unary => unary.Operand.GetParameterList(),
            _ => throw new UnsupportedExpressionException(expression.NodeType)
        };
    }
}