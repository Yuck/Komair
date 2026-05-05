# Komair.Expressions

A .NET library providing serializable equivalents of the `System.Linq.Expressions` namespace.

## Key Types

- **`ExpressionNodeBase`** &ndash; abstract base for serializable expression nodes
- **`BinaryExpressionNode`** &ndash; serializable representation of `BinaryExpression`
- **`BlockExpressionNode`** &ndash; serializable representation of `BlockExpression`
- **`ConditionalExpressionNode`** &ndash; serializable representation of `ConditionalExpression`
- **`ConstantExpressionNode`** &ndash; serializable representation of `ConstantExpression`
- **`ElementInitNode`** &ndash; serializable representation of list initializer elements (`ElementInit`)
- **`InvocationExpressionNode`** &ndash; serializable representation of `InvocationExpression`
- **`LambdaExpressionNode`** &ndash; serializable representation of `LambdaExpression`
- **`ListInitExpressionNode`** &ndash; serializable representation of `ListInitExpression`
- **`MemberAssignmentNode`** &ndash; serializable representation of member initializer assignments (`MemberAssignment`)
- **`MemberInitExpressionNode`** &ndash; serializable representation of `MemberInitExpression`
- **`MemberExpressionNode`** &ndash; serializable representation of `MemberExpression`
- **`MethodCallExpressionExtensions`** &ndash; collects `ParameterExpression` nodes referenced under a `MethodCallExpression` (used with `ExpressionExtensions.GetParameterList`)
- **`NewExpressionNode`** &ndash; serializable representation of `NewExpression`
- **`ParameterExpressionNode`** &ndash; serializable representation of `ParameterExpression`
- **`QuoteExpressionNode`** &ndash; serializable representation of quoted expressions (`ExpressionType.Quote`)
- **`UnsupportedExpressionException`** (`Komair.Expressions.Exceptions`) &ndash; thrown when an operation encounters a `System.Linq.Expressions.Expression` shape it does not support (for example `GetParameterList` on an unsupported node kind)

## Usage

Use these nodes together with the mapping and serialization packages (`Komair.Expressions.Mapping.*`, `Komair.Expressions.Serialization.*`) to transform between expression trees and transport-friendly formats.

## Installation

```shell
dotnet add package Komair.Expressions
```
