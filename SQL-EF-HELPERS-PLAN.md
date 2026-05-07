# SQL / EF Core helpers — implementation plan

This file tracks **remaining work** for optional data-layer helpers that compose [`ISpecification<T>`](src/Komair.Specifications/Abstract/Interfaces/ISpecification.cs) with `IQueryable<T>` (Entity Framework Core). **When an item below is finished, delete that bullet from this document** so the plan stays a concise backlog. Stable background and constraints stay in the sections above until they are no longer useful.

## Goals

- Keep **`Komair.Specifications`** free of EF Core (and SQL provider) dependencies so it stays usable in non-database scenarios.
- Ship an **optional** package—same idea as `Komair.Specifications.Validation.*`—that adds thin extension methods and/or documented patterns for EF Core’s `Where` pipeline.
- Prefer **expression trees that EF Core can translate**; document **parameterization** (constants captured in expressions vs values that become SQL parameters) so adopters avoid accidental SQL injection or plan-cache churn.

## Design constraints

- **Target framework**: match the repo (`net10.0` per [`src/Directory.Build.props`](src/Directory.Build.props)).
- **EF Core alignment**: reference **`Microsoft.EntityFrameworkCore`** (same major as the solution’s target stack—verify current stable version when adding the package).
- **Public API**: follow workspace C# conventions (file-scoped namespaces, XML docs on public members, package `readme.md` when `GeneratePackageOnBuild` is enabled).
- **Tests**: add `Komair.Specifications.EntityFrameworkCore.UnitTests` (or mirror the chosen package name) only if there are meaningful assertions; typical approaches include **EF Core InMemory** or **SQLite in-memory** for translation/smoke tests—avoid coupling tests to a specific SQL dialect unless truly necessary.

## Package sketch (subject to refinement)

| Aspect | Proposal |
|--------|----------|
| Name | `Komair.Specifications.EntityFrameworkCore` |
| Depends on | `Komair.Specifications`, `Microsoft.EntityFrameworkCore` |
| Primary surface | Extension methods on `IQueryable<T>` (and optionally `DbSet<T>` if they add clarity without duplication) |

## Documentation to ship in the package readme

Cover at least:

- **`query.Where(specification.ToExpression())`** vs a **`Where(this IQueryable<T>, ISpecification<T>)`** helper—same translation path; the helper is ergonomics and a single place for XML docs.
- **Parameterization**: EF Core binds **closure-captured variables** as parameters; **compile-time constants** often embed as literals—call out why `Where(x => x.Id == id)` is preferable to building strings, and when **`EF.Functions`** / database functions apply.
- **Untranslatable expressions**: specifications that compile fine for in-memory `IsSatisfiedBy` may still fail or client-evaluate under EF—point to diagnosing translation issues (logs, `ToQueryString()` where applicable).

---

## Remaining work

_Delete bullets here as each is completed._

- [ ] Implement **`IQueryable<T>` extensions** (names TBD, e.g. `Where(ISpecification<T>)`, optional `WhereIf(Boolean, ISpecification<T>)` or `WhereIf(Boolean, Expression<Func<T, Boolean>>)` if they reduce branching at call sites without encouraging untranslatable trees).
- [ ] Add **tests** that prove specifications compose with a minimal EF model and that queries execute (and, where feasible, assert **parameterized** SQL or stable query shape—without being brittle across EF patch versions).
- [ ] Update **[`README.md`](README.md)** Overview / Source Packages table with the new package row (and NuGet badge once published, if that is the house style for new packages).
- [ ] Record the release in **[`CHANGELOG.md`](CHANGELOG.md)** when shipping (project versioning follows [`src/Directory.Build.props`](src/Directory.Build.props)).

---

## Optional follow-ups (only if needed later)

_Delete items if you explicitly decide not to do them, or move them into **Remaining work** when scoped._

- Split **documentation-only** guidance into `samples/` (e.g. repository pattern snippet using specifications) if the readme grows too large.
- **`IAsyncEnumerable`** / **`AsAsyncEnumerable`** helpers only if there is a clear, safe API that does not fight EF’s execution model.
