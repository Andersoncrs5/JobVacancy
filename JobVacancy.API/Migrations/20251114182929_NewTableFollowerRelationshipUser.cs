using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableFollowerRelationshipUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FollowerRelationshipUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FollowerId = table.Column<string>(type: "text", nullable: false),
                    FollowedId = table.Column<string>(type: "text", nullable: false),
                    WishReceiveNotifyByNewPost = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewComment = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewInteraction = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowerRelationshipUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowerRelationshipUsers_app_users_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowerRelationshipUsers_app_users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowerRelationshipUsers_FollowedId",
                table: "FollowerRelationshipUsers",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowerRelationshipUsers_FollowerId_FollowedId",
                table: "FollowerRelationshipUsers",
                columns: new[] { "FollowerId", "FollowedId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FollowerRelationshipUsers");
        }
    }
}
