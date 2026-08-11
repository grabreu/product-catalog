using ProductCatalog.Application.Models.Products;
using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Application.Commands.Products.Update;

public sealed class UpdateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateProductCommand, ErrorOr<ProductDto>>
{
    public async ValueTask<ErrorOr<ProductDto>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
        {
            return Error.NotFound(description: $"No product was found with ID '{command.Id}'.");
        }

        product.Update(command.Name, command.Description, command.Category);

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
