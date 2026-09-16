namespace ProductCatalog.Api.Features.Products.UpdateProduct;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
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
