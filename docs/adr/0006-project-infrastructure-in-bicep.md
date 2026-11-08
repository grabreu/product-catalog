# Project infrastructure in Bicep, shared resources referenced as existing

The project's own resources are declared in Bicep under `infra/`: the Container App (`ca-productcatalog-api-prod-brs`, system-assigned identity, registry access through that identity) and the database `sqldb-productcatalog` (serverless, free tier) on the shared SQL Server. The resource group, Container Apps environment, container registry, SQL Server, and Log Analytics workspace are shared across projects and live in the `azure-infra` repository; here they are only referenced as `existing`. The Container App reaches SQL with its managed identity (no password), and pulls its image from the shared registry `crgrabreuprod`; CD pushes there instead of GitHub Container Registry.

Deployment stays manual: `az deployment group create` with the `image` parameter, which is required on every run so a redeploy does not revert the image CD published. The custom domain (`product-catalog.grabreu.dev`) needs its managed certificate, which can only be created through the CLI after the hostname is bound; its name goes in the `customDomainCertificateName` parameter, and a deploy with it empty removes the custom domain.

This supersedes the "no IaC" and GitHub Container Registry parts of [ADR 0004](0004-azure-container-apps-hosting.md).

**Consequences**: a Bicep deployment is a manual step whenever configuration or infrastructure changes. First-time setup (CD identity, registry roles, SQL grants for the app identity) follows `docs/bootstrap-project-identity.md` in `azure-infra`, with `productcatalog` as the project name. TODO: the infrastructure has not been deployed from this template yet.
