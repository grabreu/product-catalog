# ADR-0009: Cloud hosting - Azure Container Apps, manual provisioning, OIDC deploy

## Status

Accepted

## Context

Needed somewhere to run the published container with the least
infrastructure to own, for a single always-on environment with no team
beyond one developer.

## Decision

Azure Container Apps (Consumption plan) + Azure SQL (serverless, free
tier), provisioned by hand through the Azure Portal - no Bicep or
Terraform. GitHub Actions builds and pushes the image to GHCR, then
deploys to the Container App by immutable digest (`image@sha256:...`),
authenticated via OIDC: a federated credential scoped to the `main`
branch, with a `Container Apps Contributor` role on one resource group -
no Azure credential stored in GitHub.

## Consequences

- No IaC to maintain for one environment. Revisit with Bicep/Terraform if
  a second environment or team ownership shows up.
- Digest-pinned deploys guarantee the exact image CI built is what's
  running - a mutable tag can't be silently overwritten later.
- The Azure Portal's manual container-edit form only validates
  `name:tag` syntax and rejects `@sha256:...` - a hand edit through the
  Portal needs a temporary tag swap to get past that form's validation.
  The automated deploy job (`az containerapp update`) has no such
  restriction.
