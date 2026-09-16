namespace ProductCatalog.Api.Features.Products.AdjustProductStock;

public class AdjustProductStockValidator : AbstractValidator<AdjustProductStockCommand>
{
    public AdjustProductStockValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();
    }
}
