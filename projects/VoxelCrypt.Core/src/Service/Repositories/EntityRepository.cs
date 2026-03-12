using Kernel.Models.Entities;
using Kernel.Models.Exceptions;
using Kernel.Models.ValueObjects;
using Kernel.Services;
using Microsoft.EntityFrameworkCore;
using Service.Persistence.Database;

namespace Service.Persistence;

public sealed class EntityRepository(ServiceDbContext dbContext, IEntityValidationService entityValidationService) : IEntityRepository
{
	public async Task<Entity?> GetByIdAsync(EntityId id, CancellationToken cancellationToken = default)
		=> await dbContext.Entities
			.Include(entity => entity.Schema)
			.AsNoTracking()
			.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

	public async Task UpsertAsync(Entity entity, CancellationToken cancellationToken = default)
	{
		var schema = await dbContext.EntitySchemas
			.AsNoTracking()
			.FirstOrDefaultAsync(candidate =>
				candidate.Id == entity.EntitySchemaId && candidate.Version == entity.SchemaVersion,
				cancellationToken);

		if (schema is null)
		{
			var missingSchemaIdentifier = entity.Schema?.Identifier ?? "unknown";
			throw new ResourceNotFoundException<EntitySchema>(
				$"Entity schema '{entity.EntitySchemaId.Value}' version '{entity.SchemaVersion}' was not found. (Schema: {missingSchemaIdentifier})");
		}

		entity.SetSchema(schema);
		entity.ReplaceContent(NormalizeDictionary(entity.Content));
		entityValidationService.Validate(entity, schema);

		var existing = await dbContext.Entities
			.FirstOrDefaultAsync(candidate => candidate.Id == entity.Id, cancellationToken);

		if (existing is null)
		{
			await dbContext.Entities.AddAsync(entity, cancellationToken);
		}
		else
		{
			existing.SetSchema(schema);
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

	private static Dictionary<string, object> NormalizeDictionary(Dictionary<string, object> content)
	{
		var normalized = new Dictionary<string, object>(content.Count);
		foreach (var (key, value) in content)
		{
			normalized[key] = NormalizeValue(value);
		}

		return normalized;
	}

	private static object NormalizeValue(object value)
		=> value switch
		{
			System.Text.Json.JsonElement jsonElement => NormalizeJsonElement(jsonElement),
			_ => value
		};

	private static object NormalizeJsonElement(System.Text.Json.JsonElement jsonElement)
		=> jsonElement.ValueKind switch
		{
			System.Text.Json.JsonValueKind.Object => jsonElement
				.EnumerateObject()
				.ToDictionary(property => property.Name, property => NormalizeJsonElement(property.Value)),
			System.Text.Json.JsonValueKind.Array => jsonElement
				.EnumerateArray()
				.Select(NormalizeJsonElement)
				.ToList(),
			System.Text.Json.JsonValueKind.String => jsonElement.GetString() ?? string.Empty,
			System.Text.Json.JsonValueKind.Number => jsonElement.TryGetInt64(out var int64)
				? int64
				: jsonElement.GetDouble(),
			System.Text.Json.JsonValueKind.True => true,
			System.Text.Json.JsonValueKind.False => false,
			_ => string.Empty
		};
}
