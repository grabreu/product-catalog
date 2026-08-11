using ProductCatalog.Application.Models.Products;

namespace ProductCatalog.Application.Commands.Products.AdjustStock;

public sealed record AdjustStockCommand(Guid Id, int QuantityDelta) : ICommand<ErrorOr<ProductDto>>;
