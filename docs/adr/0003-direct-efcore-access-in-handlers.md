# Direct EF Core access in handlers, no repository or query abstraction

`ApplicationDbContext` is used directly inside command and query handlers — no `IProductRepository`, no separate query objects. EF Core's `DbSet<T>` already is the repository/unit-of-work pattern; wrapping it behind another interface would only abstract EF Core to swap it for another EF Core, and this domain has no query shape complex enough to justify one (same reasoning as jasontaylordev's Clean Architecture [ADR-001](https://github.com/jasontaylordev/CleanArchitecture/blob/main/docs/decisions/ADR-001-Use-EFCore-In-Application-Layer.md)).

**Consequences**: handlers are coupled to EF Core directly, which is acceptable since there's no plan to swap ORMs — and Vertical Slice already means each handler owns its own persistence code instead of sharing a generic repository.
