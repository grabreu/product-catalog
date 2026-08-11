using ProductCatalog.Application.Models.Products;
using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Application.Commands.Products.AdjustStock;

public sealed class AdjustStockCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<AdjustStockCommand, ErrorOr<ProductDto>>
{
    public async ValueTask<ErrorOr<ProductDto>> Handle(AdjustStockCommand command, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
        {
            return Error.NotFound(description: $"No product was found with ID '{command.Id}'.");
        }

        product.AdjustStock(command.QuantityDelta);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new ProductDto(
            product.Id,
            product.Name,
            product.Sku,
            product.Description,
            product.Price,
            product.Category,
            product.StockQuantity,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);
    }
}
