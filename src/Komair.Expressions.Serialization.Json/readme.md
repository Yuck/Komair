# Komair.Expressions.Serialization.Json

A JSON-based implementation of Komair expression serialization using `System.Text.Json`.

## Key Types

- **`ExpressionNodeSerializer<TNode>`** &ndash; serializes and deserializes expression node graphs to and from `JsonObject` using `System.Text.Json` polymorphic metadata

## Wire format

Current documents use **schema 1** (see `ExpressionSerializationWireFormat` in `Komair.Expressions.Serialization`):

```json
{
  "$schema": 1,
  "node": { "$type": "Lambda", "nodeType": 18, "type": "...", "body": { ... }, "parameters": [ ] }
}
```

**Schema 0 (legacy):** the root object is the node graph (no `$schema` / `node` wrapper). `ExpressionNodeSerializer` still deserializes these payloads.

### Migration notes

| Schema | Layout | Read | Write |
|--------|--------|------|-------|
| 0 | Bare node at root | Supported | Not emitted (upgrade by round-tripping through `Serialize`) |
| 1 | `$schema` + `node` | Supported | Current default |

When node shapes or discriminators change incompatibly, bump `ExpressionSerializationWireFormat.CurrentSchemaVersion`, add a deserialize branch for older schemas, and extend this table.

## Dependencies

- [Komair.Expressions](https://www.nuget.org/packages/Komair.Expressions)
- [Komair.Expressions.Serialization](https://www.nuget.org/packages/Komair.Expressions.Serialization)

## Installation

```shell
dotnet add package Komair.Expressions.Serialization.Json
```
