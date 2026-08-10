# Product Catalog

Reference implementation used as an architectural baseline for the rest of
the portfolio - Clean Architecture, CQRS-lite, and testing practices,
without domain complexity or authentication obscuring the fundamentals.

## Status

Implementation in progress. Backend solution scaffolded (Domain,
Application, Infrastructure, Api, AppHost, ServiceDefaults, tests) - no
endpoints or domain logic yet.

## Documentation

- [`docs/product-definition.md`](docs/product-definition.md) - problem,
  motivation, scope, and competencies this project demonstrates
- [`docs/charter.md`](docs/charter.md) - technical decisions and repository
  structure
- [`docs/domain-model.md`](docs/domain-model.md) - domain model and class
  diagram
- [`docs/architecture.md`](docs/architecture.md) - layers, key architecture
  decisions, and request flow

## Stack

.NET 10 (LTS), ASP.NET Core Minimal API, .NET Aspire (orchestration/
observability), EF Core, SQL Server LocalDB (local dev, no container),
xUnit. Frontend (React) not included in v1.
