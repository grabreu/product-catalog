using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products.ChangeProductPrice;

public class ChangeProductPriceHandler(ApplicationDbContext dbContext) : ICommandHandler<ChangeProductPriceCommand, Result<Unit>>
{
    public async ValueTask<Result<Unit>> Handle(ChangeProductPriceCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Error.NotFound("Products.NotFound", $"No product was found with ID '{command.ProductId}'.");
        }

        product.ChangePrice(command.NewPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
