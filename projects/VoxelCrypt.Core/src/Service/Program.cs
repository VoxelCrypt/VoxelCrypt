using Microsoft.EntityFrameworkCore;
using Service.Persistence;
using Service.Persistence.Database;
using Service.Persistence.Database.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("voxelcrypt")
	?? builder.Configuration.GetConnectionString("Postgres")
	?? throw new InvalidOperationException("Connection string 'voxelcrypt' (Aspire) or 'Postgres' is required.");

builder.Services.AddDbContext<ServiceDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IEntitySchemaValidator, EntitySchemaValidator>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new { service = "VoxelCrypt.Core.Service", status = "ok" }));
app.MapDefaultEndpoints();

app.Run();
