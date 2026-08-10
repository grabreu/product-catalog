using ProductCatalog.Domain.Common;

namespace ProductCatalog.Domain.Products;

public sealed record ProductCreatedEvent(Guid ProductId, DateTimeOffset OccurredAt) : IDomainEvent;
