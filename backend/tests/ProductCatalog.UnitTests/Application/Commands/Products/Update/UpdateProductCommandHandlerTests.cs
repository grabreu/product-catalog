using ProductCatalog.Application.Commands.Products.Update;
using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.UnitTests.Application.Commands.Products.Update;

public class UpdateProductCommandHandlerTests
{
    private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private UpdateProductCommandHandler Handler => new(_repository, _unitOfWork);

    private static UpdateProductCommand ValidCommand(Guid id) => new(id, "Widget Pro", "Updated description", ProductCategory.Electronics);

    [Fact]
    public async Task Handle_WithExistingProduct_UpdatesProductAndSavesChanges()
    {
        // Arrange
        var product = Product.Create("Widget", "SKU-001", 10m, ProductCategory.Other);
        _repository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>()).Returns(product);

        var command = ValidCommand(product.Id);

        // Act
        var result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.Name.ShouldBe(command.Name);
        result.Value.Description.ShouldBe(command.Description);
        result.Value.Category.ShouldBe(command.Category);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistingProduct_ReturnsNotFoundWithoutSavingChanges()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // Act
        var result = await Handler.Handle(ValidCommand(id), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.Type.ShouldBe(ErrorType.NotFound);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
