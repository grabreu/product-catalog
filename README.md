# Product Catalog

[![CI](https://github.com/grabreu/product-catalog/actions/workflows/build.yml/badge.svg)](https://github.com/grabreu/product-catalog/actions/workflows/build.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=grabreu_product-catalog&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=grabreu_product-catalog)

Reference implementation used as an architectural baseline for the rest of
the portfolio - Clean Architecture, CQRS-lite, and testing practices,
without domain complexity or authentication obscuring the fundamentals.

## Status

V1 (clean architecture baseline) and V2 (integration tests + CI pipeline)
complete; V3 (observability) underway. Full CRUD for `Product`: create,
read (by id and paginated list, filterable by `isActive`), update, adjust
stock, deactivate/reactivate (soft delete, reversible). Covered by unit
tests (Domain/Application) and integration tests running through the real
HTTP pipeline against a disposable SQL Server. CI runs format, unit, and
integration checks plus a SonarCloud quality gate on every push/PR.

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

.NET 10 (LTS), ASP.NET Core Minimal API, EF Core, SQL Server (real
container locally, orchestrated by an Aspire AppHost). Observability:
OpenTelemetry + Serilog, viewable in the Aspire Dashboard locally. Tests:
xUnit, NSubstitute (unit), Testcontainers + Respawn (integration, real SQL
Server in Docker). CI: GitHub Actions + SonarCloud. Frontend (React) not
included in v1.
