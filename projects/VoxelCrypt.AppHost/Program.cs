var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithImage("postgres")
    .WithImageTag("16-alpine")
    .WithEnvironment("POSTGRES_DB", "voxelcrypt")
    .WithEnvironment("POSTGRES_USER", "voxelcrypt")
    .WithEnvironment("POSTGRES_PASSWORD", "voxelcrypt");

var database = postgres
    .AddDatabase("voxelcrypt");

builder
    .AddProject<Projects.Service>("service")
    .WithReference(database)
    .WaitFor(postgres);

builder.Build().Run();
