using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableCommentPostUserAndCommentBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentBase",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Depth = table.Column<short>(type: "SMALLINT", nullable: true),
                    ParentCommentId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentBase_CommentBase_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "CommentBase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentBase_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentPostUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PostId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPostUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentPostUser_CommentBase_Id",
                        column: x => x.Id,
                        principalTable: "CommentBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentPostUser_PostUsers_PostId",
                        column: x => x.PostId,
                        principalTable: "PostUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentBase_ParentCommentId",
                table: "CommentBase",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentBase_UserId",
                table: "CommentBase",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPostUser_PostId",
                table: "CommentPostUser",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentPostUser");

            migrationBuilder.DropTable(
                name: "CommentBase");
        }
    }
}
