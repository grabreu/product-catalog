using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products.CreateProduct;

public record CreateProductCommand(string Name, string Sku, decimal Price, ProductCategory Category) : ICommand<Result<ProductDto>>;
