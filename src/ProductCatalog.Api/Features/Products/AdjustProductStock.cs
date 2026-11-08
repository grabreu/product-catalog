using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products;

public record AdjustProductStockCommand(Guid ProductId, int QuantityDelta) : ICommand<Result>;

public class AdjustProductStockValidator : AbstractValidator<AdjustProductStockCommand>
{
    public AdjustProductStockValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}

public class AdjustProductStockHandler(ApplicationDbContext dbContext) : ICommandHandler<AdjustProductStockCommand, Result>
{
    public async ValueTask<Result> Handle(AdjustProductStockCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Result.NotFound($"No product was found with ID '{command.ProductId}'.");
        }

        product.AdjustStock(command.QuantityDelta);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public static class AdjustProductStockEndpoint
{
    public static RouteHandlerBuilder MapAdjustProductStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPatch("/products/{productId:guid}/stock", HandleAsync)
            .WithTags("Products")
            .WithName("AdjustProductStock")
            .WithSummary("Adjust Product Stock")
            .WithDescription("Applies a delta to the stock quantity, which cannot become negative. A concurrent change to the same product responds 409.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static ValueTask<IResult> HandleAsync(Guid productId, AdjustProductStockRequest request, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new AdjustProductStockCommand(productId, request.QuantityDelta), cancellationToken).ToNoContent();
    }

    public record AdjustProductStockRequest(int QuantityDelta);
}
