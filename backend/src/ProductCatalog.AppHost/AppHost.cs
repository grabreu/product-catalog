var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithDataVolume();

var productCatalogDb = sql.AddDatabase("ProductCatalogDb");

builder.AddProject<Projects.ProductCatalog_Api>("api")
    .WithReference(productCatalogDb)
    .WaitFor(productCatalogDb);

builder.Build().Run();
