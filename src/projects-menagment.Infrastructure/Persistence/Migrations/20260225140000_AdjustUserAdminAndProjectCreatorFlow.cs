using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using projects_menagment.Infrastructure.Persistence;

#nullable disable

namespace projects_menagment.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260225140000_AdjustUserAdminAndProjectCreatorFlow")]
public partial class AdjustUserAdminAndProjectCreatorFlow : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "is_admin",
            table: "users",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.DropColumn(
            name: "role",
            table: "users");

        migrationBuilder.DropTable(
            name: "project_members");

        migrationBuilder.DropTable(
            name: "projects");

        migrationBuilder.CreateTable(
            name: "projects",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                budget = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_projects", x => x.id);
                table.ForeignKey(
                    name: "FK_projects_organizations_organization_id",
                    column: x => x.organization_id,
                    principalTable: "organizations",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_projects_users_created_by_user_id",
                    column: x => x.created_by_user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_projects_created_by_user_id",
            table: "projects",
            column: "created_by_user_id");

        migrationBuilder.CreateIndex(
            name: "IX_projects_organization_id",
            table: "projects",
            column: "organization_id");

        migrationBuilder.CreateIndex(
            name: "IX_projects_organization_id_name",
            table: "projects",
            columns: new[] { "organization_id", "name" });

        migrationBuilder.CreateTable(
            name: "project_members",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                project_id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_project_members", x => x.id);
                table.ForeignKey(
                    name: "FK_project_members_projects_project_id",
                    column: x => x.project_id,
                    principalTable: "projects",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_project_members_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_project_members_project_id",
            table: "project_members",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "IX_project_members_project_id_user_id",
            table: "project_members",
            columns: new[] { "project_id", "user_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_project_members_user_id",
            table: "project_members",
            column: "user_id");

        migrationBuilder.Sql("UPDATE organization_members SET role = 'OrganizationAdmin' WHERE role = 'ProjectApprover';");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "project_members");

        migrationBuilder.DropTable(
            name: "projects");

        migrationBuilder.AddColumn<string>(
            name: "role",
            table: "users",
            type: "character varying(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "Employee");

        migrationBuilder.DropColumn(
            name: "is_admin",
            table: "users");

        migrationBuilder.CreateTable(
            name: "projects",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                requested_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                approved_by_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                budget = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                approved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_projects", x => x.id);
                table.ForeignKey(
                    name: "FK_projects_organizations_organization_id",
                    column: x => x.organization_id,
                    principalTable: "organizations",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_projects_users_approved_by_user_id",
                    column: x => x.approved_by_user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "FK_projects_users_requested_by_user_id",
                    column: x => x.requested_by_user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_projects_approved_by_user_id",
            table: "projects",
            column: "approved_by_user_id");

        migrationBuilder.CreateIndex(
            name: "IX_projects_organization_id",
            table: "projects",
            column: "organization_id");

        migrationBuilder.CreateIndex(
            name: "IX_projects_organization_id_name",
            table: "projects",
            columns: new[] { "organization_id", "name" });

        migrationBuilder.CreateIndex(
            name: "IX_projects_requested_by_user_id",
            table: "projects",
            column: "requested_by_user_id");

        migrationBuilder.CreateTable(
            name: "project_members",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                project_id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_project_members", x => x.id);
                table.ForeignKey(
                    name: "FK_project_members_projects_project_id",
                    column: x => x.project_id,
                    principalTable: "projects",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_project_members_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_project_members_project_id",
            table: "project_members",
            column: "project_id");

        migrationBuilder.CreateIndex(
            name: "IX_project_members_project_id_role",
            table: "project_members",
            columns: new[] { "project_id", "role" },
            unique: true,
            filter: "role = 'Menager'");

        migrationBuilder.CreateIndex(
            name: "IX_project_members_project_id_user_id",
            table: "project_members",
            columns: new[] { "project_id", "user_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_project_members_user_id",
            table: "project_members",
            column: "user_id");

        migrationBuilder.Sql("UPDATE organization_members SET role = 'ProjectApprover' WHERE role = 'OrganizationAdmin';");
    }
}
