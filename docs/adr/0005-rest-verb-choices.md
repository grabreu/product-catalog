# ADR-0005: REST verb choices for lifecycle actions and partial updates

## Status

Accepted

## Context

Product deactivation is a reversible soft delete. Stock and price changes
are client-supplied values applied to an existing resource. A
`201 Created` response ideally lets a client render the new resource
without a follow-up `GET`.

## Decision

- `POST /products/{id}/deactivate` and `.../reactivate`, mirroring
  Gmail's trash/untrash (`POST`, no body) - `DELETE` would imply
  permanent removal, which this domain doesn't have.
- `PATCH /products/{id}/stock` and `.../price`, each carrying the value to
  apply (`quantityDelta`, `newPrice`) - exactly what `PATCH` is for (RFC
  5789).
- `201` responses return the full `ProductDto` - RFC 9110 §10.2.2: a 201
  "typically describes and links to the resource(s) created."

## Consequences

More endpoints than a single generic `PATCH`/`DELETE` would need, in
exchange for an API where the verb and route describe the intent.
