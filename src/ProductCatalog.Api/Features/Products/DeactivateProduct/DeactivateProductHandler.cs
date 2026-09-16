using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products.DeactivateProduct;

public class DeactivateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<DeactivateProductCommand, Result<Unit>>
{
    public async ValueTask<Result<Unit>> Handle(DeactivateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Error.NotFound("Products.NotFound", $"No product was found with ID '{command.ProductId}'.");
        }

        product.Deactivate();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
