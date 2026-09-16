namespace ProductCatalog.Api.Features.Products.ChangeProductPrice;

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
