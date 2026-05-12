# Komair.Specifications.EntityFrameworkCore

Entity Framework Core query composition helpers for Komair specifications. This package adds optional data-layer integration while keeping the core specification package provider-agnostic.

## Key Types

- **`QueryableSpecificationExtensions`** &ndash; `Where` applies an `ISpecification<T>` to an `IQueryable<T>` when the filter always applies.
- **`WhereIf`** &ndash; overloads that apply a specification or a predicate only when a Boolean condition is true, so optional filters (search fields, toggles) stay in one fluent chain without branching into separate `Where` calls. Prefer `Where` / `Queryable.Where` when the filter is unconditional. Arguments are still evaluated and null-checked before the method runs; see the XML documentation remarks on each overload for details.

## Dependencies

- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
- [Komair.Specifications](https://www.nuget.org/packages/Komair.Specifications/)

## Installation

```bash
dotnet add package Komair.Specifications.EntityFrameworkCore
```
