# Product Catalog Frontend — Charter

## Overview

A single-page React client for the Product Catalog API - full CRUD for
`Product` (list, create, edit, deactivate/reactivate), no auth. See
`product-definition.md` for why.

## Scope

Same entity, same operations as the API - see `product-definition.md`.
No new business rules; the frontend is a client, not a second source of
truth.

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

See [`docs/adr/`](adr/) for the reasoning behind these choices.

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

## Roadmap

Mirrors the backend's per-version engineering-practice approach (see
`product-definition.md`), applied fresh to this component - numbered
`F1`-`F4` to avoid colliding with the backend's own `V1`-`V5`:

- **F1** — Scaffold: Vite + React + TanStack (Router/Query/Form),
  shadcn/ui, generated API client, feature-first structure. Full CRUD
  screens for `Product`.
- **F2** — Component tests (Vitest + React Testing Library), Biome as a
  CI check.
- **F3** — Deploy: Azure Static Web Apps, CORS on the API, CD pipeline.
- **F4** — Documentation discipline: this charter, `frontend-architecture.md`,
  and the ADRs they reference - written before F1's code this time,
  applying what V4 established for the backend from the start.
