# Validation Bridge Project Plan

## Goal

Create an optional "validation bridge" so a single set of business rules can be represented as:

- `ISpecification<T>` predicates for LINQ/query scenarios
- FluentValidation rules for API/application input validation
- DataAnnotations attributes/metadata for DTO- and model-driven validation surfaces

The bridge should be additive and optional: existing specification usage remains unchanged.

## Non-Goals

- Replacing FluentValidation or DataAnnotations as primary frameworks
- Auto-translating every possible custom validator/attribute shape
- Runtime expression compilation tricks that break provider translation for EF/LINQ providers

## Proposed Package Shape

### Existing Baseline

- `Komair.Specifications` remains the core abstraction layer

### New Optional Packages

- `Komair.Specifications.Validation.Abstractions`
  - Shared bridge interfaces and rule descriptors
- `Komair.Specifications.Validation.FluentValidation`
  - Adapter from bridge descriptors/specifications to FluentValidation
- `Komair.Specifications.Validation.DataAnnotations`
  - Adapter from bridge descriptors/specifications to DataAnnotations-compatible output

This keeps dependencies optional and avoids pulling FluentValidation/DataAnnotations into core consumers unless explicitly referenced.

## Conceptual Architecture

## 1) Canonical Rule Representation

Introduce a canonical rule model that can express what both validation frameworks and specifications have in common.

Example concepts:

- `ValidationRule<T, TProperty>`
  - `PropertyPath`
  - `Predicate` (`Expression<Func<T, Boolean>>` and/or property predicate)
  - `ErrorCode`
  - `MessageTemplate`
  - `Severity`
  - `Tags`/metadata
- `IValidationRuleProvider<T>`
  - Returns normalized rules
- `IValidationBridge<T>`
  - Builds framework-specific artifacts from normalized rules/specifications

Why: specification trees are great for query composition, but validation frameworks need richer metadata (messages, member names, codes). A normalized model is the "bridge language."

## 2) Specification-to-Rule Mapping

Define explicit mapping strategies:

- Direct mapping for simple expression specifications (`x => x.Age >= 18`)
- Composite handling for `And`/`Or`/`Not` specifications
- "Unmappable" marker for rules that cannot safely translate to a target adapter

Add an opt-in contract for better fidelity:

- `IValidationAwareSpecification<T>`
  - Exposes message/code/property metadata in addition to predicate semantics

Why: pure predicates lose user-facing validation detail; this interface provides it without burdening all specifications.

## 3) Target Adapters

### FluentValidation Adapter

- Build `AbstractValidator<T>` rules from normalized descriptors
- Preserve:
  - property path
  - message
  - error code
  - severity (where possible)
- Support:
  - Whole-object rules (`RuleFor(x => x).Must(...)`)
  - Property-level rules (`RuleFor(x => x.Property).Must(...)`)

### DataAnnotations Adapter

Two-track strategy:

- Runtime validation attribute adapter for supported simple predicates
- Metadata export path (for OpenAPI/client generation pipelines) when direct attribute generation is limited

Because DataAnnotations is attribute-oriented and static by design, not every dynamic/composite rule can map cleanly. Provide graceful degradation and clear diagnostics.

## Cross-Cutting Design Decisions

## Diagnostics and Failure Modes

Define a translation result object:

- `Succeeded`
- `Warnings` (partial mapping)
- `Failures` (with reason categories: UnsupportedNodeType, MissingPropertyPath, DynamicRuleNotSupported, etc.)

This prevents silent rule loss.

## Rule Identity and Deduplication

Use stable IDs/signatures for rules so the same logical rule is not emitted multiple times when specs are composed.

## Localization

Store message keys + fallback text in rule metadata so adapters can use target framework localization patterns.

## Performance

- Cache translation output by specification type/signature
- Avoid repeated expression rewriting for hot paths

## Versioning and Compatibility

- Start with non-breaking additive APIs
- Keep bridge interfaces small and extensible to avoid frequent major changes

## Milestone Tracker

- [X] Phase 0 - Discovery and Constraints
- [X] Phase 1 - Abstractions Package
- [ ] Phase 2 - FluentValidation Adapter
- [ ] Phase 3 - DataAnnotations Adapter
- [ ] Phase 4 - Documentation and Samples

## Implementation Phases

## Phase 0 - Discovery and Constraints (1-2 days)

- [X] Inventory existing specification node types and composition patterns
- [X] Identify what is currently serializable/mappable in `Komair.Expressions*` that can be reused
- [X] Produce mapping matrix: Specification construct -> FluentValidation/DataAnnotations support level

Deliverable: mapping matrix and initial API sketch.

### Phase 0 findings

Status: complete.

Current specification model inventory (`Komair.Specifications`):

- `ISpecification<T>` exposes `ToExpression()`, `And(...)`, `Or(...)`, `Not()`, `Where(...)`, and `IsSatisfiedBy(...)`
- Composition is currently implemented with internal wrapper specifications:
  - `AndSpecification<T>` -> `Expression.AndAlso(...)`
  - `OrSpecification<T>` -> `Expression.OrElse(...)`
  - `NotSpecification<T>` -> `Expression.Not(...)`
  - `ExpressionSpecification<T>` wraps direct `Expression<Func<T, Boolean>>`
- Baseline identities:
  - `TrueSpecification<T>.Identity`
  - `FalseSpecification<T>.Identity`

Reusable expression infrastructure inventory (`Komair.Expressions*`):

- `Komair.Expressions` provides node model abstraction (`ExpressionNodeBase`) and concrete nodes:
  - `BinaryExpressionNode`, `BlockExpressionNode`, `ConditionalExpressionNode`, `ConstantExpressionNode`
  - `InvocationExpressionNode`, `LambdaExpressionNode`, `ListInitExpressionNode`, `MemberExpressionNode`
  - `MemberInitExpressionNode`, `NewExpressionNode`, `ParameterExpressionNode`, `QuoteExpressionNode`
- `Komair.Expressions.Mapping.Mapster` already maps between LINQ expression trees and node graphs in both directions
- Root mapping constraints already exist and can inform bridge diagnostics:
  - invalid root throws `InvalidTreeRootException` / `InvalidNodeRootException`
  - unsupported node kinds throw `UnsupportedExpressionException`

### Mapping Matrix (Phase 0, v1 target posture)

- `ExpressionSpecification<T>` with member-access predicate
  - FluentValidation: strong support (property rule or object rule depending on path extraction)
  - DataAnnotations: partial support (attribute generation only for simple/property-addressable predicates)
- `AndSpecification<T>`
  - FluentValidation: strong support (emit multiple validators / combined must chain)
  - DataAnnotations: partial support (may require multiple attributes or metadata export)
- `OrSpecification<T>`
  - FluentValidation: partial support (object-level `Must` often safest; per-property decomposition may be lossy)
  - DataAnnotations: weak/metadata-only for many cases
- `NotSpecification<T>`
  - FluentValidation: partial support (`Must` inversion is possible, but message semantics must be explicit)
  - DataAnnotations: weak/metadata-only for many cases
- `TrueSpecification<T>` / `FalseSpecification<T>`
  - FluentValidation: supported but usually optimized away during translation
  - DataAnnotations: supported as no-op/fail-fast metadata forms
- Arbitrary expression nodes with no stable property path
  - FluentValidation: object-level rule fallback with diagnostics
  - DataAnnotations: metadata export fallback with diagnostics

Initial API sketch from discovery:

- `IValidationRuleProvider<T>`: emits normalized validation rule descriptors
- `ValidationRuleDescriptor<T>`: canonical rule with predicate, optional property path, code, message, severity, tags
- `ValidationTranslationResult<TArtifact>`: output artifact(s) plus warnings/failures
- `ValidationTranslationFailure`: structured reason (`UnsupportedNodeType`, `MissingPropertyPath`, `AmbiguousComposite`, `DynamicRuleNotSupported`)
- `IValidationAwareSpecification<T>` (optional): lets specification authors provide message/code/path metadata for high-fidelity adapter output

## Phase 1 - Abstractions Package (2-4 days)

- [X] Create `Komair.Specifications.Validation.Abstractions`
- [X] Add canonical rule descriptors, translation result types, and extension points
- [X] Add XML docs for all public API

Deliverable: compile-ready abstractions package with tests.

### Phase 1 completion notes

- Added new package: `src/Komair.Specifications.Validation.Abstractions`
- Added core contracts:
  - `IValidationRuleProvider<T>`
  - `IValidationBridge<T, TArtifact>`
  - `IValidationAwareSpecification<T>`
- Added canonical abstractions:
  - `ValidationRuleDescriptor<T>`
  - `ValidationTranslationResult<TArtifact>`
  - `ValidationTranslationFailure`
  - `ValidationTranslationWarning`
  - `ValidationSeverity`
  - `ValidationSupportLevel`
  - `ValidationTranslationFailureReason`
- Added unit test project: `test/Komair.Specifications.Validation.Abstractions.UnitTests`
- Added tests covering constructor guards and translation result semantics

## Phase 2 - FluentValidation Adapter (3-5 days)

- Create `Komair.Specifications.Validation.FluentValidation`
- Implement converter pipeline from rules/specifications to `AbstractValidator<T>`
- Add coverage for simple, composite, and partially mappable cases

Deliverable: adapter package + integration tests.

## Phase 3 - DataAnnotations Adapter (3-5 days)

- Create `Komair.Specifications.Validation.DataAnnotations`
- Implement best-effort attribute/metadata generation
- Add explicit diagnostics for unsupported dynamic/composite scenarios

Deliverable: adapter package + integration tests.

## Phase 4 - Documentation and Samples (2-3 days)

- Add package readmes and update root docs with bridge usage
- Provide sample:
  - one domain model
  - one specification set
  - API validator wiring for both adapters

Deliverable: end-to-end sample and updated docs.

## Testing Strategy

## Unit Tests

- Canonical rule construction/validation
- Mapping of each specification composition type
- Diagnostics correctness for unsupported cases

## Integration Tests

- FluentValidation execution parity with specification predicate results
- DataAnnotations behavior parity for supported constructs
- LINQ provider compatibility checks (ensuring specification expressions remain provider-translatable)

## Golden/Approval Tests

- Stable snapshots for generated adapter outputs (especially metadata/export forms)

## Risks and Mitigations

- Semantic mismatch between predicates and validation UX
  - Mitigation: `IValidationAwareSpecification<T>` metadata contract
- DataAnnotations static nature limits dynamic rule translation
  - Mitigation: metadata export + explicit unsupported diagnostics
- Rule drift between query specs and API validators
  - Mitigation: parity tests and shared canonical descriptor pipeline

## Open Questions to Refine

- Should adapters be one-way (specification -> validators) only, or also support validator -> specification imports for simple cases?
- Do we want severity/error-code concepts in core abstractions immediately, or behind optional interfaces?
- Should rule localization be mandatory (message key required) or optional?
- What minimum set of specification node types must be supported in v1?

## External Reuse Notes

- `YuckQi.Domain.Validation` is a potential integration target for result/diagnostic projection, not a required core dependency for bridge abstractions.
- Keep `Komair.Specifications.Validation.Abstractions` framework-neutral and specification-focused.
- Prefer optional integration via a dedicated adapter package (for example `Komair.Specifications.Validation.YuckQi`) that maps:
  - `ValidationTranslationResult<TArtifact>` -> `Result` / `Result<T>`
  - `ValidationTranslationFailure` / `ValidationTranslationWarning` -> `ResultDetail` equivalents
- Revisit implementation in Phase 4 during docs/sample work, where a sample can demonstrate end-to-end mapping into YuckQi-style API responses.

## Suggested v1 Scope

- Support expression-based and `And`/`Or`/`Not` composite specifications
- Full FluentValidation adapter for supported rules
- DataAnnotations best-effort adapter with explicit diagnostics
- Clear docs and examples showing "single source of truth" rule authoring

## Exit Criteria (v1)

- Developers can define rules once and reuse them in query + API validation layers
- Translation failures are explicit and testable (no silent drops)
- Adapter packages are optional and independently versioned
- Documentation shows realistic usage and known limitations
