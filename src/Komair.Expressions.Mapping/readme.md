# Komair.Expressions.Mapping

A .NET abstraction library for mapping `Komair.Expressions.ExpressionNode` objects to and from other representations (in particular `System.Linq.Expressions`).

## Key Types

Custom exceptions are in the `Komair.Expressions.Mapping.Exceptions` namespace. Root arguments are validated; passing null for an invalid root throws `ArgumentNullException`.

- **`InvalidMemberNodeException`** &ndash; thrown when a member expression node lacks the inner expression needed to resolve the member
- **`InvalidNodeRootException`** &ndash; thrown when a mapper expects a `LambdaExpressionNode` at the root but receives another `ExpressionNodeBase` type
- **`InvalidTreeRootException`** &ndash; thrown when a mapper expects a `LambdaExpression` at the root but receives another `System.Linq.Expressions.Expression` shape

## Key Concepts

- **mapping abstraction** &ndash; defines contracts for converting between serializable expression nodes and runtime expression trees
- **expression-node centric design** &ndash; focuses on `ExpressionNodeBase` and its derived node types as the mapping boundary

This package does not depend on any specific mapping framework; concrete implementations live in separate packages such as `Komair.Expressions.Mapping.Mapster`.

## Installation

```shell
dotnet add package Komair.Expressions.Mapping
```
