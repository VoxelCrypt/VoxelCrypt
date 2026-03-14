using Kernel.Models.Entities;
using Kernel.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Service.Persistence.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .ValueGeneratedNever();

        builder.Property(user => user.Username)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => HashedPassword.From(value))
            .HasMaxLength(1024)
            .IsRequired();

        builder.HasMany(user => user.AccessTokens)
            .WithOne(token => token.User)
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(user => user.AccessTokens)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(user => user.Username)
            .IsUnique();
    }
}
