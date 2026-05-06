# Validation Bridge - Remaining Decisions

## Open Questions

- Should adapters stay one-way (`specification/rules -> validators/artifacts`) or support limited reverse mapping (`validator -> specification`) for simple cases?
- Should severity/error-code be mandatory in core abstractions, or remain optional metadata?
- Should localization be required (`message key + fallback`) or optional?
- Do we want a hard minimum supported expression/spec shape list documented as a compatibility contract?

## DataAnnotations Direction

- Keep current runtime-artifact approach only, or also add optional spec-backed attribute helpers for `[MustBeX]` ergonomics?
- If spec-backed attributes are added, should they live in a dedicated package to keep core adapters minimal?

## External Reuse (YuckQi)

- Do we implement `Komair.Specifications.Validation.YuckQi` now, or defer to a later release?
- If implemented, should mapping target `Result` only, or support both `Result` and `Result<T>` projection paths from translation outcomes?
