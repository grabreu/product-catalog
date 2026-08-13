using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Commands.Products.ChangePrice;

public sealed record ChangePriceCommand(Guid Id, decimal NewPrice) : ICommand<ErrorOr<ProductDto>>;
