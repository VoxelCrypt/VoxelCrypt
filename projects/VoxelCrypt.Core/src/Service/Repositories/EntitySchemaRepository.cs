using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Service.Persistence.Database;

namespace Service.Persistence;

public sealed class EntitySchemaRepository(ServiceDbContext dbContext) : IEntitySchemaRepository
{
	public async Task<EntitySchema?> GetByIdAsync(EntitySchemaId id, CancellationToken cancellationToken = default)
		=> await dbContext.EntitySchemas
			.AsNoTracking()
			.FirstOrDefaultAsync(schema => schema.Id == id, cancellationToken);

	public async Task<EntitySchema?> GetByIdentifierAsync(string identifier, int version, CancellationToken cancellationToken = default)
		=> await dbContext.EntitySchemas
			.AsNoTracking()
			.FirstOrDefaultAsync(schema => schema.Identifier == identifier && schema.Version == version, cancellationToken);

	public async Task UpsertAsync(EntitySchema entitySchema, CancellationToken cancellationToken = default)
	{
		var existing = await dbContext.EntitySchemas
			.FirstOrDefaultAsync(schema => schema.Id == entitySchema.Id, cancellationToken);

		if (existing is null)
		{
			await dbContext.EntitySchemas.AddAsync(entitySchema, cancellationToken);
		}
		else
		{
			existing.Define(
				entitySchema.Name,
				entitySchema.Identifier,
				entitySchema.Version,
				new Dictionary<string, string>(entitySchema.Properties),
				new HashSet<string>(entitySchema.Required));
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(EntitySchemaId id, CancellationToken cancellationToken = default)
	{
		var existing = await dbContext.EntitySchemas
			.FirstOrDefaultAsync(schema => schema.Id == id, cancellationToken);

		if (existing is null)
		{
			return;
		}

		dbContext.EntitySchemas.Remove(existing);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
