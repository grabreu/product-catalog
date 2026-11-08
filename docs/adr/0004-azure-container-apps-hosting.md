# Azure Container Apps hosting, OIDC federated credential

Hosted on Azure Container Apps (Consumption plan) with Azure SQL (serverless, free tier), inside the shared Container Apps environment `cae-grabreu-prod-brs`. GitHub Actions deploys through an OIDC federated credential on a user-assigned managed identity (`id-productcatalog-cd-prod-brs`), with subjects for the `main` branch and the `prod` environment, so no Azure credential is stored in GitHub. How the infrastructure is declared and how the image is published is in [ADR 0006](0006-project-infrastructure-in-bicep.md).

Custom domain (`product-catalog.grabreu.dev`) is bound with Container Apps' free managed certificate, with DNS on Cloudflare. The CNAME and the TXT (`asuid`) record must stay **DNS only** (unproxied): the proxy breaks both the managed certificate's domain validation and SNI-based routing in the shared environment.

**Consequences**: the environment is shared with other projects, so its settings and the DNS-only requirement apply to every app in it. The managed certificate is created through the CLI after the hostname is bound, and recreating the custom domain changes the certificate name.
