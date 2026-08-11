using ProductCatalog.Application.Models.Products;
using ProductCatalog.Domain.Products;

namespace ProductCatalog.Application.Commands.Products.Create;

public sealed record CreateProductCommand(
    string Name,
    string Sku,
    decimal Price,
    ProductCategory Category) : ICommand<ErrorOr<ProductDto>>;
