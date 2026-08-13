using ProductCatalog.Application.Commands.Products.ChangePrice;
using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.UnitTests.Application.Commands.Products.ChangePrice;

public class ChangePriceCommandHandlerTests
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ChangePriceCommandHandler _handler;

    public ChangePriceCommandHandlerTests()
    {
        _repository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new ChangePriceCommandHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_WithExistingProduct_ChangesPriceAndSavesChanges()
    {
        // Arrange
        var product = Product.Create("Widget", "SKU-001", 10m, ProductCategory.Other);
        _repository.GetByIdAsync(product.Id, Arg.Any<CancellationToken>()).Returns(product);

        // Act
        var result = await _handler.Handle(new ChangePriceCommand(product.Id, 20m), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeFalse();
        result.Value.Price.ShouldBe(20m);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithNonExistingProduct_ReturnsNotFoundWithoutSavingChanges()
    {
        // Arrange
        var id = Guid.CreateVersion7();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Product?)null);

        // Act
        var result = await _handler.Handle(new ChangePriceCommand(id, 20m), CancellationToken.None);

        // Assert
        result.IsError.ShouldBeTrue();
        result.FirstError.Type.ShouldBe(ErrorType.NotFound);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
