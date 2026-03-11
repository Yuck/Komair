# Komair

A .NET solution of lightweight, composable libraries for working with expression trees and specifications. It provides serializable equivalents of `System.Linq.Expressions`, abstractions for mapping and serialization, and an implementation of the Specification pattern.

The repository layout, build configuration, and CI are structured similarly to [YuckQi](https://github.com/Yuck/YuckQi).

## Overview

Komair provides:

- **Expressions** — A .NET library providing serializable equivalents of the `System.Linq.Expressions` namespace (`Komair.Expressions`).
- **Expression serialization** — Abstractions and concrete implementations for serializing expression nodes (`Komair.Expressions.Serialization`, `Komair.Expressions.Serialization.Json`).
- **Expression mapping** — Abstractions and Mapster-based implementations for mapping between expression representations (`Komair.Expressions.Mapping`, `Komair.Expressions.Mapping.Mapster`).
- **Specifications** — A .NET implementation of the Specification pattern (`Komair.Specifications`).

## Repository Structure

| Folder   | Contents                                                              |
|---------|-----------------------------------------------------------------------|
| **src/**  | NuGet-ready class libraries (net8.0). Each package may be published independently. |
| **test/** | Unit test projects aligned to the source projects.                  |

### Source Packages

| Package                                   | Description |
|-------------------------------------------|-------------|
| **Komair.Expressions**                    | Serializable equivalents of `System.Linq.Expressions`. |
| **Komair.Expressions.Serialization**      | Abstractions for serializing `ExpressionNode` objects. |
| **Komair.Expressions.Serialization.Json** | JSON-based implementation of expression serialization (Newtonsoft.Json). |
| **Komair.Expressions.Mapping**            | Abstractions for mapping `ExpressionNode` objects to/from other representations. |
| **Komair.Expressions.Mapping.Mapster**    | Mapster-based implementation of expression mapping. |
| **Komair.Specifications**                 | Implementation of the Specification pattern. |

## Building and Testing

```bash
dotnet build
dotnet test
```

## CI

GitHub Actions (`.github/workflows/ci.yml`) runs `dotnet build` and `dotnet test` on pull requests and pushes to `main`/`master`, using the `Komair.slnx` solution file.

Requires [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
