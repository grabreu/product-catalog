using ProductCatalog.Domain.Products;

namespace ProductCatalog.Api.Endpoints.Products;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    string Sku,
    string Description,
    decimal Price,
    ProductCategory Category,
    int StockQuantity,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
