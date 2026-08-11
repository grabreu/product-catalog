using ProductCatalog.Domain.Products;

namespace ProductCatalog.Application.Commands.Products.Create;

public sealed record CreateProductResult(
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
