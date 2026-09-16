using ProductCatalog.Api.Domain.Products.Events;

namespace ProductCatalog.Api.Features.Products.EventHandlers;

public class LogProductCreatedHandler(ILogger<LogProductCreatedHandler> logger) : INotificationHandler<ProductCreatedDomainEvent>
{
    public ValueTask Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Product {ProductId} was created.", notification.ProductId);
        return ValueTask.CompletedTask;
    }
}
