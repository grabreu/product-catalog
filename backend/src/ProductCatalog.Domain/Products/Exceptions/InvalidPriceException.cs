using ProductCatalog.Domain.SeedWork;

namespace ProductCatalog.Domain.Products.Exceptions;

public sealed class InvalidPriceException() : DomainException("Price must be greater than zero.");
