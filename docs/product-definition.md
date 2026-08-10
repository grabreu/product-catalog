# Product Catalog — Product Definition

## 1. The Problem

There is no real business problem being solved here. This is a meta-need:
a clean, boring reference project to validate architecture, testing, and CI
patterns before applying them to the domain-rich projects in the portfolio.
The person who feels this need is the author, as a developer organizing
their own portfolio - not an external user.

## 2. Domain Familiarity

Product catalog is a universally known domain - every developer has seen it
in tutorials, sample projects, and boilerplates. It does not come from
personal or professional experience, and that's fine here: this project's
value doesn't depend on domain depth.

## 3. Personal Motivation

Not used as a final product. Used as a living template - something to
revisit when starting a new project to recall "how did I structure
validation/testing last time."

## 4. Existing Products

Many CRUD boilerplates already exist (Microsoft's eShopOnWeb, various
TodoMVC-style samples, etc.). The difference here is that this one is
calibrated to the author's own stack and conventions (folder structure,
testing approach, documentation style), not a generic third-party tutorial.

## 5. Scope — MVP

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

## 6. Business Rules

Minimal, but present:

- SKU must be unique
- Price must be greater than 0
- Stock quantity cannot be negative
- Delete is a soft delete (marks inactive), not physical removal - a small
  but real architectural decision worth documenting

## 7. Future Evolution

Unlike the other portfolio projects, future versions here introduce
_engineering practice_, not _domain features_:

- **V1** — Clean architecture baseline + unit tests
- **V2** — Integration tests + CI pipeline
- **V3** — Observability/logging
- **V4** — Documentation discipline / Architecture Decision Records (ADRs)
- **V5** — Frontend (React) - architecture decided when this version
  starts, not guessed at now

Each version is an exercise in engineering maturity, not business
complexity.

## 8. Engineering Competencies

- Clean architecture skeleton
- Testing practices (unit -> integration over versions)
- Documentation conventions
- CI/CD setup

If this project were removed from the portfolio, there would be no clean
baseline to compare the other, more complex projects against.

---

## Portfolio Fit Checklist

- [x] Do I understand this domain? (Trivially, by design)
- [x] Is the problem realistic? (Meta-problem: needing a clean baseline)
- [x] Would I use this system? (As a living reference template)
- [x] Can I explain the business without mentioning technology? (N/A by
      design - this project exists to validate technology/practice, not to
      model a business)
- [x] Is the scope intentionally small? (Yes - smallest project in the
      portfolio)
- [x] Does this project demonstrate competencies that another project does
      not? (Yes - clean baseline architecture/testing/CI discipline)
- [x] If removed, would a specific competency disappear? (Yes - the
      reference point for engineering practice maturity)

All criteria met - see `charter.md`, `domain-model.md`, and `architecture.md`
for the resulting technical decisions.
