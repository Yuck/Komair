# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [10.1.0] - 2026-05-06

### Added

- Validation bridge package `Komair.Specifications.Validation.Abstractions` with normalized rule and translation contracts.
- FluentValidation adapter package `Komair.Specifications.Validation.FluentValidation` for projecting specification/rule descriptors into `IValidator<T>`.
- DataAnnotations adapter package `Komair.Specifications.Validation.DataAnnotations` for projecting specification/rule descriptors into runtime `ValidationAttribute` artifacts with metadata-only fallback support.
- New unit test projects for validation bridge adapters:
  - `Komair.Specifications.Validation.Abstractions.UnitTests`
  - `Komair.Specifications.Validation.FluentValidation.UnitTests`
  - `Komair.Specifications.Validation.DataAnnotations.UnitTests`
- Validation bridge sample documentation under `samples/`, including:
  - `AbstractValidator<T>` integration via `Include(...)`
  - FluentValidation and DataAnnotations usage from specification and rule descriptor inputs
  - ASP.NET Core model-binding flow with `IValidatableObject`

### Changed

- Updated root and package readmes to document validation bridge packages and usage patterns.
- Enforced namespace-to-folder alignment via `IDE0130` analyzer severity configuration.

## [10.0.1] - 2026-05-05

### Added

- Serializable expression nodes and Mapster mapping for additional LINQ shapes: conditional, `New`, member/list initializer, invocation, quote, and block expressions.
- JSON polymorphism and deserialization support for the new node types in `Komair.Expressions.Serialization.Json`.
- Unit test projects `Komair.Expressions.Serialization.UnitTests` and `Komair.Expressions.Mapping.UnitTests` (included in `Komair.slnx`).

### Changed

- `InvalidNodeRootException` and `InvalidTreeRootException` validate null roots before property initializers run so constructors throw `ArgumentNullException` instead of `NullReferenceException`.
- `ExpressionExtensions.GetParameterList` covers more expression kinds used in real LINQ trees.
- `DefaultTypeAdapterConfiguration` registers LINQ-to-node and node-to-LINQ adapters in separate helpers with alphabetized `ForType` entries.
- EditorConfig: enforce IDE0001 (simplified names), `EnforceCodeStyleInBuild`, and consolidated C# settings under `[*.{cs,csx,cake}]`.
- Unit tests reorganized per mapper; conventions clarified (no XML doc comments in tests).

## [10.0.0] - 2026-05-05

### Changed

- Upgraded all source and test projects to target `net10.0`.
- Updated CI and publish workflows to install the `.NET 10` SDK (`10.0.x`).
- Updated shared NuGet package metadata tags from `net8` to `net10`.
- Upgraded NuGet dependencies to newer releases, including `Mapster`, `Microsoft.NET.Test.Sdk`, `NUnit`, and `NUnit3TestAdapter`.

## [8.4.1] - 2026-05-04

### Added

- XML documentation comments across public API surfaces.
- `GenerateDocumentationFile` enabled for produced packages.
- Shared `KomairPackageTags` metadata for NuGet discovery.

## [8.4.0] - 2026-05-04

### Changed

- Replaced generic exceptions with typed exception types for clearer error handling.

## [8.3.0] - 2026-03-22

### Added

- `And` / `Or` overloads accepting parameter lists of specifications.

### Changed

- README updates (removed redundant CI detail).

## [8.2.1] - 2026-03-20

### Changed

- JSON serialization: unit test coverage and related serialization updates.

## [8.2.0] - 2026-03-14

### Changed

- JSON expression tree serialization now uses `System.Text.Json`.

## [8.1.0] - 2026-03-12

### Added

- Initial Komair monorepo layout with NuGet packages targeting `net8.0`.
- CI build/test workflow and tag-based NuGet publish workflow.
- Shared authoring, licensing, and repository metadata for packages.

[10.1.0]: https://github.com/Yuck/Komair/releases/tag/v10.1.0
[10.0.1]: https://github.com/Yuck/Komair/releases/tag/v10.0.1
[10.0.0]: https://github.com/Yuck/Komair/releases/tag/v10.0.0
[8.4.1]: https://github.com/Yuck/Komair/releases/tag/v8.4.1
[8.4.0]: https://github.com/Yuck/Komair/releases/tag/v8.4.0
[8.3.0]: https://github.com/Yuck/Komair/releases/tag/v8.3.0
[8.2.1]: https://github.com/Yuck/Komair/releases/tag/v8.2.1
[8.2.0]: https://github.com/Yuck/Komair/releases/tag/v8.2.0
[8.1.0]: https://github.com/Yuck/Komair/releases/tag/v8.1.0
