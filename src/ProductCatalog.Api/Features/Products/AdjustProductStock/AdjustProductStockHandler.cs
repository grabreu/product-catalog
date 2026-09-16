using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products.AdjustProductStock;

public class AdjustProductStockHandler(ApplicationDbContext dbContext) : ICommandHandler<AdjustProductStockCommand, Result<Unit>>
{
    public async ValueTask<Result<Unit>> Handle(AdjustProductStockCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Error.NotFound("Products.NotFound", $"No product was found with ID '{command.ProductId}'.");
        }

        product.AdjustStock(command.QuantityDelta);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
