using ProductCatalog.Application.Models.Products;
using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Application.Commands.Products.Create;

public sealed class CreateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<CreateProductCommand, ErrorOr<ProductDto>>
{
    public async ValueTask<ErrorOr<ProductDto>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        if (await repository.SkuExistsAsync(command.Sku, cancellationToken))
        {
            return Error.Conflict(description: $"A product with SKU '{command.Sku}' already exists.");
        }

        var product = Product.Create(command.Name, command.Sku, command.Price, command.Category);

        await repository.AddAsync(product, cancellationToken);

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
