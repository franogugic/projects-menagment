using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Infrastructure.Persistence.Configurations;

public sealed class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");

        builder.HasKey(organization => organization.Id);
        builder.Property(organization => organization.Id)
            .HasColumnName("id");

        builder.Property(organization => organization.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(organization => organization.PlanId)
            .HasColumnName("plan_id")
            .IsRequired();

        builder.Property(organization => organization.CreatedByUserId)
            .HasColumnName("created_by_user_id")
            .IsRequired();

        builder.Property(organization => organization.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne<Plan>()
            .WithMany()
            .HasForeignKey(organization => organization.PlanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(organization => organization.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(organization => organization.PlanId);
        builder.HasIndex(organization => organization.CreatedByUserId);
    }
}
