# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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

[8.4.1]: https://github.com/Yuck/Komair/releases/tag/v8.4.1
[8.4.0]: https://github.com/Yuck/Komair/releases/tag/v8.4.0
[8.3.0]: https://github.com/Yuck/Komair/releases/tag/v8.3.0
[8.2.1]: https://github.com/Yuck/Komair/releases/tag/v8.2.1
[8.2.0]: https://github.com/Yuck/Komair/releases/tag/v8.2.0
[8.1.0]: https://github.com/Yuck/Komair/releases/tag/v8.1.0
