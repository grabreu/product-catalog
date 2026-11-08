using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products;

public record DeactivateProductCommand(Guid ProductId) : ICommand<Result>;

public class DeactivateProductValidator : AbstractValidator<DeactivateProductCommand>
{
    public DeactivateProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}

public class DeactivateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<DeactivateProductCommand, Result>
{
    public async ValueTask<Result> Handle(DeactivateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Result.NotFound($"No product was found with ID '{command.ProductId}'.");
        }

        product.Deactivate();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public static class DeactivateProductEndpoint
{
    public static RouteHandlerBuilder MapDeactivateProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPatch("/products/{productId:guid}/deactivate", HandleAsync)
            .WithTags("Products")
            .WithName("DeactivateProduct")
            .WithSummary("Deactivate Product")
            .WithDescription("Deactivates a product (soft delete). It can be reactivated and stays readable by id. A concurrent change to the same product responds 409.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static ValueTask<IResult> HandleAsync(Guid productId, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new DeactivateProductCommand(productId), cancellationToken).ToNoContent();
    }
}
