using ProductCatalog.Api.Data;
using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products.CreateProduct;

public class CreateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<CreateProductCommand, Result<ProductDto>>
{
    public async ValueTask<Result<ProductDto>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        if (await dbContext.Products.AnyAsync(tl => tl.Sku == command.Sku, cancellationToken))
        {
            return Error.Conflict("Products.SkuAlreadyExists", $"A product with SKU '{command.Sku}' already exists.");
        }

        var product = new Product(command.Name, command.Sku, command.Price, command.Category);

        dbContext.Products.Add(product);

        await dbContext.SaveChangesAsync(cancellationToken);

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
