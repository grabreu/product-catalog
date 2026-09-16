using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid ProductId,
    string Name,
    string Description,
    ProductCategory Category) : ICommand<Result<Unit>>;
