namespace ProductCatalog.Api.Features.Products.DeactivateProduct;

public record DeactivateProductCommand(Guid ProductId) : ICommand<Result<Unit>>;
