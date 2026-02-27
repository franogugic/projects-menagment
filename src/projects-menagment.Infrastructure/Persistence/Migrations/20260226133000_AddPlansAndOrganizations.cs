using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using projects_menagment.Infrastructure.Persistence;

#nullable disable

namespace projects_menagment.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260226133000_AddPlansAndOrganizations")]
public partial class AddPlansAndOrganizations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "plans",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                max_projects = table.Column<int>(type: "integer", nullable: false),
                max_members = table.Column<int>(type: "integer", nullable: false),
                price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_plans", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "organizations",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                plan_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_organizations", x => x.id);
                table.ForeignKey(
                    name: "FK_organizations_plans_plan_id",
                    column: x => x.plan_id,
                    principalTable: "plans",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_organizations_users_created_by_user_id",
                    column: x => x.created_by_user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_plans_code",
            table: "plans",
            column: "code",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_organizations_created_by_user_id",
            table: "organizations",
            column: "created_by_user_id");

        migrationBuilder.CreateIndex(
            name: "IX_organizations_plan_id",
            table: "organizations",
            column: "plan_id");

        migrationBuilder.Sql(
            """
            INSERT INTO plans (id, code, name, max_projects, max_members, price, is_active)
            VALUES
                ('11111111-1111-1111-1111-111111111111', 'FREE', 'Free', 3, 10, 0.00, true),
                ('22222222-2222-2222-2222-222222222222', 'PREMIUM', 'Premium', 20, 100, 29.99, true),
                ('33333333-3333-3333-3333-333333333333', 'PRO', 'Pro', 100, 1000, 99.99, true)
            ON CONFLICT (id) DO NOTHING;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "organizations");

        migrationBuilder.DropTable(
            name: "plans");
    }
}
