using ProductCatalog.Application.Commands.Products.ChangePrice;

namespace ProductCatalog.UnitTests.Application.Commands.Products.ChangePrice;

public class ChangePriceCommandValidatorTests
{
    private readonly ChangePriceCommandValidator _validator = new();

    private static ChangePriceCommand ValidCommand => new(Guid.CreateVersion7(), 10m);

    [Fact]
    public void Validate_WithValidCommand_HasNoErrors()
    {
        // Act
        var result = _validator.TestValidate(ValidCommand);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithNonPositivePrice_HasErrorForNewPrice(decimal newPrice)
    {
        // Arrange
        var command = ValidCommand with { NewPrice = newPrice };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NewPrice);
    }
}
