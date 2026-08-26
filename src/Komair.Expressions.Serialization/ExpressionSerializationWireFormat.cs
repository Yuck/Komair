namespace Komair.Expressions.Serialization;

/// <summary>
/// Identifies the versioned expression serialization envelope schema shared by concrete serializers.
/// </summary>
/// <remarks>
/// <para><see cref="CurrentSchemaVersion"/> is shared across formats. JSON documents use <see cref="SchemaPropertyName"/> and <see cref="NodePropertyName"/>; MessagePack documents use a 2-element array of <c>[schemaVersion, node]</c>.</para>
/// <para><b>Schema 0 (JSON legacy):</b> the document root is the expression node graph (for example a root <c>$type</c> of <c>Lambda</c>).</para>
/// <para><b>Schema 1 (current):</b> JSON roots are objects with <see cref="NodePropertyName"/> holding the node graph and <see cref="SchemaPropertyName"/> set to <see cref="CurrentSchemaVersion"/>. MessagePack roots are <c>[1, node]</c>.</para>
/// <para>When node shapes change in a breaking way, increment <see cref="CurrentSchemaVersion"/>, teach each concrete serializer to read older schemas, and document migration steps in that package's readme.</para>
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
