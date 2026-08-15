# Product Catalog — Charter

## Overview

A deliberately minimal reference API: full CRUD for a single entity, built
to demonstrate clean structure, testing discipline, and CI/CD practice
without domain complexity or authentication obscuring the fundamentals.

## Scope

CRUD for a single entity, `Product` - create, read, update, soft-delete,
list with pagination. Full MVP scope and non-goals: see
[`product-definition.md`](product-definition.md).

## Facts

| Area          | Choice                                                                                         |
| -------------- | ------------------------------------------------------------------------------------------------ |
| Runtime        | .NET 10 (LTS, supported through Nov 2028)                                                        |
| Architecture   | Clean Architecture - `Domain`, `Application`, `Infrastructure`, `Api` (see `architecture.md`)   |
| Persistence    | SQL Server + EF Core                                                                              |
| API docs       | Scalar                                                                                             |
| Testing        | xUnit, NSubstitute (unit); Testcontainers + Respawn (integration, real SQL Server in Docker)     |
| CI/CD          | GitHub Actions - format, unit/integration tests, SonarCloud, then publish (GHCR) and deploy (Azure Container Apps) on push to `main` |
| Cloud hosting  | Azure Container Apps + Azure SQL (serverless, free tier)                                          |

See [`docs/adr/`](adr/) for the reasoning behind these choices.

## Repository Structure

```text
product-catalog/
├── .github/
│   ├── workflows/
│   │   ├── ci.yml                 # PR validation: format, tests, sonarcloud
│   │   ├── cd.yml                 # push to main: format, tests, sonarcloud, publish, deploy
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
│   ├── adr/                # architecture decision records
│   ├── architecture.md
│   ├── charter.md
│   ├── domain-model.md
│   └── product-definition.md
└── README.md
```
