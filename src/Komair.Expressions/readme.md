# komair.expressions

A .NET library providing serializable equivalents of the `System.Linq.Expressions` namespace.

## key types

- **`ExpressionNodeBase`** &ndash; abstract base for serializable expression nodes
- **`BinaryExpressionNode`** &ndash; serializable representation of `BinaryExpression`
- **`ConstantExpressionNode`** &ndash; serializable representation of `ConstantExpression`
- **`LambdaExpressionNode`** &ndash; serializable representation of `LambdaExpression`
- **`MemberExpressionNode`** &ndash; serializable representation of `MemberExpression`
- **`ParameterExpressionNode`** &ndash; serializable representation of `ParameterExpression`

## usage

Use these nodes together with the mapping and serialization packages (`Komair.Expressions.Mapping.*`, `Komair.Expressions.Serialization.*`) to transform between expression trees and transport-friendly formats.

## installation

```shell
dotnet add package Komair.Expressions
```
