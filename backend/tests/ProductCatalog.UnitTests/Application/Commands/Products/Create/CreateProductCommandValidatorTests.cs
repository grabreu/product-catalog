using ProductCatalog.Application.Commands.Products.Create;
using ProductCatalog.Domain.Products;

namespace ProductCatalog.UnitTests.Application.Commands.Products.Create;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator = new();

    private static CreateProductCommand ValidCommand => new("Widget", "SKU-001", 10m, ProductCategory.Other);

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        // Act
        var result = _validator.TestValidate(ValidCommand);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithMissingName_HasErrorForName(string? name)
    {
        // Arrange
        var command = ValidCommand with { Name = name! };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Validate_WithNameTooLong_HasErrorForName()
    {
        // Arrange
        var command = ValidCommand with { Name = new string('a', 201) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_WithMissingSku_HasErrorForSku(string? sku)
    {
        // Arrange
        var command = ValidCommand with { Sku = sku! };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Sku);
    }

    [Fact]
    public void Validate_WithSkuTooLong_HasErrorForSku()
    {
        // Arrange
        var command = ValidCommand with { Sku = new string('a', 51) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Sku);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithNonPositivePrice_HasErrorForPrice(decimal price)
    {
        // Arrange
        var command = ValidCommand with { Price = price };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Validate_WithUndefinedCategory_HasErrorForCategory()
    {
        // Arrange
        var command = ValidCommand with { Category = (ProductCategory)999 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Category);
    }
}
