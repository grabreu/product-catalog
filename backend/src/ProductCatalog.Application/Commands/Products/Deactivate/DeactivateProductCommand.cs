namespace ProductCatalog.Application.Commands.Products.Deactivate;

public sealed record DeactivateProductCommand(Guid Id) : ICommand<ErrorOr<Deleted>>;
