using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableFollowerUserRelationshipEnterprise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowerUserRelationshipEnterprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    WishReceiveNotifyByNewPost = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewVacancy = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewComment = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewInteraction = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowerUserRelationshipEnterprise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowerUserRelationshipEnterprise_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowerUserRelationshipEnterprise_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowerUserRelationshipEnterprise_EnterpriseId",
                table: "FollowerUserRelationshipEnterprise",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowerUserRelationshipEnterprise_UserId_EnterpriseId",
                table: "FollowerUserRelationshipEnterprise",
                columns: new[] { "UserId", "EnterpriseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowerUserRelationshipEnterprise");
        }
    }
}
