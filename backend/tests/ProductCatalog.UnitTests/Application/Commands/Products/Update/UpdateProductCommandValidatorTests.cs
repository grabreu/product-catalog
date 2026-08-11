using ProductCatalog.Application.Commands.Products.Update;
using ProductCatalog.Domain.Products;

namespace ProductCatalog.UnitTests.Application.Commands.Products.Update;

public class UpdateProductCommandValidatorTests
{
    private readonly UpdateProductCommandValidator _validator = new();

    private static UpdateProductCommand ValidCommand => new(Guid.CreateVersion7(), "Widget", "A widget", ProductCategory.Other);

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

    [Fact]
    public void Validate_WithDescriptionTooLong_HasErrorForDescription()
    {
        // Arrange
        var command = ValidCommand with { Description = new string('a', 2001) };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description);
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
