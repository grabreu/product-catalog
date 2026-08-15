# ADR-0004: ErrorOr for anticipated failures, exceptions for invariant violations

## Status

Accepted

## Context

Handlers need to return validation/conflict errors a caller can reasonably
trigger (e.g. duplicate SKU) without exception overhead. Domain invariants
(e.g. price <= 0) should never be reachable by the time a handler calls
the aggregate, since upstream validation already checked them.

## Decision

`ErrorOr<T>` for Application-level errors, mapped to HTTP problem details.
`DomainException` plus a global `IExceptionHandler` for invariant
violations.

## Consequences

- Two error-handling paths to maintain, not one.
- A thrown `DomainException` at runtime signals a bug in upstream
  validation, not a normal branch - it's a guard, not expected traffic.
