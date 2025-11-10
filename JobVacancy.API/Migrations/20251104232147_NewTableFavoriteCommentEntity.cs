using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableFavoriteCommentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteCommentEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CommentId = table.Column<string>(type: "text", nullable: false),
                    CommentBaseEntityId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteCommentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteCommentEntities_CommentBase_CommentBaseEntityId",
                        column: x => x.CommentBaseEntityId,
                        principalTable: "CommentBase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteCommentEntities_CommentBase_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CommentBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteCommentEntities_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCommentEntities_CommentBaseEntityId",
                table: "FavoriteCommentEntities",
                column: "CommentBaseEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCommentEntities_CommentId",
                table: "FavoriteCommentEntities",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCommentEntities_UserId_CommentId",
                table: "FavoriteCommentEntities",
                columns: new[] { "UserId", "CommentId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteCommentEntities");
        }
    }
}
