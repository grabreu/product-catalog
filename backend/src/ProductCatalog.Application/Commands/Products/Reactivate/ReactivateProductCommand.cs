using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Commands.Products.Reactivate;

public sealed record ReactivateProductCommand(Guid Id) : ICommand<ErrorOr<ProductDto>>;
