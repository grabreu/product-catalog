namespace ProductCatalog.IntegrationTests.Common;

[CollectionDefinition(Name)]
public sealed class IntegrationTestCollection : ICollectionFixture<ProductCatalogApiFactory>
{
    public const string Name = "Integration";
}
