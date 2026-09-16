using ProductCatalog.Api.Domain.Products.Events;

namespace ProductCatalog.Api.Features.Products.EventHandlers;

public partial class LogProductCreatedHandler(ILogger<LogProductCreatedHandler> logger) : INotificationHandler<ProductCreatedDomainEvent>
{
    public ValueTask Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        LogProductCreated(notification.ProductId);
        return ValueTask.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Product {ProductId} was created.")]
    private partial void LogProductCreated(Guid productId);
}
