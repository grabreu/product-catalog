using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products.ReactivateProduct;

public class ReactivateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<ReactivateProductCommand, Result<Unit>>
{
    public async ValueTask<Result<Unit>> Handle(ReactivateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Error.NotFound("Products.NotFound", $"No product was found with ID '{command.ProductId}'.");
        }

        product.Reactivate();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
