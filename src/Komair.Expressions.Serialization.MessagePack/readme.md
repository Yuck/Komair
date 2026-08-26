# Komair.Expressions.Serialization.MessagePack

A MessagePack-based implementation of Komair expression serialization for compact binary payloads (RPC, caching, and storage).

## Key Types

- **`ExpressionNodeSerializer<TNode>`** &ndash; serializes and deserializes expression node graphs to and from `byte[]` using MessagePack polymorphic discriminators

## Wire format

Current documents use **schema 1** (see `ExpressionSerializationWireFormat.CurrentSchemaVersion` in `Komair.Expressions.Serialization`):

```
[schemaVersion: int, node]
```

The `node` element is a 2-element array `[discriminator: string, payload: map]`. Discriminators match the JSON serializer (`Binary`, `Block`, `Conditional`, `Constant`, `Invocation`, `Lambda`, `ListInit`, `Member`, `MemberInit`, `New`, `Parameter`, `Quote`).

CLR `Type` values are stored as assembly-qualified name strings. Constant values are encoded using the constant node's CLR type.

### Migration notes

| Schema | Layout | Read | Write |
|--------|--------|------|-------|
| 1 | `[schema, node]` | Supported | Current default |

When node shapes or discriminators change incompatibly, bump `ExpressionSerializationWireFormat.CurrentSchemaVersion`, add a deserialize branch for older schemas, and extend this table.

## Dependencies

- [Komair.Expressions](https://www.nuget.org/packages/Komair.Expressions)
- [Komair.Expressions.Serialization](https://www.nuget.org/packages/Komair.Expressions.Serialization)
- [MessagePack](https://www.nuget.org/packages/MessagePack)

## Installation

```shell
dotnet add package Komair.Expressions.Serialization.MessagePack
```
