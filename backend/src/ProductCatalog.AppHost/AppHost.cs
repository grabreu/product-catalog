var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("ProductCatalogDb");

builder.AddProject<Projects.ProductCatalog_Api>("api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
