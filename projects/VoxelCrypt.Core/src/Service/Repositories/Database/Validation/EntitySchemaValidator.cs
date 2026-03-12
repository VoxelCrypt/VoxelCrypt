using System.Text.Json;
using Kernel.Models.Entities;
using Kernel.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Service.Persistence.Database.Validation;

public sealed class EntitySchemaValidator(ServiceDbContext dbContext) : IEntitySchemaValidator
{
	public async Task ValidateAsync(Entity entity, CancellationToken cancellationToken = default)
	{
		var schema = await dbContext.EntitySchemas
			.AsNoTracking()
			.FirstOrDefaultAsync(candidate =>
				candidate.Id == entity.EntitySchemaId && candidate.Version == entity.SchemaVersion,
				cancellationToken);

		if (schema is null)
		{
			throw new ResourceNotFoundException<EntitySchema>(
				$"Entity schema '{entity.EntitySchemaId.Value}' version '{entity.SchemaVersion}' was not found.");
		}

		var errors = new List<string>();

		foreach (var requiredProperty in schema.Required)
		{
			if (!entity.Content.TryGetValue(requiredProperty, out var value) || value is null)
			{
				errors.Add($"Missing required property '{requiredProperty}'.");
			}
		}

		foreach (var (propertyName, expectedType) in schema.Properties)
		{
			if (!entity.Content.TryGetValue(propertyName, out var value) || value is null)
			{
				continue;
			}

			if (!MatchesType(value, expectedType))
			{
				errors.Add($"Property '{propertyName}' expects type '{expectedType}'.");
			}
		}

		if (errors.Count > 0)
		{
			throw new EntitySchemaValidationException(errors);
		}
	}

	private static bool MatchesType(object value, string expectedType)
	{
		var normalizedType = expectedType.Trim().ToLowerInvariant();
		var jsonElement = ToJsonElement(value);

		return normalizedType switch
		{
			"string" => jsonElement.ValueKind == JsonValueKind.String,
			"number" => jsonElement.ValueKind == JsonValueKind.Number,
			"boolean" => jsonElement.ValueKind is JsonValueKind.True or JsonValueKind.False,
			"object" => jsonElement.ValueKind == JsonValueKind.Object,
			"array" => jsonElement.ValueKind == JsonValueKind.Array,
			_ => false
		};
	}

	private static JsonElement ToJsonElement(object value)
		=> value is JsonElement jsonElement
			? jsonElement
			: JsonSerializer.SerializeToElement(value);
}
