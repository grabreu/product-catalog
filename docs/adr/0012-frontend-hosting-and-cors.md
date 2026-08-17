# ADR-0012: Azure Static Web Apps hosting, CORS on the API

## Status

Accepted

## Context

The frontend needs to be reachable publicly. The API is already hosted
on Azure Container Apps (ADR-0008); needed the smallest amount of new
infrastructure to own, consistent with the rest of the stack.

## Decision

Azure Static Web Apps hosts the built SPA, deployed via GitHub Actions on
push to `main` (path-filtered to `frontend/**`). The API's CORS policy is
opened to the Static Web App's origin - frontend and API stay on separate
origins rather than being combined into one deployable.

## Consequences

- One more Azure resource to provision by hand (ADR-0008's no-IaC
  reasoning applies here too).
- CORS policy is another surface to keep in sync if the frontend's domain
  changes later (e.g. a custom domain).
