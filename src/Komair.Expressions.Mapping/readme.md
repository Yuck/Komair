# Komair.Expressions.Mapping

A .NET abstraction library for mapping `Komair.Expressions.ExpressionNode` objects to and from other representations (in particular `System.Linq.Expressions`). Concrete mappers (such as Mapster) live in separate packages and implement `IExpressionNodeMapper`.

## Key Types

- **`IExpressionNodeMapper<T>`** (`Komair.Expressions.Mapping.Abstract.Interfaces`) &ndash; maps between `ExpressionNodeBase` graphs and `System.Linq.Expressions.Expression` trees
- **`InvalidMemberNodeException`** (`Komair.Expressions.Mapping.Exceptions`) &ndash; thrown when a member expression node lacks the inner expression needed to resolve the member
- **`InvalidNodeRootException`** (`Komair.Expressions.Mapping.Exceptions`) &ndash; thrown when a mapper expects a `LambdaExpressionNode` at the root but receives another `ExpressionNodeBase` type
- **`InvalidTreeRootException`** (`Komair.Expressions.Mapping.Exceptions`) &ndash; thrown when a mapper expects a `LambdaExpression` at the root but receives another `System.Linq.Expressions.Expression` shape

Root arguments are validated; passing null for an invalid root throws `ArgumentNullException`.

## Dependencies

- [Komair.Expressions](https://www.nuget.org/packages/Komair.Expressions)

## Installation

```shell
dotnet add package Komair.Expressions.Mapping
```
