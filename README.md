# Komair

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A .NET solution of lightweight, composable libraries for working with expression trees and specifications. It provides serializable equivalents of `System.Linq.Expressions`, abstractions for mapping and serialization, and an implementation of the Specification pattern.

## Overview

Komair provides:

- **Expressions** — A .NET library providing serializable equivalents of the `System.Linq.Expressions` namespace (`Komair.Expressions`).
- **Expression serialization** — Abstractions and concrete implementations for serializing expression nodes (`Komair.Expressions.Serialization`, `Komair.Expressions.Serialization.Json`, `Komair.Expressions.Serialization.MessagePack`).
- **Expression mapping** — Abstractions and Mapster-based implementations for mapping between expression representations (`Komair.Expressions.Mapping`, `Komair.Expressions.Mapping.Mapster`).
- **Specifications** — A .NET implementation of the Specification pattern (`Komair.Specifications`).
- **Specifications + EF Core** — Optional `IQueryable<T>` helpers that compose specifications with Entity Framework Core (`Komair.Specifications.EntityFrameworkCore`).
- **Validation bridge adapters** — Optional adapters that project shared rules to FluentValidation and DataAnnotations (`Komair.Specifications.Validation.*`).

## Repository Structure

| Folder   | Contents                                                              |
|---------|-----------------------------------------------------------------------|
| **src**  | NuGet-ready class libraries (net10.0). Each package may be published independently. |
| **test** | Unit test projects aligned to the source projects.                  |

### Source Packages

| Package                                   | NuGet | Description |
|-------------------------------------------|-------|-------------|
| **Komair.Expressions**                    | [![NuGet](https://img.shields.io/nuget/v/Komair.Expressions.svg)](https://www.nuget.org/packages/Komair.Expressions) | Serializable equivalents of `System.Linq.Expressions`. |
| **Komair.Expressions.Serialization**      | [![NuGet](https://img.shields.io/nuget/v/Komair.Expressions.Serialization.svg)](https://www.nuget.org/packages/Komair.Expressions.Serialization) | Abstractions for serializing `ExpressionNode` objects. |
| **Komair.Expressions.Serialization.Json** | [![NuGet](https://img.shields.io/nuget/v/Komair.Expressions.Serialization.Json.svg)](https://www.nuget.org/packages/Komair.Expressions.Serialization.Json) | System.Text.Json implementation of expression serialization. |
| **Komair.Expressions.Serialization.MessagePack** | [![NuGet](https://img.shields.io/nuget/v/Komair.Expressions.Serialization.MessagePack.svg)](https://www.nuget.org/packages/Komair.Expressions.Serialization.MessagePack) | MessagePack implementation of expression serialization. |
| **Komair.Expressions.Mapping**            | [![NuGet](https://img.shields.io/nuget/v/Komair.Expressions.Mapping.svg)](https://www.nuget.org/packages/Komair.Expressions.Mapping) | Abstractions for mapping `ExpressionNode` objects. |
| **Komair.Expressions.Mapping.Mapster**    | [![NuGet](https://img.shields.io/nuget/v/Komair.Expressions.Mapping.Mapster.svg)](https://www.nuget.org/packages/Komair.Expressions.Mapping.Mapster) | Mapster-based implementation of expression mapping. |
| **Komair.Specifications**                 | [![NuGet](https://img.shields.io/nuget/v/Komair.Specifications.svg)](https://www.nuget.org/packages/Komair.Specifications) | Implementation of the Specification pattern. |
| **Komair.Specifications.EntityFrameworkCore** | [![NuGet](https://img.shields.io/nuget/v/Komair.Specifications.EntityFrameworkCore.svg)](https://www.nuget.org/packages/Komair.Specifications.EntityFrameworkCore) | Optional EF Core helpers for composing specifications with `IQueryable<T>`. |
| **Komair.Specifications.Validation.Abstractions** | [![NuGet](https://img.shields.io/nuget/v/Komair.Specifications.Validation.Abstractions.svg)](https://www.nuget.org/packages/Komair.Specifications.Validation.Abstractions) | Shared validation bridge rule and translation contracts. |
| **Komair.Specifications.Validation.FluentValidation** | [![NuGet](https://img.shields.io/nuget/v/Komair.Specifications.Validation.FluentValidation.svg)](https://www.nuget.org/packages/Komair.Specifications.Validation.FluentValidation) | FluentValidation adapter for shared validation bridge rules. |
| **Komair.Specifications.Validation.DataAnnotations** | [![NuGet](https://img.shields.io/nuget/v/Komair.Specifications.Validation.DataAnnotations.svg)](https://www.nuget.org/packages/Komair.Specifications.Validation.DataAnnotations) | DataAnnotations adapter with metadata fallback and diagnostics. |

## Validation Bridge Sample

- Sample index: `samples/validation-bridge-end-to-end.md`
- `AbstractValidator<T>` integration example: `samples/validation-bridge-fluentvalidator-class.md`
- DataAnnotations class integration example: `samples/validation-bridge-dataannotations-class.md`

## Building and Testing

```bash
dotnet build
dotnet test
```
