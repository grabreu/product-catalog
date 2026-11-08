using ProductCatalog.Api.Data;
using ProductCatalog.Api.Domain.Products;

namespace ProductCatalog.Api.Features.Products;

public record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    string Description,
    decimal Price,
    ProductCategory Category,
    int StockQuantity,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public record CreateProductCommand(string Name, string Sku, decimal Price, ProductCategory Category) : ICommand<Result<ProductDto>>;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Sku)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Category)
            .IsInEnum();
    }
}

public class CreateProductHandler(ApplicationDbContext dbContext) : ICommandHandler<CreateProductCommand, Result<ProductDto>>
{
    public async ValueTask<Result<ProductDto>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        if (await dbContext.Products.AnyAsync(x => x.Sku == command.Sku, cancellationToken))
        {
            return Result.Conflict($"A product with SKU '{command.Sku}' already exists.");
        }

        var product = new Product(command.Name, command.Sku, command.Price, command.Category);

        dbContext.Products.Add(product);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception) when (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            return Result.Conflict($"A product with SKU '{command.Sku}' already exists.");
        }

        return new ProductDto(
            product.Id,
            product.Name,
            product.Sku,
            product.Description,
            product.Price,
            product.Category,
            product.StockQuantity,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);
    }
}

public static class CreateProductEndpoint
{
    public static RouteHandlerBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/products", HandleAsync)
            .WithTags("Products")
            .WithName("CreateProduct")
            .WithSummary("Create Product")
            .WithDescription("Creates an active product with no stock. A SKU that already exists responds 409.")
            .Produces<ProductDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static ValueTask<IResult> HandleAsync(CreateProductRequest request, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new CreateProductCommand(request.Name, request.Sku, request.Price, request.Category), cancellationToken).ToCreated(product => $"/products/{product.Id}");
    }

    public record CreateProductRequest(string Name, string Sku, decimal Price, ProductCategory Category);
}
