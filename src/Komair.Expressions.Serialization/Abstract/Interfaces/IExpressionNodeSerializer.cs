using Komair.Expressions.Abstract;

namespace Komair.Expressions.Serialization.Abstract.Interfaces;

/// <summary>
/// Serializes and deserializes <see cref="ExpressionNodeBase"/> graphs to a document type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The serialized document type (for example JSON).</typeparam>
/// <typeparam name="TExpressionNode">The concrete expression node type used as the root.</typeparam>
public interface IExpressionNodeSerializer<T, TExpressionNode> where TExpressionNode : ExpressionNodeBase
{
    /// <summary>
    /// Deserializes a document to an expression node graph.
    /// </summary>
    /// <param name="document">The serialized document.</param>
    /// <returns>The root expression node.</returns>
    TExpressionNode Deserialize(T document);

    /// <summary>
    /// Serializes an expression node graph to a document.
    /// </summary>
    /// <param name="node">The root expression node.</param>
    /// <returns>The serialized document. JSON implementations use the versioned envelope described by <see cref="ExpressionSerializationWireFormat"/>.</returns>
    T Serialize(TExpressionNode node);
}
