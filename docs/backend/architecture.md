# Product Catalog — Architecture

## Layers and Dependency Rule

Dependencies point inward, toward the Domain layer.

```mermaid
flowchart LR
    Api --> Application
    Api --> Infrastructure
    Application --> Domain
    Infrastructure --> Domain
```

- **Domain** — `Product` aggregate, invariants, domain events, and the
  `IProductRepository`/`IUnitOfWork` interfaces Infrastructure implements.
  No external dependencies except `Mediator.INotification` (see ADR-0003).
- **Application** — commands, queries, handlers, validators. Organized by
  kind (`Commands/`, `Queries/`, `EventHandlers/`, `Behaviors/`,
  `Models/`), then by aggregate.
- **Infrastructure** — EF Core `DbContext`, repository implementations,
  SQL Server access.
- **Api** — Minimal API endpoints (`Endpoints/`), exception handling
  (`ExceptionHandling/`), `ErrorOr` → HTTP problem mapping.

Why these choices over the alternatives considered: see
[`docs/adr/`](../adr/).

## Domain Event Dispatch

Aggregates raise events into an in-memory list (`Product.DomainEvents`);
nothing is published until persistence actually succeeds. A `SaveChanges`
interceptor (`DispatchDomainEventsInterceptor`, Infrastructure) collects
pending events from tracked entities after `SavedChangesAsync`, clears them,
and publishes each one through Mediator - handlers (e.g.
`ProductCreatedEventHandler`) then react to it. Dispatching after the save
(not before) avoids publishing an event for a change that didn't actually
commit.

## Request Flow: Create Product

```mermaid
sequenceDiagram
    participant Client
    participant Api as Minimal API
    participant Mediator
    participant Validation as Validation Behavior
    participant Handler as CreateProductHandler
    participant Repo as ProductRepository
    participant DB as SQL Server

    Client->>Api: POST /products
    Api->>Mediator: Send(CreateProductCommand)
    Mediator->>Validation: Validate command
    alt invalid
        Validation-->>Api: ErrorOr<ValidationError>
        Api-->>Client: 400 Bad Request
    else valid
        Validation->>Handler: Handle(command)
        Handler->>Repo: Add(product)
        Repo->>DB: INSERT
        Handler-->>Mediator: ErrorOr<ProductDto>
        Mediator-->>Api: Result
        Api-->>Client: 201 Created (Location + full product body)
    end
```

## Testing Strategy

Unit tests (`ProductCatalog.UnitTests`) cover Domain and Application in
isolation - dependencies mocked with NSubstitute. Integration tests
(`ProductCatalog.IntegrationTests`) run through the real HTTP pipeline
(`WebApplicationFactory`) against a disposable SQL Server (Testcontainers),
reset between tests with Respawn instead of recreating the container each
time.

## CI/CD Pipeline

```mermaid
flowchart LR
    subgraph PR["ci.yml — pull request"]
        A1[Format] --> A3[SonarCloud]
        A2[Tests] --> A3
    end
    subgraph Main["cd.yml — push to main"]
        B1[Format] --> B3[SonarCloud]
        B2[Tests] --> B3
        B3 --> B4[Publish → GHCR]
        B4 --> B5[Deploy → Azure Container Apps]
    end
```

`ci.yml` and `cd.yml` run the same format/test/SonarCloud checks;
`cd.yml` adds publish and deploy, since only `main` is meant to ship. Both
skip entirely on changes limited to docs/README/LICENSE. `cd.yml` deploys
by resolving the image's immutable digest and updating the Container App
to it (ADR-0008) - no separate release/tagging step.
