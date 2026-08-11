using ProductCatalog.Domain.Products.Events;

namespace ProductCatalog.Application.EventHandlers.Products;

public sealed class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    public ValueTask Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Product {ProductId} created at {OccurredAt}",
            notification.ProductId,
            notification.OccurredAt);

        return ValueTask.CompletedTask;
    }
}
