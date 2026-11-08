using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products;

public record ReactivateProductCommand(Guid ProductId) : ICommand<Result>;

public class ReactivateProductValidator : AbstractValidator<ReactivateProductCommand>
{
    public ReactivateProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}

public class ReactivateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<ReactivateProductCommand, Result>
{
    public async ValueTask<Result> Handle(ReactivateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Result.NotFound($"No product was found with ID '{command.ProductId}'.");
        }

        product.Reactivate();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public static class ReactivateProductEndpoint
{
    public static RouteHandlerBuilder MapReactivateProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPatch("/products/{productId:guid}/reactivate", HandleAsync)
            .WithTags("Products")
            .WithName("ReactivateProduct")
            .WithSummary("Reactivate Product")
            .WithDescription("Reactivates a deactivated product. A concurrent change to the same product responds 409.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static ValueTask<IResult> HandleAsync(Guid productId, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new ReactivateProductCommand(productId), cancellationToken).ToNoContent();
    }
}
