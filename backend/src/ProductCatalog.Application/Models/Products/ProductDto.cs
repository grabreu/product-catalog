using ProductCatalog.Domain.Products;

namespace ProductCatalog.Application.Models.Products;

public sealed record ProductDto(
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
