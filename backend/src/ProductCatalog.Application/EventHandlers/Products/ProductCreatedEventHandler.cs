using ProductCatalog.Domain.Products.Events;

namespace ProductCatalog.Application.EventHandlers.Products;

public sealed partial class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    public ValueTask Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        LogProductCreated(notification.ProductId, notification.OccurredAt);

        return ValueTask.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Product {ProductId} created at {OccurredAt}")]
    private partial void LogProductCreated(Guid productId, DateTimeOffset occurredAt);
}
