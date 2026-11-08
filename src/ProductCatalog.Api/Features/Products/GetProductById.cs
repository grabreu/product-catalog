using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products;

public record GetProductByIdQuery(Guid ProductId) : IQuery<Result<ProductDto>>;

public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}

public class GetProductByIdHandler(ApplicationDbContext dbContext) : IQueryHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async ValueTask<Result<ProductDto>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.Id == query.ProductId)
            .Select(x => new ProductDto(
                x.Id,
                x.Name,
                x.Sku,
                x.Description,
                x.Price,
                x.Category,
                x.StockQuantity,
                x.IsActive,
                x.CreatedAt,
                x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            return Result.NotFound($"No product was found with ID '{query.ProductId}'.");
        }

        return product;
    }
}

public static class GetProductByIdEndpoint
{
    public static RouteHandlerBuilder MapGetProductByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/products/{productId:guid}", HandleAsync)
            .WithTags("Products")
            .WithName("GetProductById")
            .WithSummary("Get Product By Id")
            .WithDescription("Retrieves a product by its id, active or not.")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static ValueTask<IResult> HandleAsync(Guid productId, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new GetProductByIdQuery(productId), cancellationToken).ToOk();
    }
}
