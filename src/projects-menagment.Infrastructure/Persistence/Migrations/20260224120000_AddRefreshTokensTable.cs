using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using projects_menagment.Infrastructure.Persistence;

#nullable disable

namespace projects_menagment.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260224120000_AddRefreshTokensTable")]
public partial class AddRefreshTokensTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "refresh_tokens",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                token = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_refresh_tokens", x => x.id);
                table.ForeignKey(
                    name: "FK_refresh_tokens_users_user_id",
                    column: x => x.user_id,
                    principalTable: "users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_refresh_tokens_token",
            table: "refresh_tokens",
            column: "token",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_refresh_tokens_user_id",
            table: "refresh_tokens",
            column: "user_id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "refresh_tokens");
    }
}
