using Kernel.Models.ValueObjects;

namespace Kernel.Models.Entities;

public sealed class EntitySchema : Resource
{
	public EntitySchemaId Id { get; init; } = EntitySchemaId.New();

	public string Name { get; private set; } = string.Empty;

	public string Identifier { get; private set; } = string.Empty;

	public int Version { get; private set; } = 1;

	public Dictionary<string, string> Properties { get; private set; } = [];

	public HashSet<string> Required { get; private set; } = [];

	public void Define(
		string name,
		string identifier,
		int version,
		Dictionary<string, string>? properties,
		HashSet<string>? required)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Name is required.", nameof(name));
		}

		if (string.IsNullOrWhiteSpace(identifier))
		{
			throw new ArgumentException("Identifier is required.", nameof(identifier));
		}

		if (version < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(version), "Version must be greater than 0.");
		}

		Name = name;
		Identifier = identifier;
		Version = version;
		Properties = properties ?? [];
		Required = required ?? [];
	}
}
