using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableEnterpriseFollowsUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnterpriseFollowsUser",
                columns: table => new
                {
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    WishReceiveNotifyByNewPost = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewEndorsement = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByProfileUpdate = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewInteraction = table.Column<bool>(type: "boolean", nullable: false),
                    Id = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseFollowsUser", x => new { x.EnterpriseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_EnterpriseFollowsUser_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseFollowsUser_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowerUserRelationshipEnterprise_UserId",
                table: "FollowerUserRelationshipEnterprise",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseFollowsUser_EnterpriseId",
                table: "EnterpriseFollowsUser",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseFollowsUser_UserId",
                table: "EnterpriseFollowsUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnterpriseFollowsUser");

            migrationBuilder.DropIndex(
                name: "IX_FollowerUserRelationshipEnterprise_UserId",
                table: "FollowerUserRelationshipEnterprise");
        }
    }
}
