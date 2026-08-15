# ADR-0007: Aspire for local dev orchestration only

## Status

Accepted

## Context

Local dev needs a fast way to run a real SQL Server instance, so dev,
integration tests, and prod all run against the same database engine
instead of a lighter substitute that could hide provider-specific bugs.

## Decision

.NET Aspire (`AppHost` + `ServiceDefaults`) orchestrates local dev only -
a real SQL Server container with a persisted data volume. Not used for
cloud provisioning. Removed early in the project for adding overhead with
no real logic yet to observe; reintroduced once there was an actual CRUD
flow and test suite to justify it.

## Consequences

- Dev, integration tests (Testcontainers), and prod (Azure SQL) share one
  engine - no SQLite-vs-SQL-Server drift to debug.
- Local dev config (Aspire wiring) and cloud config (plain env vars on the
  Container App) diverge somewhat, since Aspire isn't driving cloud
  provisioning - `azd`'s Bicep auto-generation was considered and rejected
  as more ceremony than a single-environment app needs (see ADR-0009).
