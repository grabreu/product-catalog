# Azure Container Apps hosting, OIDC federated credential, no IaC

Superseded in part by [ADR 0006](0006-project-infrastructure-in-bicep.md): the infrastructure is now declared in Bicep and images go to the shared Azure Container Registry.

Hosted on Azure Container Apps (Consumption plan) + Azure SQL (serverless, free tier), provisioned by hand through the Azure Portal/CLI — no Terraform or Bicep, since this is a single always-on environment with no team beyond one developer. GitHub Actions deploys via an OIDC federated credential scoped to the prod environment, so no Azure credential is stored in GitHub.

**Consequences**: no IaC to maintain for one environment; would need revisiting if a second environment or shared ownership shows up.

Custom domain (`product-catalog.grabreu.dev`) bound via ACA's free managed certificate, DNS hosted on Cloudflare. The CNAME and TXT (`asuid`) records must stay **DNS only** (unproxied) — Cloudflare's proxy breaks both the managed certificate's domain validation and SNI-based routing in the shared `cae-shared-prod-brs` environment.
