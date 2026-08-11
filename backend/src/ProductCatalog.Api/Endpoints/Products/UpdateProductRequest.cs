using ProductCatalog.Domain.Products;

namespace ProductCatalog.Api.Endpoints.Products;

public sealed record UpdateProductRequest(string Name, string Description, ProductCategory Category);
