using ProductCatalog.Domain.Products;
using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Application.Commands.Products.Deactivate;

public sealed class DeactivateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : ICommandHandler<DeactivateProductCommand, ErrorOr<Deleted>>
{
    public async ValueTask<ErrorOr<Deleted>> Handle(DeactivateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (product is null)
        {
            return Error.NotFound(description: $"No product was found with ID '{command.Id}'.");
        }

        product.Deactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}
