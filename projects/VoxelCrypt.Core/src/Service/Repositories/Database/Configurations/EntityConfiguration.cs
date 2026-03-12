using System.Text.Json;
using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Service.Persistence.Database.Configurations;

public sealed class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    public void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder.ToTable("entities");

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id)
            .HasConversion(
                id => id.Value,
                value => EntityId.From(value))
            .ValueGeneratedNever();

        builder.Property(entity => entity.EntitySchemaId)
            .HasConversion(
                id => id.Value,
                value => EntitySchemaId.From(value))
            .IsRequired();

        builder.Property(entity => entity.SchemaVersion)
            .IsRequired();

        builder.HasOne<EntitySchema>()
            .WithMany()
            .HasForeignKey(entity => entity.EntitySchemaId)
            .HasPrincipalKey(schema => schema.Id)
            .OnDelete(DeleteBehavior.Restrict);

        var contentComparer = new ValueComparer<Dictionary<string, object>>(
            (left, right) => SerializeContent(left) == SerializeContent(right),
            content => SerializeContent(content).GetHashCode(),
            content => DeserializeContent(SerializeContent(content)));

        builder.Property(entity => entity.Content)
            .HasColumnType("jsonb")
            .HasConversion(
                content => SerializeContent(content),
                json => DeserializeContent(json))
            .Metadata.SetValueComparer(contentComparer);
    }

    private static string SerializeContent(Dictionary<string, object>? content)
        => JsonSerializer.Serialize(content ?? new Dictionary<string, object>());

    private static Dictionary<string, object> DeserializeContent(string json)
        => JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? [];
}
