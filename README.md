# product-catalog

[![CI](https://github.com/grabreu/product-catalog/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/grabreu/product-catalog/actions/workflows/ci.yml)
[![CD](https://github.com/grabreu/product-catalog/actions/workflows/cd.yml/badge.svg?branch=main)](https://github.com/grabreu/product-catalog/actions/workflows/cd.yml)
[![License](https://img.shields.io/github/license/grabreu/product-catalog?style=flat-square)](LICENSE)

A backend-only reference API for a product catalog: minimal writes, rich read/query surface.

_The domain is deliberately flat — two validation rules, no state machine — so the focus stays on the architecture, testing, and deployment rigor around it, not domain complexity._

**[Try it live →](https://product-catalog.grabreu.dev)**

## Tech stack

.NET 10 Minimal APIs · Vertical Slice + CQRS · EF Core + SQL Server · Mediator + FluentValidation + Desfecho · OpenTelemetry · .NET Aspire for local orchestration

## Features

- **Full product lifecycle** — create, update, deactivate and reactivate; deactivation is a reversible soft delete, not a hard delete.
- **Stock and price as dedicated actions** — stock changes apply a delta, price changes set a new absolute value, each through its own endpoint instead of a generic update.
- **Paged listing** — filterable by active status.
- **Get by id**

See [docs/architecture.md](docs/architecture.md) for the domain model and request flow, and [docs/adr/](docs/adr/) for the reasoning behind these decisions.

## Development

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download) and a container runtime (Docker), since the AppHost starts a SQL Server container.

```bash
dotnet restore
dotnet run --project src/ProductCatalog.AppHost
```

The AppHost starts SQL Server and the API. Migrations and seed data run automatically on startup, and the API reference (Scalar) is served at `/scalar`.

Other commands: `dotnet format --verify-no-changes` (formatting check, matches CI).

## Deployment

Auto-deployed to Azure Container Apps (Consumption plan) + Azure SQL (serverless, free tier) on every merge to `main` via GitHub Actions, authenticated with an OIDC federated credential scoped to the prod environment — no Azure credential stored in GitHub. The project's infrastructure is defined in Bicep (`infra/`); shared resources come from `azure-infra`. Custom domain DNS managed via Cloudflare.

## License

Licensed under the [MIT License](LICENSE).
