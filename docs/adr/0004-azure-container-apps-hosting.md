# Azure Container Apps hosting, OIDC federated credential, no IaC

Hosted on Azure Container Apps (Consumption plan) + Azure SQL (serverless, free tier), provisioned by hand through the Azure Portal/CLI — no Terraform or Bicep, since this is a single always-on environment with no team beyond one developer. GitHub Actions deploys via an OIDC federated credential scoped to the prod environment, so no Azure credential is stored in GitHub.

**Consequences**: no IaC to maintain for one environment; would need revisiting if a second environment or shared ownership shows up.
