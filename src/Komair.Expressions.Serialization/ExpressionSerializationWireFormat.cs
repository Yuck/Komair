namespace Komair.Expressions.Serialization;

/// <summary>
/// Identifies the versioned JSON envelope produced by <c>Komair.Expressions.Serialization.Json</c>.
/// </summary>
/// <remarks>
/// <para><b>Schema 0 (legacy):</b> the document root is the expression node graph (for example a root <c>$type</c> of <c>Lambda</c>).</para>
/// <para><b>Schema 1 (current):</b> the document root is an object with <see cref="NodePropertyName"/> holding the node graph and <see cref="SchemaPropertyName"/> set to <see cref="CurrentSchemaVersion"/>.</para>
/// <para>When node shapes change in a breaking way, increment <see cref="CurrentSchemaVersion"/>, teach <c>ExpressionNodeSerializer</c> to read older schemas, and document migration steps in the JSON package readme.</para>
/// </remarks>
public static class ExpressionSerializationWireFormat
{
    /// <summary>
    /// The wire-format schema version written by current serializers.
    /// </summary>
    public const Int32 CurrentSchemaVersion = 1;

    /// <summary>
    /// JSON property that holds the expression node graph in schema 1 documents.
    /// </summary>
    public const String NodePropertyName = "node";

    /// <summary>
    /// JSON property that holds the wire-format schema version in schema 1 documents.
    /// </summary>
    public const String SchemaPropertyName = "$schema";
}
