using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a lambda <see cref="System.Linq.Expressions.Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class LambdaExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the lambda body.
    /// </summary>
    public required ExpressionNodeBase Body { get; set; }

    /// <summary>
    /// Gets or sets the lambda parameters.
    /// </summary>
    public required IReadOnlyCollection<ParameterExpressionNode> Parameters { get; set; }
}
