using ProductCatalog.Api.Data;
using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products;

public record UpdateProductCommand(Guid ProductId, string Name, string Description, ProductCategory Category) : ICommand<Result>;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotNull()
            .MaximumLength(2000);

        RuleFor(x => x.Category)
            .IsInEnum();
    }
}

public class UpdateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<UpdateProductCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync([command.ProductId], cancellationToken);

        if (product is null)
        {
            return Result.NotFound($"No product was found with ID '{command.ProductId}'.");
        }

        product.Update(command.Name, command.Description, command.Category);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public static class UpdateProductEndpoint
{
    public static RouteHandlerBuilder MapUpdateProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/products/{productId:guid}", HandleAsync)
            .WithTags("Products")
            .WithName("UpdateProduct")
            .WithSummary("Update Product")
            .WithDescription("Updates the name, description, and category of a product. Price and stock change through their own endpoints. A concurrent change to the same product responds 409.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static ValueTask<IResult> HandleAsync(Guid productId, UpdateProductRequest request, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new UpdateProductCommand(productId, request.Name, request.Description, request.Category), cancellationToken).ToNoContent();
    }

    public record UpdateProductRequest(string Name, string Description, ProductCategory Category);
}
