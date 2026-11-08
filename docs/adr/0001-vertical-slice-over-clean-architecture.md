# Vertical Slice over Clean Architecture

This started as layered Clean Architecture (`Domain`, `Application`, `Infrastructure`, `Api`), which added interfaces and cross-layer mapping that didn't pay off for a handful of tightly related, single-entity use cases (product CRUD plus a few lifecycle actions). Switched to Vertical Slice: each use case (e.g. `Features/Products/CreateProduct.cs`) is one file that owns its own command/query, validator, handler and endpoint, with CQRS as the internal split between writes and reads.

**Consequences**: less immediately recognizable to engineers coming from a Clean Architecture background, in exchange for no indirection to navigate for a domain this size.
