# Komair.Specifications

A .NET implementation of the Specification pattern for building composable business rules.

## Key Types

- **`ISpecification<T>`** &ndash; abstraction for a predicate that can be combined and evaluated
- **`SpecificationBase<T>`** &ndash; base class with `And`, `Or`, and `Not` combinators and `ToExpression` for LINQ providers
- **`TrueSpecification<T>.Identity` / `FalseSpecification<T>.Identity`** &ndash; neutral elements for folding specifications: true for `And`, false for `Or`
- **`And` / `Or` with `params`** &ndash; fold any number of specifications onto the receiver (e.g. `mySpec.And(a, b)` is equivalent to chaining `mySpec.And(a).And(b)`; same idea for `Or`)

## Usage

Use the always-true identity when you want “and together these specs” without an existing left-hand specification:

```csharp
var spec = TrueSpecification<Order>.Identity.And(active, paid, notCancelled);
```

Use the always-false identity when you want “or together these specs” the same way:

```csharp
var spec = FalseSpecification<Order>.Identity.Or(draft, archived, pendingReview);
```

With no extra arguments, `TrueSpecification<T>.Identity.And()` stays always true, and `FalseSpecification<T>.Identity.Or()` stays always false. You can still start from any other specification instead (e.g. `active.And(paid, shipped)`).

Use specifications to encapsulate complex predicates, compose them fluently, and reuse them across repositories, query handlers, and validation layers.

## Installation

```shell
dotnet add package Komair.Specifications
```
