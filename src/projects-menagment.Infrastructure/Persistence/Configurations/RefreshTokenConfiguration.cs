using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder.HasKey(token => token.Id);

        builder.Property(token => token.Id)
            .HasColumnName("id");

        builder.Property(token => token.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(token => token.Token)
            .HasColumnName("token")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(token => token.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(token => token.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(token => token.RevokedAt)
            .HasColumnName("revoked_at")
            .HasColumnType("timestamp with time zone");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(token => token.Token)
            .IsUnique();

        builder.HasIndex(token => token.UserId);
    }
}
