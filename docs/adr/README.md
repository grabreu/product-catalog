# Architecture Decision Records

Short records of decisions that had a real alternative worth naming and a
reason one was picked over the other. Absence of a feature (no auth, no
multi-tenancy) isn't a decision to record here - see `product-definition.md`
for scope and non-goals.

- [0001](0001-clean-architecture-over-vertical-slice.md) - Clean Architecture over Vertical Slice
- [0002](0002-repository-for-writes-direct-queries-for-reads.md) - Repository for writes, direct queries for reads
- [0003](0003-mediator-and-fluentvalidation-pipeline.md) - Mediator for dispatch, FluentValidation as a pipeline behavior
- [0004](0004-minimal-api-over-a-framework.md) - Minimal API over a dedicated framework
- [0005](0005-erroror-and-domain-exceptions.md) - ErrorOr for anticipated failures, exceptions for invariant violations
- [0006](0006-rest-verb-choices.md) - REST verb choices for lifecycle actions and partial updates
- [0007](0007-aspire-for-local-dev-only.md) - Aspire for local dev orchestration only
- [0008](0008-observability-stack.md) - Observability: OpenTelemetry, Serilog, health checks
- [0009](0009-azure-container-apps-hosting.md) - Cloud hosting: Azure Container Apps, manual provisioning, OIDC deploy
