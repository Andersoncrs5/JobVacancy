using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class FixErrorTPT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUsers_Categories_CategoryId",
                table: "PostUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUsers_PostsBase_Id",
                table: "PostUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUsers_app_users_UserId",
                table: "PostUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUsers",
                table: "PostUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostsBase",
                table: "PostsBase");

            migrationBuilder.RenameTable(
                name: "PostUsers",
                newName: "PostUser");

            migrationBuilder.RenameTable(
                name: "PostsBase",
                newName: "BasePostTable");

            migrationBuilder.RenameIndex(
                name: "IX_PostUsers_UserId",
                table: "PostUser",
                newName: "IX_PostUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostUsers_CategoryId",
                table: "PostUser",
                newName: "IX_PostUser_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUser",
                table: "PostUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasePostTable",
                table: "BasePostTable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser_BasePostTable_Id",
                table: "PostUser",
                column: "Id",
                principalTable: "BasePostTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser_Categories_CategoryId",
                table: "PostUser",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser_app_users_UserId",
                table: "PostUser",
                column: "UserId",
                principalTable: "app_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUser_BasePostTable_Id",
                table: "PostUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUser_Categories_CategoryId",
                table: "PostUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUser_app_users_UserId",
                table: "PostUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUser",
                table: "PostUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasePostTable",
                table: "BasePostTable");

            migrationBuilder.RenameTable(
                name: "PostUser",
                newName: "PostUsers");

            migrationBuilder.RenameTable(
                name: "BasePostTable",
                newName: "PostsBase");

            migrationBuilder.RenameIndex(
                name: "IX_PostUser_UserId",
                table: "PostUsers",
                newName: "IX_PostUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostUser_CategoryId",
                table: "PostUsers",
                newName: "IX_PostUsers_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUsers",
                table: "PostUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostsBase",
                table: "PostsBase",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostUsers_Categories_CategoryId",
                table: "PostUsers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUsers_PostsBase_Id",
                table: "PostUsers",
                column: "Id",
                principalTable: "PostsBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUsers_app_users_UserId",
                table: "PostUsers",
                column: "UserId",
                principalTable: "app_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
