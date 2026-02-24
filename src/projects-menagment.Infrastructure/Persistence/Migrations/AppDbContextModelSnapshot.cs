using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using projects_menagment.Domain.Enums;
using projects_menagment.Infrastructure.Persistence;

#nullable disable

namespace projects_menagment.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.Entity("projects_menagment.Domain.Entities.User", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("character varying(256)")
                    .HasColumnName("email");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)")
                    .HasColumnName("first_name");

                b.Property<bool>("IsActive")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("boolean")
                    .HasColumnName("is_active")
                    .HasDefaultValue(true);

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("character varying(100)")
                    .HasColumnName("last_name");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnType("character varying(500)")
                    .HasColumnName("password_hash");

                b.Property<UserRole>("Role")
                    .IsRequired()
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .HasColumnType("character varying(50)")
                    .HasColumnName("role");

                b.HasKey("Id");

                b.HasIndex("Email")
                    .IsUnique();

                b.ToTable("users", (string)null);
            });
#pragma warning restore 612, 618
    }
}
