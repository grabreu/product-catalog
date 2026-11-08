using ProductCatalog.Api.Common.Models;
using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.Features.Products;

public record ListProductsQuery(int Page, int PageSize, bool? IsActive) : IQuery<Result<PagedResult<ProductDto>>>;

public class ListProductsValidator : AbstractValidator<ListProductsQuery>
{
    public ListProductsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}

public class ListProductsHandler(ApplicationDbContext dbContext) : IQueryHandler<ListProductsQuery, Result<PagedResult<ProductDto>>>
{
    public async ValueTask<Result<PagedResult<ProductDto>>> Handle(ListProductsQuery query, CancellationToken cancellationToken)
    {
        var products = dbContext.Products.AsNoTracking();

        if (query.IsActive.HasValue)
        {
            products = products.Where(x => x.IsActive == query.IsActive.Value);
        }

        var totalCount = await products.CountAsync(cancellationToken);

        var items = await products
            .OrderBy(x => x.CreatedAt)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
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
            .ToListAsync(cancellationToken);

        return new PagedResult<ProductDto>(items, query.Page, query.PageSize, totalCount);
    }
}

public static class ListProductsEndpoint
{
    public static RouteHandlerBuilder MapListProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/products", HandleAsync)
            .WithTags("Products")
            .WithName("ListProducts")
            .WithSummary("List Products")
            .WithDescription("Retrieves products oldest first, in pages of up to 100 (page 1 with 20 items by default), optionally filtered by active status.")
            .Produces<PagedResult<ProductDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
    }

    private static ValueTask<IResult> HandleAsync([AsParameters] ListProductsRequest request, ISender sender, CancellationToken cancellationToken)
    {
        return sender.Send(new ListProductsQuery(request.Page, request.PageSize, request.IsActive), cancellationToken).ToOk();
    }

    public record ListProductsRequest([FromQuery] int Page = 1, [FromQuery] int PageSize = 20, [FromQuery] bool? IsActive = null);
}
