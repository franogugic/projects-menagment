using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Infrastructure.Persistence.Configurations;

public sealed class OrganizationMemberConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.ToTable("organization_members");

        builder.HasKey(member => member.Id);

        builder.Property(member => member.Id)
            .HasColumnName("id");

        builder.Property(member => member.OrganizationId)
            .HasColumnName("organization_id")
            .IsRequired();

        builder.Property(member => member.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(member => member.Role)
            .HasColumnName("role")
            .HasConversion(
                role => role.ToString().ToUpperInvariant(),
                dbValue => Enum.Parse<Domain.Enums.OrganizationMemberRole>(dbValue, true))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(member => member.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(member => member.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(member => member.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(member => member.OrganizationId);
        builder.HasIndex(member => member.UserId);
        builder.HasIndex(member => new { member.OrganizationId, member.UserId })
            .IsUnique();
        builder.HasIndex(member => new { member.OrganizationId, member.Role })
            .HasDatabaseName("IX_organization_members_org_role");
    }
}
