using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products;

public record ProductDto(
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
