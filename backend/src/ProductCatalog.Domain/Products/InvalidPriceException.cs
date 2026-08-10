using ProductCatalog.Domain.Common;

namespace ProductCatalog.Domain.Products;

public sealed class InvalidPriceException() : DomainException("Price must be greater than zero.");
