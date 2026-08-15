# ADR-0004: Minimal API over a dedicated framework

## Status

Accepted

## Context

ASP.NET Core Minimal API is the native mechanism for exposing HTTP
endpoints; FastEndpoints is used elsewhere in the portfolio.

## Decision

Minimal API for this project's endpoints.

## Consequences

- No extra framework dependency here.
- The portfolio demonstrates both approaches instead of standardizing on
  one - deliberate stack variety, at the cost of some inconsistency across
  repos for anyone comparing them side by side.
