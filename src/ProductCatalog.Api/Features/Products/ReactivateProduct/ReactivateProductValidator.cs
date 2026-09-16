namespace ProductCatalog.Api.Features.Products.ReactivateProduct;

public class ReactivateProductValidator : AbstractValidator<ReactivateProductCommand>
{
    public ReactivateProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
