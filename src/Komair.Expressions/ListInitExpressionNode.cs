using System.Linq.Expressions;
using Komair.Expressions.Abstract;

namespace Komair.Expressions;

/// <summary>
/// Represents a list-initializer <see cref="Expression"/> in serializable form.
/// </summary>
/// <param name="nodeType">The expression tree kind for this node.</param>
/// <param name="type">The CLR type of the expression.</param>
public class ListInitExpressionNode(ExpressionType nodeType, Type type) : ExpressionNodeBase(nodeType, type)
{
    /// <summary>
    /// Gets or sets the constructor expression.
    /// </summary>
    public required NewExpressionNode NewExpression { get; set; }

    /// <summary>
    /// Gets or sets collection element initializers.
    /// </summary>
    public required IReadOnlyCollection<ElementInitNode> Initializers { get; set; }
}
