# ADR-0011: Feature-first folder structure

## Status

Accepted

## Context

TanStack Router already imposes file-based routing under `routes/`.
Everything else - components, hooks, API bindings - needs an organizing
principle. Grouping by technical type (`components/`, `hooks/`,
`services/`) scatters everything related to one concept (e.g. Product)
across unrelated folders.

## Decision

`features/products/` holds that feature's own components, hooks, and API
bindings together. `components/ui/` stays reserved for shadcn/ui's
generated primitives, shared across features.

## Consequences

- Inverse of the backend's own convention (kind-first, then aggregate -
  e.g. `Commands/Products/Create/`): the frontend groups by feature
  first, then kind. Different structural pressures (routed pages vs.
  CQRS kind boundaries) justify the different axis; the shared principle
  is keeping related code together, not a literal folder-shape match.
- Scales better as more features are added; for a single-entity app the
  benefit today is mostly setting the precedent.
