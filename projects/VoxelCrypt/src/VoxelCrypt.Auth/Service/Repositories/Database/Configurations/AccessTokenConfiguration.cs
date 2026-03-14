using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Service.Persistence.Database.Configurations;

public sealed class AccessTokenConfiguration : IEntityTypeConfiguration<AccessToken>
{
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.ToTable("access_tokens");

        builder.HasKey(token => token.Token);

        builder.Property(token => token.Token)
            .HasMaxLength(512)
            .ValueGeneratedNever();

        builder.Property(token => token.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .IsRequired();

        builder.Property(token => token.ExpiresAt)
            .IsRequired();

        builder.HasIndex(token => token.UserId);
    }
}
