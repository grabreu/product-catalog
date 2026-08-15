# ADR-0008: Observability - OpenTelemetry, Serilog, health checks

## Status

Accepted

## Context

Needed traces, metrics, and logs in one place without maintaining a
second logging pipeline, plus a way for Azure Container Apps to detect an
unhealthy replica rather than just an open TCP port.

## Decision

OpenTelemetry for traces and metrics. Serilog stays the logging pipeline
and forwards to the same OTLP endpoint (`Serilog.Sinks.OpenTelemetry`),
viewable through the Container Apps Environment's built-in Aspire
Dashboard. `/health` runs a real SQL Server check, wired to Container
Apps' readiness probe; `/alive` is a dependency-free liveness check. Both
paths are excluded from tracing and demoted to `Verbose` in request logs,
since Container Apps polls them every few seconds.

## Consequences

- One logging pipeline, not two.
- Container Apps can actually act on health (restart a dead replica, stop
  routing to one that can't reach the database), not just check the
  process is listening.
- Application Insights was evaluated and dropped - its surface area
  (Smart Detection, Workbooks, Cohorts, ...) is disproportionate for a
  single-developer project at this scale. Revisit if this needs longer
  retention, alerting, or a team beyond one person.
