namespace ProductCatalog.Api.Features.Products.ChangeProductPrice;

public record ChangeProductPriceCommand(Guid ProductId, decimal NewPrice) : ICommand<Result<Unit>>;
