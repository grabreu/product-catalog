# Product Catalog

[![CI](https://github.com/grabreu/product-catalog/actions/workflows/ci.yml/badge.svg)](https://github.com/grabreu/product-catalog/actions/workflows/ci.yml)
[![CD](https://github.com/grabreu/product-catalog/actions/workflows/cd.yml/badge.svg)](https://github.com/grabreu/product-catalog/actions/workflows/cd.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=grabreu_product-catalog&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=grabreu_product-catalog)

A deliberately minimal reference API - full CRUD for a single entity,
demonstrating Clean Architecture, CQRS-lite, and testing practices without
domain complexity or authentication obscuring the fundamentals.

## Status

Backend (V1-V4) complete: clean architecture baseline, integration tests +
CI pipeline, observability (OpenTelemetry/Serilog, health checks wired to
Azure Container Apps probes), and documentation discipline (ADRs). Full
CRUD for `Product`: create, read (by id and paginated list, filterable by
`isActive`), update, adjust stock, deactivate/reactivate (soft delete,
reversible). Covered by unit tests (Domain/Application) and integration
tests running through the real HTTP pipeline against a disposable SQL
Server. CI runs format, unit, and integration checks plus a SonarCloud
quality gate on every push/PR; CD publishes and deploys on every push to
`main`. V5 (frontend) planned - see
[`docs/frontend/charter.md`](docs/frontend/charter.md).

## Documentation

- [`docs/product-definition.md`](docs/product-definition.md) - problem,
  scope, and competencies this project demonstrates
- [`docs/backend/`](docs/backend/) - backend charter, architecture, and
  domain model
- [`docs/frontend/`](docs/frontend/) - frontend charter and architecture
- [`docs/adr/`](docs/adr/) - architecture decision records: the
  alternative considered and why it lost

## Stack

**Backend:** .NET 10, ASP.NET Core Minimal API, EF Core + SQL Server,
OpenTelemetry + Serilog. Hosted on Azure Container Apps + Azure SQL,
deployed via GitHub Actions on every push to `main`. Full facts:
[`docs/backend/charter.md`](docs/backend/charter.md).

**Frontend (planned):** Vite + React, TanStack Router/Query/Form,
shadcn/ui + Tailwind. Full facts:
[`docs/frontend/charter.md`](docs/frontend/charter.md).

The reasoning behind each choice, either side: [`docs/adr/`](docs/adr/).
