# Komair.Expressions.Serialization

An abstraction library for serializing `Komair.Expressions.ExpressionNode` objects to and from other representations (including `System.Linq.Expressions`). Concrete serializers (such as JSON) live in separate packages and implement `IExpressionNodeSerializer`.

## Key Types

- **`ExpressionSerializationWireFormat`** &ndash; documents the versioned JSON envelope (`$schema`, `node`) and the legacy bare-node layout (schema 0)
- **`ExpressionSerializationException`** (`Komair.Expressions.Serialization.Exceptions`) &ndash; thrown when an `IExpressionNodeSerializer` implementation cannot produce or consume the expected serialized shape (for example a null deserialize result or an unexpected JSON root)
- **`IExpressionNodeSerializer<T, TExpressionNode>`** (`Komair.Expressions.Serialization.Abstract.Interfaces`) &ndash; serializes and deserializes `ExpressionNodeBase` graphs to a transport document type

Pair this package with `Komair.Expressions.Serialization.Json` in applications that need to send or persist expression trees.

## Dependencies

- [Komair.Expressions](https://www.nuget.org/packages/Komair.Expressions)

## Installation

```shell
dotnet add package Komair.Expressions.Serialization
```
