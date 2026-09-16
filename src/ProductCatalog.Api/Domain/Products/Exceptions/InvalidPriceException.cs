using ProductCatalog.Api.Domain.SeedWork;

namespace ProductCatalog.Api.Domain.Products.Exceptions;

public class InvalidPriceException() : DomainException("Price must be greater than zero.");
