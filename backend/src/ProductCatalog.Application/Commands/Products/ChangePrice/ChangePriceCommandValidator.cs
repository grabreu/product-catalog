namespace ProductCatalog.Application.Commands.Products.ChangePrice;

public sealed class ChangePriceCommandValidator : AbstractValidator<ChangePriceCommand>
{
    public ChangePriceCommandValidator()
    {
        RuleFor(x => x.NewPrice)
            .GreaterThan(0);
    }
}
