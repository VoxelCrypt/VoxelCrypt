using Kernel.Services;
using Microsoft.EntityFrameworkCore;
using Service.Persistence;
using Service.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("voxelcrypt")
	?? builder.Configuration.GetConnectionString("Postgres")
	?? throw new InvalidOperationException("Connection string 'voxelcrypt' (Aspire) or 'Postgres' is required.");

builder.Services.AddDbContext<ServiceDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IEntityValidationService, EntityValidationService>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();
builder.Services.AddScoped<IEntitySchemaRepository, EntitySchemaRepository>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { service = "VoxelCrypt.Core.Service", status = "ok" }));
app.MapDefaultEndpoints();

app.Run();
