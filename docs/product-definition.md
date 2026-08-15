# Product Catalog — Product Definition

## 1. The Problem

There is no real business problem being solved here. This is a meta-need:
a clean, boring reference project to validate architecture, testing, and
CI/CD patterns before applying them elsewhere. The person who feels this
need is the author, as a developer - not an external user.

## 2. Domain Familiarity

Product catalog is a universally known domain - every developer has seen it
in tutorials, sample projects, and boilerplates. It does not come from
personal or professional experience, and that's fine here: this project's
value doesn't depend on domain depth.

## 3. Existing Products

Many CRUD boilerplates already exist (Microsoft's eShopOnWeb, various
TodoMVC-style samples, etc.). The difference here is a specific, consistent
set of conventions - folder structure, testing approach, documentation
style - applied deliberately, rather than a generic third-party tutorial.

## 4. Scope — MVP

Full CRUD for Product:

- Name
- SKU
- Description
- Price
- Category (fixed enum)
- Stock quantity
- Active/inactive flag
- Created/updated timestamps

Simple listing with basic pagination.

**Non-goals:** no login, no multi-user/multi-tenancy, no approval flow, no
external integrations, no elaborate UI.

**No login/authentication by design** - single implicit user, no accounts.
This is intentional: zero auth complexity so nothing distracts from clean
fundamentals.

## 5. Business Rules

Minimal, but present:

- SKU must be unique
- Price must be greater than 0
- Stock quantity cannot be negative
- Deactivation is a soft delete (marks inactive), not physical removal, and
  is reversible (`ReactivateProduct`). Exposed as `POST .../deactivate` and
  `POST .../reactivate`, not the `DELETE` verb - `DELETE` implies permanent
  removal, which this domain doesn't have. See `architecture.md` and
  ADR-0005 for the reasoning

## 6. Future Evolution

Future versions introduce engineering practice, not domain features:

- **V1** — Clean architecture baseline + unit tests
- **V2** — Integration tests + CI pipeline
- **V3** — Observability/logging
- **V4** — Documentation discipline / Architecture Decision Records (ADRs)
- **V5** — Frontend (React) - architecture decided when this version
  starts, not guessed at now

Each version is an exercise in engineering maturity, not business
complexity.

## 7. Engineering Competencies

- Clean architecture skeleton
- Testing practices (unit -> integration over versions)
- Documentation conventions
- CI/CD setup

If this project were removed, there would be no clean baseline to compare
more complex systems against. See `charter.md`, `domain-model.md`, and
`architecture.md` for the resulting technical decisions.
