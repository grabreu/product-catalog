using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products.UpdateProduct;

public class UpdateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<UpdateProductCommand, Result<Unit>>
{
    public async ValueTask<Result<Unit>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Error.NotFound("Products.NotFound", $"No product was found with ID '{command.ProductId}'.");
        }

        product.Update(command.Name, command.Description, command.Category);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
