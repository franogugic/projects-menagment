using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using projects_menagment.Infrastructure.Persistence;

#nullable disable

namespace projects_menagment.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260227141000_AddOrganizationMembersTable")]
public partial class AddOrganizationMembersTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "organization_members",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_organization_members", x => x.id);
                table.ForeignKey(
                    name: "FK_organization_members_organizations_organization_id",
                    column: x => x.organization_id,
                    principalTable: "organizations",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_organization_members_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_organization_members_organization_id",
            table: "organization_members",
            column: "organization_id");

        migrationBuilder.CreateIndex(
            name: "IX_organization_members_org_role",
            table: "organization_members",
            columns: new[] { "organization_id", "role" });

        migrationBuilder.CreateIndex(
            name: "IX_organization_members_organization_id_user_id",
            table: "organization_members",
            columns: new[] { "organization_id", "user_id" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_organization_members_user_id",
            table: "organization_members",
            column: "user_id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "organization_members");
    }
}
