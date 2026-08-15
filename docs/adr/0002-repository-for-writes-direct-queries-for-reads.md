# ADR-0002: Repository for writes, direct queries for reads

## Status

Accepted

## Context

A repository interface generic enough to serve every read shape a UI needs
tends to either leak `IQueryable` past the domain boundary or accumulate a
method per screen.

## Decision

`IProductRepository` (Domain-defined, EF Core-backed) handles writes and
aggregate loading. Reads go through `IProductQueries`, backed by direct EF
Core queries (`.AsNoTracking()`), shaped for exactly what each query needs.

## Consequences

- No `IQueryable` leaking past Infrastructure, no repository bloated with
  per-screen query methods.
- Two read paths to keep straight conceptually: repository loads for
  commands that mutate an aggregate, direct queries for read-only results.
