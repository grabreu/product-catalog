# Product Catalog

## Repository

A backend-only reference API for a product catalog: minimal writes, rich read/query surface (filtering, search, pagination, aggregation, facet counts). No frontend, ever. Read `README.md` before making changes.

## General Rules

- Keep changes scoped to the requested change.
- Prefer existing patterns over introducing new abstractions.
- Do not add dependencies unless they are necessary.
- Do not fill gaps with assumptions when the user hasn't given the information — ask, or mark it as pending.
- Do not claim a validation command passed unless it was actually run.
- Code, comments, commit messages, and documentation are always written in English.

## Git

- Do not create or switch branches unless explicitly requested.
- Do not create commits unless explicitly requested.
- Do not push unless explicitly requested.
- Keep commits focused on the requested change.
- Commit messages follow [Conventional Commits](https://www.conventionalcommits.org/) (`type: summary`).

## Documentation

### Audience

Future-you revisiting this months later, or someone browsing the portfolio to see how it works. Not onboarding material — keep it concise and skimmable.

### Content Rules

- State facts concisely. Avoid unnecessary explanations or trailing rationale.
- Do not document information that is already obvious from the repository structure or configuration.
- Do not invent features, API shapes, or future direction — mark undecided things as TODO.
- Document a capability only after it is implemented and verified.
- Use proper Markdown headings (`##`, `###`), not bold text as headings.

---

## Project-Specific Guidelines

### Architecture

- Vertical Slice Architecture: feature folders, each holding its own command/query + handler. No separate controller layer — Minimal API endpoints only.
- CQRS: commands for writes (`CreateProduct`, `UpdateProduct`, `ChangePrice`, `AdjustStock`, `DeactivateProduct`, `ReactivateProduct`), queries for reads (`ListProducts`, `GetProductById`, `GetCategoryFacetCounts`).
- Data: EF Core + SQL Server.
- The domain is deliberately flat: field validation only (`Price` > 0, `StockQuantity` >= 0), no state-based business rules. Do not add invariants beyond these two — the point of this project is engineering rigor around a simple domain, not domain complexity.
- No frontend code belongs in this repo, ever.

### Testing

- Full test pyramid: unit tests (domain/application) + integration tests against a real SQL Server via Testcontainers.
- Mutation testing with Stryker.NET proves the suite catches regressions, not just that it covers lines — run it, don't just add coverage.

### Observability & Quality Gates

- OpenTelemetry + Serilog for observability.
- SonarCloud is the CI quality gate.

### Deployment

- Azure Container Apps.

### Current State

- This is a redesign of the already-built [github.com/grabreu/product-catalog](https://github.com/grabreu/product-catalog). The existing README/docs predate the redesign and don't reflect it yet.
- Pending refactor: drop the in-progress frontend, move to Vertical Slice, add mutation testing.

### Open Questions

- TODO: exact filter/pagination shape (query string parameters vs. request body).
- TODO: Stryker.NET mutation score threshold to enforce in CI.
