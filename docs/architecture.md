# Architecture

## Domain Model

`Product` is the only aggregate root. Invariants: SKU is unique (enforced by a DB unique index), `Price > 0`, `StockQuantity >= 0`. Deactivation is a soft delete (`IsActive = false`), reversible via reactivation — products are never physically removed.

Concurrent writes: `Product` carries a SQL Server `rowversion` token, so a write based on a stale read fails and responds 409; the client retries. A duplicate SKU responds 409 as well, including when two creates race and the unique index rejects the second.

```mermaid
classDiagram
    class Product {
        +Guid Id
        +string Name
        +string Sku
        +string Description
        +decimal Price
        +ProductCategory Category
        +int StockQuantity
        +bool IsActive
        +DateTimeOffset CreatedAt
        +DateTimeOffset UpdatedAt
        +Update(name, description, category)
        +ChangePrice(decimal newPrice)
        +AdjustStock(int quantityDelta)
        +Deactivate()
        +Reactivate()
    }

    class ProductCategory {
        <<enumeration>>
        Electronics
        Apparel
        Home
        Other
    }

    class ProductCreatedDomainEvent {
        <<domain event>>
        +Guid ProductId
    }

    Product "1" --> "1" ProductCategory : has
    Product ..> ProductCreatedDomainEvent : raises
```

## Request Flow

`CreateProduct`, as a representative example — every command/query follows the same shape (Minimal API endpoint → `Mediator` → `ValidationBehavior` → handler):

```mermaid
sequenceDiagram
    participant Client
    participant Api as Minimal API
    participant Mediator
    participant Validation as ValidationBehavior
    participant Handler as CreateProductHandler
    participant DB as SQL Server

    Client->>Api: POST /products
    Api->>Mediator: Send(CreateProductCommand)
    Mediator->>Validation: validate command
    alt invalid
        Validation-->>Api: Result (validation errors)
        Api-->>Client: 400 Bad Request
    else valid
        Validation->>Handler: Handle(command)
        Handler->>DB: INSERT (via DbContext, no repository)
        DB-->>Handler: saved
        Handler-->>Mediator: Result<ProductDto>
        Mediator-->>Api: Result
        Api-->>Client: 201 Created (full product body)
    end
```

Domain events (e.g. `ProductCreatedDomainEvent`) are dispatched through the same `Mediator`, by a `SaveChanges` interceptor (`DispatchDomainEventsInterceptor`) — and only after the write actually commits, never before, so an event never fires for a change that didn't persist.
