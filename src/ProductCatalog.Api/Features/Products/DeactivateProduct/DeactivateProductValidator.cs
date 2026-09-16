namespace ProductCatalog.Api.Features.Products.DeactivateProduct;

public class DeactivateProductValidator : AbstractValidator<DeactivateProductCommand>
{
    public DeactivateProductValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
