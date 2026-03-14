using Kernel.Models.Entities;
using Kernel.Models.Exceptions;

namespace Kernel.Services;

public sealed class EntityValidationService : IEntityValidationService
{
	public void Validate(Entity entity, EntitySchema schema)
	{
		ArgumentNullException.ThrowIfNull(entity);
		ArgumentNullException.ThrowIfNull(schema);

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
			throw new ResourceValidationException(
				$"Entity content failed validation. (Schema: {schema.Identifier})",
				errors);
		}
	}

	private static bool MatchesType(object value, string expectedType)
	{
		var normalizedType = expectedType.Trim().ToLowerInvariant();

		return normalizedType switch
		{
			"string" => value is string,
			"number" => IsNumeric(value),
			"boolean" => value is bool,
			"object" => value is Dictionary<string, object>,
			"array" => value is List<object>,
			_ => false
		};
	}

	private static bool IsNumeric(object value)
		=> value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double or decimal;
}
