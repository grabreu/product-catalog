using ProductCatalog.Api.Domain.SeedWork;

namespace ProductCatalog.Api.Domain.Products.Events;

public record ProductCreatedDomainEvent(Guid ProductId) : IDomainEvent;
