namespace ProductCatalog.Api.Features.Products.GetProductById;

public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
