using System.Text.Json;
using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Service.Persistence.Database.Configurations;

public sealed class EntitySchemaConfiguration : IEntityTypeConfiguration<EntitySchema>
{
	public void Configure(EntityTypeBuilder<EntitySchema> builder)
	{
		builder.ToTable("entity_schemas");

		builder.HasKey(schema => schema.Id);

		builder.Property(schema => schema.Id)
			.HasConversion(
				id => id.Value,
				value => EntitySchemaId.From(value))
			.ValueGeneratedNever();

		builder.Property(schema => schema.Name)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(schema => schema.Identifier)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(schema => schema.Version)
			.IsRequired();

		var propertiesComparer = new ValueComparer<Dictionary<string, string>>(
			(left, right) => SerializeProperties(left) == SerializeProperties(right),
			properties => SerializeProperties(properties).GetHashCode(),
			properties => DeserializeProperties(SerializeProperties(properties)));

		builder.Property(schema => schema.Properties)
			.HasColumnType("jsonb")
			.HasConversion(
				properties => SerializeProperties(properties),
				json => DeserializeProperties(json))
			.Metadata.SetValueComparer(propertiesComparer);

		var requiredComparer = new ValueComparer<HashSet<string>>(
			(left, right) => SerializeRequired(left) == SerializeRequired(right),
			required => SerializeRequired(required).GetHashCode(),
			required => DeserializeRequired(SerializeRequired(required)));

		builder.Property(schema => schema.Required)
			.HasColumnType("jsonb")
			.HasConversion(
				required => SerializeRequired(required),
				json => DeserializeRequired(json))
			.Metadata.SetValueComparer(requiredComparer);

		builder.HasIndex(schema => new { schema.Identifier, schema.Version })
			.IsUnique();
	}

	private static string SerializeProperties(Dictionary<string, string>? properties)
		=> JsonSerializer.Serialize(properties ?? new Dictionary<string, string>());

	private static Dictionary<string, string> DeserializeProperties(string json)
		=> JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];

	private static string SerializeRequired(HashSet<string>? required)
		=> JsonSerializer.Serialize(required ?? new HashSet<string>());

	private static HashSet<string> DeserializeRequired(string json)
		=> JsonSerializer.Deserialize<HashSet<string>>(json) ?? [];
}
