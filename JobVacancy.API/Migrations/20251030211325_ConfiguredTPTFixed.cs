using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguredTPTFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasePostTable_Categories_CategoryId",
                table: "BasePostTable");

            migrationBuilder.DropForeignKey(
                name: "FK_BasePostTable_app_users_UserId",
                table: "BasePostTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasePostTable",
                table: "BasePostTable");

            migrationBuilder.DropIndex(
                name: "IX_BasePostTable_CategoryId",
                table: "BasePostTable");

            migrationBuilder.DropIndex(
                name: "IX_BasePostTable_UserId",
                table: "BasePostTable");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BasePostTable");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "BasePostTable");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BasePostTable");

            migrationBuilder.RenameTable(
                name: "BasePostTable",
                newName: "PostsBase");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostsBase",
                table: "PostsBase",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PostUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostUsers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUsers_PostsBase_Id",
                        column: x => x.Id,
                        principalTable: "PostsBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUsers_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUsers_CategoryId",
                table: "PostUsers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUsers_UserId",
                table: "PostUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostsBase",
                table: "PostsBase");

            migrationBuilder.RenameTable(
                name: "PostsBase",
                newName: "BasePostTable");

            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "BasePostTable",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "BasePostTable",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BasePostTable",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasePostTable",
                table: "BasePostTable",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BasePostTable_CategoryId",
                table: "BasePostTable",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BasePostTable_UserId",
                table: "BasePostTable",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasePostTable_Categories_CategoryId",
                table: "BasePostTable",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasePostTable_app_users_UserId",
                table: "BasePostTable",
                column: "UserId",
                principalTable: "app_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
