using Kernel.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Service.Persistence.Database;

public sealed class ServiceDbContext(DbContextOptions<ServiceDbContext> options) : DbContext(options)
{
	public DbSet<Entity> Entities => Set<Entity>();
	public DbSet<EntitySchema> EntitySchemas => Set<EntitySchema>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServiceDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}
