# product-catalog

## Repository

A backend-only reference API for a product catalog: minimal writes, filterable/paginated reads. No frontend, ever.

Read `README.md` before making changes — it documents the project pitch. Read `docs/architecture.md` for the domain model, invariants, and request flow. Significant, hard-to-reverse decisions are recorded in `docs/adr/` — check it before revisiting one, and add an entry when making a new one (see the `domain-modeling` skill for the format).

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

### Source

Generated, don't hand-edit:

- `Data/Migrations/*.cs` and `ApplicationDbContextModelSnapshot.cs` — regenerate with `dotnet ef migrations add <Name> --project src/ProductCatalog.Api`.

Layout: feature slices in `Features/Products/<UseCase>/` (command/query + handler + validator + endpoint), domain in `Domain/Products/`, EF Core config in `Data/`, cross-cutting pipeline behaviors in `Common/Behaviors/`.

### Validation

Run `dotnet format --verify-no-changes`, `dotnet build`, and `dotnet test` before considering a change done — CI (`.github/workflows/ci.yml`) runs the same on push/PR to `main`.

### Open Questions

- TODO: reintroduce the test pyramid and mutation testing (excluded during the redesign).
- TODO: Stryker.NET mutation score threshold to enforce in CI.
- TODO: `GetCategoryFacetCounts` — count of active products per category, the one query requiring aggregation rather than filtering. Not implemented yet.
- TODO: search — not implemented yet, exact shape undecided (by name? by SKU?).
