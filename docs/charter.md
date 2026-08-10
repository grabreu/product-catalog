# Product Catalog — Charter

## Overview

Reference implementation used as an architectural baseline for the rest of
the portfolio. Intentionally minimal business domain: value comes from
demonstrating clean structure, testing discipline, and CI practice without
domain complexity or authentication obscuring the fundamentals.

## Scope

CRUD for a single entity, `Product` - create, read, update, soft-delete,
list with pagination.

**Non-goals:** authentication, multi-tenancy, approval flows, external
integrations, elaborate UI.

## Technical Decisions

| Area               | Decision                                                                                                                            |
| ------------------ | ----------------------------------------------------------------------------------------------------------------------------------- |
| Authentication     | None - single implicit user, no accounts                                                                                            |
| Authorization      | None                                                                                                                                |
| Modules            | Single module                                                                                                                       |
| Architecture style | Clean Architecture (Domain, Application, Infrastructure, Api)                                                                       |
| Runtime            | .NET 10 (LTS, supported through Nov 2028)                                                                                           |
| Persistence        | SQL Server + EF Core                                                                                                                |
| Local database     | SQL Server LocalDB (bundled with Visual Studio), referenced via .NET Aspire AppHost (`AddConnectionString`) - no container          |
| Orchestration      | .NET Aspire (AppHost + ServiceDefaults) - service discovery, dashboard, OpenTelemetry out of the box                                |
| Testing framework  | xUnit                                                                                                                               |
| Frontend           | Not included in v1. Repository structured with a `frontend/` placeholder so a React client can be added later without restructuring |

**Clean Architecture over Vertical Slice:** chosen because this project
serves as the common architectural baseline referenced by the rest of the
portfolio. Clean Architecture is the more universal, more widely
recognizable pattern. Vertical Slice is reserved for a project with many
loosely related use cases where it fits better (e.g. the Change/Approval
Workflow project).

## Repository Structure

```text
product-catalog/
├── backend/
│   ├── src/
│   │   ├── ProductCatalog.Domain/
│   │   ├── ProductCatalog.Application/
│   │   ├── ProductCatalog.Infrastructure/
│   │   ├── ProductCatalog.Api/
│   │   ├── ProductCatalog.AppHost/
│   │   └── ProductCatalog.ServiceDefaults/
│   └── tests/
│       ├── ProductCatalog.UnitTests/
│       └── ProductCatalog.IntegrationTests/
├── frontend/
├── docs/
│   └── adr/
└── README.md
```

## Roadmap

See `product-definition.md`. Frontend architecture is intentionally
undecided here - it gets its own short charter + architecture pass
when that version starts, not guessed at now. See the portfolio-roadmap
repo's `project-definition-guide.md` ("Evolution Between Versions") for
the general rule.
