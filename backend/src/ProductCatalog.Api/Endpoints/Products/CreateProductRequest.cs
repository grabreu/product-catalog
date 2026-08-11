using ProductCatalog.Domain.Products;

namespace ProductCatalog.Api.Endpoints.Products;

public sealed record CreateProductRequest(string Name, string Sku, decimal Price, ProductCategory Category);
