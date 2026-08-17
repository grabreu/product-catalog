# ADR-0009: Vite SPA + TanStack ecosystem over a full-stack meta-framework

## Status

Accepted

## Context

The API already exists as a separately deployed service (ASP.NET Core on
Azure Container Apps). The frontend only needs to consume it - no SEO
requirement (internal CRUD demo), no server-only data to protect (no
auth). TanStack Start and Next.js add SSR, server functions, and a Node
server to operate, solving problems this project doesn't have.

## Decision

Vite + React, client-rendered SPA. Routing, server state, and forms all
come from the TanStack family - Router, Query, Form with Zod validation -
for one consistent API and mental model across the three, instead of
mixing libraries from unrelated ecosystems (e.g. react-router + redux +
react-hook-form).

## Consequences

- No Node server to operate for the frontend - ships as static files.
- No SSR/SEO capability if a future version needs it; would mean
  introducing a framework at that point.
- Consistent conventions across Router/Query/Form, at the cost of each
  individually having a smaller community than its most popular
  alternative (react-router, react-hook-form).
