# Komair.Expressions.Mapping.Mapster

A Mapster-based implementation of the Komair expression mapping abstractions.

## Key Types

- **`MapsterExpressionNodeMapper<T>`** &ndash; maps between `ExpressionNodeBase` and `Expression<T>` using Mapster configuration
- **mapper configuration** &ndash; Mapster `TypeAdapterConfig` with profiles for all supported expression node shapes

## Dependencies

- [Mapster](https://www.nuget.org/packages/Mapster)
- `Komair.Expressions`
- `Komair.Expressions.Mapping`

## Installation

```shell
dotnet add package Komair.Expressions.Mapping.Mapster
```
