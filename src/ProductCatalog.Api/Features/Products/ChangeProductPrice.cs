using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products;

public record ChangeProductPriceCommand(Guid ProductId, decimal NewPrice) : ICommand<Result>;

public class ChangeProductPriceValidator : AbstractValidator<ChangeProductPriceCommand>
{
    public ChangeProductPriceValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.NewPrice)
            .GreaterThan(0);
    }
}

public class ChangeProductPriceHandler(ApplicationDbContext dbContext) : ICommandHandler<ChangeProductPriceCommand, Result>
{
    public async ValueTask<Result> Handle(ChangeProductPriceCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Result.NotFound($"No product was found with ID '{command.ProductId}'.");
        }

        product.ChangePrice(command.NewPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public static class ChangeProductPriceEndpoint
{
    public static RouteHandlerBuilder MapChangeProductPriceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPatch("/products/{productId:guid}/price", HandleAsync)
            .WithTags("Products")
            .WithName("ChangeProductPrice")
            .WithSummary("Change Product Price")
            .WithDescription("Sets a new absolute price, which must be greater than zero. A concurrent change to the same product responds 409.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static ValueTask<IResult> HandleAsync(Guid productId, ChangeProductPriceRequest request, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new ChangeProductPriceCommand(productId, request.NewPrice), cancellationToken).ToNoContent();
    }

    public record ChangeProductPriceRequest(decimal NewPrice);
}
