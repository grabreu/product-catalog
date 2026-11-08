var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var sqldb = sql.AddDatabase("sqldb");

builder.AddProject<Projects.ProductCatalog_Api>("api")
    .WithReference(sqldb, "DefaultConnection")
    .WaitFor(sqldb);

var app = builder.Build();

await app.RunAsync();
