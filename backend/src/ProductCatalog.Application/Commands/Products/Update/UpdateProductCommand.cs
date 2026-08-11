using ProductCatalog.Application.Models.Products;
using ProductCatalog.Domain.Products;

namespace ProductCatalog.Application.Commands.Products.Update;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    ProductCategory Category) : ICommand<ErrorOr<ProductDto>>;
