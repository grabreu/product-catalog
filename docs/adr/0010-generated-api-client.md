# ADR-0010: Generated API client from the OpenAPI spec

## Status

Accepted

## Context

The API already publishes an OpenAPI document (`/openapi/v1.json`).
Hand-written fetch calls and TypeScript types drift from the API's actual
shape whenever the API changes - silently, until something breaks at
runtime.

## Decision

`@hey-api/openapi-ts` generates TypeScript types and API functions from
the OpenAPI document, wired into TanStack Query hooks. Hand-written hooks
are being tried alongside it early on to compare the two before
committing further.

## Consequences

- Regenerating after an API change surfaces type errors at compile time
  instead of at runtime.
- Adds a codegen step to the frontend build; generated code needs
  re-running whenever the API's OpenAPI document changes.
