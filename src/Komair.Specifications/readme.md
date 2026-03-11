# komair.specifications

A .NET implementation of the Specification pattern for building composable business rules.

## key concepts

- **`ISpecification<T>`** &ndash; abstraction for a predicate that can be combined and evaluated
- **combinators** &ndash; `And`, `Or`, and `Not` specifications for composing more complex rules
- **expression pipeline** &ndash; specifications expose `Expression<Func<T, Boolean>>` for use with LINQ providers

## typical usage

Use specifications to encapsulate complex predicates, compose them fluently, and reuse them across repositories, query handlers, and validation layers.

## installation

```shell
dotnet add package Komair.Specifications
```
