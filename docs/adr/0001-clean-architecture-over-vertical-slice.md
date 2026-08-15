# ADR-0001: Clean Architecture over Vertical Slice

## Status

Accepted

## Context

This project is the architectural baseline the rest of the portfolio is
compared against. Vertical Slice organizes code by use case rather than by
layer, and fits better when a project has many loosely related use cases
(e.g. a Change/Approval Workflow project elsewhere in the portfolio).

## Decision

Layered Clean Architecture: `Domain`, `Application`, `Infrastructure`,
`Api`, dependencies pointing inward.

## Consequences

- More widely recognizable pattern for a baseline other projects reference.
- Adds indirection (interfaces, mapping between layers) that wouldn't pay
  off in a project with few, tightly related use cases - Vertical Slice
  stays the better fit there.
