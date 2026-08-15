# ADR-0003: Mediator for dispatch, FluentValidation as a pipeline behavior

## Status

Accepted

## Context

Needed a way to demonstrate pipeline behaviors (cross-cutting concerns
like validation) without coupling handlers to them. MediatR, the common
default, moved to a dual commercial license.

## Decision

`Mediator` (source-generator based, fully free) dispatches commands and
queries. FluentValidation runs as a Mediator pipeline behavior ahead of
handlers, not inside them.

## Consequences

- No commercial licensing exposure.
- Validation logic stays out of handlers.
- `Mediator`'s source generator needs a closed, concrete message type per
  handler - this is why `IDomainEvent` inherits `Mediator.INotification`
  directly instead of going through a generic bridge type, unlike what a
  reflection-based mediator would allow.
