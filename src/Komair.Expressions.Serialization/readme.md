# Komair.Expressions.Serialization

An abstraction library for serializing `Komair.Expressions.ExpressionNode` objects to and from other representations (including `System.Linq.Expressions`). Concrete serializers (such as JSON and MessagePack) live in separate packages and implement `IExpressionNodeSerializer`.

## Key Types

- **`ExpressionSerializationWireFormat`** &ndash; documents the shared schema version and the JSON envelope (`$schema`, `node`) plus the legacy bare-node layout (schema 0); MessagePack uses the same schema version with a binary `[schema, node]` envelope
- **`ExpressionSerializationException`** (`Komair.Expressions.Serialization.Exceptions`) &ndash; thrown when an `IExpressionNodeSerializer` implementation cannot produce or consume the expected serialized shape (for example a null deserialize result or an unexpected document root)
- **`IExpressionNodeSerializer<T, TExpressionNode>`** (`Komair.Expressions.Serialization.Abstract.Interfaces`) &ndash; serializes and deserializes `ExpressionNodeBase` graphs to a transport document type

Pair this package with `Komair.Expressions.Serialization.Json` or `Komair.Expressions.Serialization.MessagePack` in applications that need to send or persist expression trees.

## Dependencies

- [Komair.Expressions](https://www.nuget.org/packages/Komair.Expressions)

## Installation

```shell
dotnet add package Komair.Expressions.Serialization
```
