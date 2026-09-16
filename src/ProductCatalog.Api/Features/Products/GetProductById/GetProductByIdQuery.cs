namespace ProductCatalog.Api.Features.Products.GetProductById;

public record GetProductByIdQuery(Guid ProductId) : IQuery<Result<ProductDto>>;
