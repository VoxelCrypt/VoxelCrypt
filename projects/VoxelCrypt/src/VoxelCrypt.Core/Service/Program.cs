using System;
using Kernel.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Persistence;
using Service.Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Postgres")
	?? builder.Configuration.GetConnectionString("Default")
	?? builder.Configuration.GetConnectionString("voxelcrypt")
	?? throw new InvalidOperationException("Connection string 'Postgres', 'Default', or 'voxelcrypt' is required.");

builder.Services.AddDbContext<ServiceDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IEntityValidationService, EntityValidationService>();
builder.Services.AddScoped<IEntityRepository, EntityRepository>();
builder.Services.AddScoped<IEntitySchemaRepository, EntitySchemaRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Ok(new { service = "VoxelCrypt.Core.Service", status = "ok" }));
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
