# ADR-0001: Clean Architecture over Vertical Slice

## Status

Accepted

## Context

This system has a small number of tightly related use cases (single-entity
CRUD: create, read, update, deactivate/reactivate). Vertical Slice
organizes code by use case rather than by layer, and fits better when a
system has many loosely related use cases that don't share much
structure.

## Decision

Layered Clean Architecture: `Domain`, `Application`, `Infrastructure`,
`Api`, dependencies pointing inward.

## Consequences

- Widely recognizable pattern, easy to onboard onto without prior context.
- Adds indirection (interfaces, mapping between layers) that wouldn't pay
  off in a system with few, tightly related use cases - Vertical Slice
  would be the better fit there.
