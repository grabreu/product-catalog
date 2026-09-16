# Product Catalog

[![CI](https://github.com/grabreu/product-catalog/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/grabreu/product-catalog/actions/workflows/ci.yml)
[![CD](https://github.com/grabreu/product-catalog/actions/workflows/cd.yml/badge.svg?branch=main)](https://github.com/grabreu/product-catalog/actions/workflows/cd.yml)
[![License](https://img.shields.io/github/license/grabreu/product-catalog?style=flat-square)](LICENSE)

A backend-only reference API for a product catalog: minimal writes, rich read/query surface.

_The domain is deliberately flat — two validation rules, no state machine — so the focus stays on the architecture, testing, and deployment rigor around it, not domain complexity._

## Tech stack

.NET 10 Minimal APIs · Vertical Slice + CQRS · EF Core + SQL Server · Mediator + FluentValidation + Desfecho · Serilog

## Features

- **Full product lifecycle** — create, update, deactivate and reactivate; deactivation is a reversible soft delete, not a hard delete.
- **Stock and price as dedicated actions** — stock changes apply a delta, price changes set a new absolute value, each through its own endpoint instead of a generic update.
- **Paged listing** — filterable by active status.
- **Get by id**

See [docs/architecture.md](docs/architecture.md) for the domain model and request flow, and [docs/adr/](docs/adr/) for the reasoning behind these decisions.

## Development

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download) and SQL Server LocalDB (or point `ConnectionStrings:DefaultConnection` in `appsettings.Development.json` at another SQL Server instance).

```bash
dotnet restore
dotnet run --project src/ProductCatalog.Api
```

Migrations and seed data run automatically on startup in Development. The app opens to `/scalar` for the OpenAPI explorer.

Other commands: `dotnet format --verify-no-changes` (formatting check, matches CI).

## Deployment

Deployed to Azure Container Apps (Consumption plan) + Azure SQL (serverless, free tier) via GitHub Actions, authenticated with an OIDC federated credential scoped to the prod environment — no Azure credential stored in GitHub, no Terraform or Bicep.

## License

Licensed under the [MIT License](LICENSE).
