using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projects_menagment.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationMemberInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organization_member_invitations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organization_id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    invited_by_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    accepted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_member_invitations", x => x.id);
                    table.ForeignKey(
                        name: "FK_organization_member_invitations_organizations_organization_~",
                        column: x => x.organization_id,
                        principalTable: "organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_organization_member_invitations_users_invited_by_user_id",
                        column: x => x.invited_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_organization_member_invitations_email",
                table: "organization_member_invitations",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_organization_member_invitations_invited_by_user_id",
                table: "organization_member_invitations",
                column: "invited_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_organization_member_invitations_organization_id",
                table: "organization_member_invitations",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_organization_member_invitations_token",
                table: "organization_member_invitations",
                column: "token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organization_member_invitations");
        }
    }
}
