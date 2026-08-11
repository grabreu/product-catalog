using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Domain.Products.Events;

public sealed record ProductCreatedEvent(Guid ProductId, DateTimeOffset OccurredAt) : IDomainEvent;
