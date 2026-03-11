# komair.expressions.mapping.mapster

A Mapster-based implementation of the Komair expression mapping abstractions.

## key types

- **`MapsterExpressionNodeMapper<T>`** &ndash; maps between `ExpressionNodeBase` and `Expression<T>` using Mapster configuration
- **mapper configuration** &ndash; Mapster `TypeAdapterConfig` with profiles for all supported expression node shapes

## dependencies

- [Mapster](https://www.nuget.org/packages/Mapster)
- `Komair.Expressions`
- `Komair.Expressions.Mapping`

## installation

```shell
dotnet add package Komair.Expressions.Mapping.Mapster
```
