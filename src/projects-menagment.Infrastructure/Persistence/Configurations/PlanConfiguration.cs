using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projects_menagment.Domain.Entities;

namespace projects_menagment.Infrastructure.Persistence.Configurations;

public sealed class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("plans");

        builder.HasKey(plan => plan.Id);
        builder.Property(plan => plan.Id)
            .HasColumnName("id");

        builder.Property(plan => plan.Code)
            .HasColumnName("code")
            .HasConversion(
                code => code.ToString().ToUpperInvariant(),
                dbValue => Enum.Parse<Domain.Enums.PlanCode>(dbValue, true))
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(plan => plan.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(plan => plan.MaxProjects)
            .HasColumnName("max_projects")
            .IsRequired();

        builder.Property(plan => plan.MaxMembers)
            .HasColumnName("max_members")
            .IsRequired();

        builder.Property(plan => plan.Price)
            .HasColumnName("price")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(plan => plan.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true)
            .IsRequired();

        builder.HasIndex(plan => plan.Code)
            .IsUnique();
    }
}
