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
| Local database     | Real SQL Server in a container, orchestrated by the Aspire AppHost (`WithDataVolume`, persists between runs) - same engine as integration tests (Testcontainers) and prod (Azure SQL) |
| Orchestration      | .NET Aspire (`AppHost` + `ServiceDefaults`), scoped to local dev only. Tried and removed early on for adding overhead with no logic yet to observe; reintroduced once there was a real CRUD + test suite to justify it |
| Observability      | OpenTelemetry (traces/metrics) + Serilog with the `Serilog.Sinks.OpenTelemetry` sink, landing logs on the same OTLP pipeline. Health checks at `/health` (real SQL Server check) and `/alive` (liveness), wired to Azure Container Apps' native probes. Viewable via the Container Apps Environment's built-in Aspire Dashboard - Application Insights was evaluated and dropped as disproportionate surface area for this scale |
| Testing framework  | xUnit. Unit tests mock dependencies (NSubstitute); integration tests run through the real HTTP pipeline (`WebApplicationFactory`) against a disposable SQL Server (Testcontainers), reset between tests (Respawn) |
| CI/CD              | GitHub Actions - format check (`dotnet format`), unit tests, integration tests as parallel jobs, a SonarCloud static analysis + quality gate job, then a publish job (image built and pushed to GHCR, tagged by commit SHA) and a deploy job (updates the Azure Container App to the new image by digest) |
| Cloud hosting      | Azure Container Apps (Consumption plan) + Azure SQL (serverless, free tier). Provisioned by hand through the Azure Portal, not IaC - deliberate for a single always-on environment at this scale; revisit with Bicep/Terraform if a second environment or team ownership shows up. Deploy auth is OIDC (GitHub Actions federated credential, no stored Azure secret) |
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
├── .github/
│   ├── workflows/
│   │   ├── ci.yml               # PR validation: format, tests, sonarcloud
│   │   ├── cd.yml                # push to main: format, tests, sonarcloud, publish, deploy
│   │   └── _*.yml                 # reusable workflows called by ci.yml/cd.yml
│   └── dependabot.yml
├── backend/
│   ├── src/
│   │   ├── ProductCatalog.Domain/
│   │   ├── ProductCatalog.Application/
│   │   ├── ProductCatalog.Infrastructure/
│   │   ├── ProductCatalog.Api/
│   │   ├── ProductCatalog.AppHost/          # local dev orchestration only
│   │   └── ProductCatalog.ServiceDefaults/  # shared OTel/Serilog/health-check wiring
│   └── tests/
│       ├── ProductCatalog.UnitTests/
│       └── ProductCatalog.IntegrationTests/
├── frontend/
├── docs/
│   └── adr/            # planned for V4, empty for now
└── README.md
```

## Roadmap

See `product-definition.md`. Frontend architecture is intentionally
undecided here - it gets its own short charter + architecture pass
when that version starts, not guessed at now. See the portfolio-roadmap
repo's `project-definition-guide.md` ("Evolution Between Versions") for
the general rule.
