namespace ProductCatalog.Api.Features.Products.AdjustProductStock;

public record AdjustProductStockCommand(Guid ProductId, int QuantityDelta) : ICommand<Result<Unit>>;
