using System.Linq.Expressions;

namespace Komair.Expressions.Exceptions;

/// <summary>
/// Thrown when an expression kind is not supported for the current operation.
/// </summary>
/// <param name="nodeType">The unsupported expression kind.</param>
public sealed class UnsupportedExpressionException(ExpressionType nodeType) : Exception($"Unsupported expression kind: {nodeType}.")
{
    /// <summary>
    /// Gets the unsupported expression kind.
    /// </summary>
    public ExpressionType NodeType { get; } = nodeType;
}
