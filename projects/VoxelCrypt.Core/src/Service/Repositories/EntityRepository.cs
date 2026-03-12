using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Service.Persistence.Database;
using Service.Persistence.Database.Validation;

namespace Service.Persistence;

public sealed class EntityRepository(ServiceDbContext dbContext, IEntitySchemaValidator schemaValidator) : IEntityRepository
{
	public async Task<Entity?> GetByIdAsync(EntityId id, CancellationToken cancellationToken = default)
		=> await dbContext.Entities
			.AsNoTracking()
			.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

	public async Task UpsertAsync(Entity entity, CancellationToken cancellationToken = default)
	{
		await schemaValidator.ValidateAsync(entity, cancellationToken);

		var existing = await dbContext.Entities
			.FirstOrDefaultAsync(candidate => candidate.Id == entity.Id, cancellationToken);

		if (existing is null)
		{
			await dbContext.Entities.AddAsync(entity, cancellationToken);
		}
		else
		{
			existing.SetSchema(entity.EntitySchemaId, entity.SchemaVersion);
			existing.ReplaceContent(new Dictionary<string, object>(entity.Content));
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(EntityId id, CancellationToken cancellationToken = default)
	{
		var existing = await dbContext.Entities
			.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

		if (existing is null)
		{
			return;
		}

		dbContext.Entities.Remove(existing);
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
