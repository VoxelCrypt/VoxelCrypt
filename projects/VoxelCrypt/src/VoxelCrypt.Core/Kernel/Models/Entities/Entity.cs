using Kernel.Models.ValueObjects;

namespace Kernel.Models.Entities;

public sealed class Entity : Resource
{
	public EntityId Id { get; init; } = EntityId.New();

	public EntitySchemaId EntitySchemaId { get; private set; }

	public EntitySchema? Schema { get; private set; }

	public int SchemaVersion { get; private set; } = 1;

	public Dictionary<string, object> Content { get; private set; } = [];

	public void SetSchema(EntitySchemaId entitySchemaId, int schemaVersion)
	{
		if (schemaVersion < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(schemaVersion), "Schema version must be greater than 0.");
		}

		EntitySchemaId = entitySchemaId;
		SchemaVersion = schemaVersion;
		Schema = null;
	}

	public void SetSchema(EntitySchema schema)
	{
		ArgumentNullException.ThrowIfNull(schema);

		EntitySchemaId = schema.Id;
		SchemaVersion = schema.Version;
		Schema = schema;
	}

	public void ReplaceContent(Dictionary<string, object>? content)
	{
		Content = content ?? [];
	}
}
