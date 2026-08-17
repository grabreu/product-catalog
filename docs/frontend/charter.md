# Product Catalog Frontend — Charter

## Overview

A single-page React client for the Product Catalog API - full CRUD for
`Product` (list, create, edit, deactivate/reactivate), no auth. See
[`product-definition.md`](../product-definition.md) for why.

## Scope

Same entity, same operations as the API - see
[`product-definition.md`](../product-definition.md). No new business
rules; the frontend is a client, not a second source of truth.

## Facts

| Area            | Choice                                                          |
| ---------------- | ------------------------------------------------------------------ |
| Build tool        | Vite                                                                |
| Routing           | TanStack Router (file-based)                                       |
| Server state      | TanStack Query                                                     |
| Forms             | TanStack Form + Zod                                                 |
| UI components     | shadcn/ui + Tailwind CSS                                            |
| API client        | Generated from the API's OpenAPI document (`@hey-api/openapi-ts`)  |
| Local/UI state    | React `useState`/`useContext`; Zustand only if a real need appears |
| Testing           | Vitest + React Testing Library                                     |
| Lint/format       | Biome                                                               |
| Package manager   | pnpm                                                                |
| Hosting           | Azure Static Web Apps                                              |
| CI/CD             | GitHub Actions, scoped to `frontend/**` via path filters           |

See [`docs/adr/`](../adr/) for the reasoning behind these choices.

## Repository Structure

```text
frontend/
├── src/
│   ├── routes/          # TanStack Router file-based routes
│   ├── features/
│   │   └── products/    # components, hooks, and API bindings for Product
│   ├── components/ui/   # shadcn/ui primitives, shared across features
│   └── lib/              # generated API client, shared utilities
└── ...
```

## Build Milestones

The backend's roadmap (`V1`-`V5` in `product-definition.md`) is a
retrospective record of engineering practice reached, in order. This is
a prospective build plan instead - a different kind of list, so it uses
its own naming instead of continuing `V`:

- **M1 — Scaffold**: Vite + React + TypeScript, TanStack Router wired
  with a base route, shadcn/ui installed, Biome + pnpm configured. No
  feature yet - just the shell running.
- **M2 — Read path**: generated API client, TanStack Query, the Product
  list screen working end-to-end against the real API.
- **M3 — Write path**: create/edit forms (TanStack Form + Zod),
  deactivate/reactivate actions - full CRUD complete.
- **M4 — Testing & CI**: Vitest + React Testing Library component tests,
  Biome as a CI gate, pipeline scoped to `frontend/**`.
- **M5 — Deploy**: Azure Static Web Apps, CORS on the API, CD pipeline.

No documentation milestone: unlike the backend, where `V4` closed a gap
left by `V1`-`V3`, this charter and its ADRs were written before any of
the above - there's no later gap to close.
