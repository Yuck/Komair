# Komair.Specifications.EntityFrameworkCore

Entity Framework Core query composition helpers for Komair specifications. This package adds optional data-layer integration while keeping the core specification package provider-agnostic.

## Key Types

- **`QueryableSpecificationExtensions`** &ndash; `Where` applies an `ISpecification<T>` to an `IQueryable<T>` when the filter always applies.
- **`WhereIf`** &ndash; overloads that apply a specification or a predicate only when a Boolean condition is true, so optional filters (search fields, toggles) stay in one fluent chain without branching into separate `Where` calls. Prefer `Where` / `Queryable.Where` when the filter is unconditional. Arguments are still evaluated and null-checked before the method runs; see the XML documentation remarks on each overload for details.

## `Where` helper vs `specification.ToExpression()`

`query.Where(specification.ToExpression())` and `query.Where(specification)` compile to the same LINQ shape: both supply an `Expression<Func<T, Boolean>>` to `Queryable.Where`. The extension method is ergonomics, a discoverable entry point, and a single place for XML documentation; it does not change EF Core translation.

## Parameterization and literals

EF Core binds values captured from the surrounding scope (closure fields, locals, parameters) as **parameters** in SQL, which helps plan cache reuse and avoids treating user input as raw SQL. The exact placeholder names depend on the provider and EF version (for example `@p0` or `@_id` in diagnostic SQL).

Compile-time **constants** inside the expression tree are often emitted as **literals** in SQL. Prefer `Where(t => t.Id == id)` with a variable or parameter `id` over building text that you splice into queries.

Use **`EF.Functions`** (and related database-function surfaces) when you need provider-translated calls such as `Like` or collations; ordinary .NET methods on entities may not translate.

## Untranslatable or client-evaluated expressions

Specifications that work for in-memory `IsSatisfiedBy` can still fail translation or fall back to **client evaluation** (or throw) when used as `IQueryable` filters, depending on the provider and expression shape. When a query misbehaves, enable detailed logging, inspect the generated SQL (`ToQueryString()` on relational providers where supported), and consult EF Core documentation for translation rules.

## Dependencies

- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
- [Komair.Specifications](https://www.nuget.org/packages/Komair.Specifications/)

## Installation

```bash
dotnet add package Komair.Specifications.EntityFrameworkCore
```
