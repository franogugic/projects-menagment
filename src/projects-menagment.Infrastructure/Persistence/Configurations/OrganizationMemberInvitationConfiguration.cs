using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Infrastructure.Persistence.Configurations;

public sealed class OrganizationMemberInvitationConfiguration : IEntityTypeConfiguration<OrganizationMemberInvitation>
{
    public void Configure(EntityTypeBuilder<OrganizationMemberInvitation> builder)
    {
        builder.ToTable("organization_member_invitations");

        builder.HasKey(invitation => invitation.Id);

        builder.Property(invitation => invitation.Id)
            .HasColumnName("id");

        builder.Property(invitation => invitation.OrganizationId)
            .HasColumnName("organization_id")
            .IsRequired();

        builder.Property(invitation => invitation.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(invitation => invitation.Role)
            .HasColumnName("role")
            .HasConversion(
                role => role.ToString().ToUpperInvariant(),
                dbValue => ParseOrganizationMemberRole(dbValue))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(invitation => invitation.InvitedByUserId)
            .HasColumnName("invited_by_user_id")
            .IsRequired();

        builder.Property(invitation => invitation.Token)
            .HasColumnName("token")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(invitation => invitation.ExpiresAt)
            .HasColumnName("expires_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(invitation => invitation.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(invitation => invitation.AcceptedAt)
            .HasColumnName("accepted_at")
            .HasColumnType("timestamp with time zone");

        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(invitation => invitation.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(invitation => invitation.InvitedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(invitation => invitation.Token)
            .IsUnique();
        builder.HasIndex(invitation => invitation.OrganizationId);
        builder.HasIndex(invitation => invitation.Email);
    }

    private static Domain.Enums.OrganizationMemberRole ParseOrganizationMemberRole(string dbValue)
    {
        return dbValue.Trim().ToUpperInvariant() switch
        {
            "OWNER" => Domain.Enums.OrganizationMemberRole.Owner,
            "MENAGER" => Domain.Enums.OrganizationMemberRole.Menager,
            "MANAGER" => Domain.Enums.OrganizationMemberRole.Menager,
            "EMPLOYEE" => Domain.Enums.OrganizationMemberRole.Employee,
            _ => throw new InvalidOperationException($"Unsupported organization invitation role value '{dbValue}'.")
        };
    }
}
