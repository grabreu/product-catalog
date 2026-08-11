using ProductCatalog.Application.Queries.Products.List;

namespace ProductCatalog.UnitTests.Application.Queries.Products.List;

public class GetProductsQueryValidatorTests
{
    private readonly GetProductsQueryValidator _validator = new();

    private static GetProductsQuery ValidQuery => new(1, 20);

    [Fact]
    public void Validate_WithValidQuery_HasNoErrors()
    {
        // Act
        var result = _validator.TestValidate(ValidQuery);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithNonPositivePage_HasErrorForPage(int page)
    {
        // Arrange
        var query = ValidQuery with { Page = page };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void Validate_WithPageSizeOutOfRange_HasErrorForPageSize(int pageSize)
    {
        // Arrange
        var query = ValidQuery with { PageSize = pageSize };

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
}
