using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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

        modelBuilder.Entity("projects_menagment.Domain.Entities.RefreshToken", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<DateTime>("CreatedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("created_at");

                b.Property<DateTime>("ExpiresAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("expires_at");

                b.Property<DateTime?>("RevokedAt")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("revoked_at");

                b.Property<string>("Token")
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnType("character varying(512)")
                    .HasColumnName("token");

                b.Property<Guid>("UserId")
                    .HasColumnType("uuid")
                    .HasColumnName("user_id");

                b.HasKey("Id");

                b.HasIndex("Token")
                    .IsUnique();

                b.HasIndex("UserId");

                b.ToTable("refresh_tokens", (string)null);
            });

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

                b.HasKey("Id");

                b.HasIndex("Email")
                    .IsUnique();

                b.ToTable("users", (string)null);
            });

        modelBuilder.Entity("projects_menagment.Domain.Entities.RefreshToken", b =>
            {
                b.HasOne("projects_menagment.Domain.Entities.User", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
#pragma warning restore 612, 618
    }
}
