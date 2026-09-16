namespace ProductCatalog.Api.Features.Products.ReactivateProduct;

public record ReactivateProductCommand(Guid ProductId) : ICommand<Result<Unit>>;
