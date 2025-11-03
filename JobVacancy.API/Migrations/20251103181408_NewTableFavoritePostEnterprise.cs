using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableFavoritePostEnterprise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoritePostEnterprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PostEnterpriseId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePostEnterprise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoritePostEnterprise_PostEnterprises_PostEnterpriseId",
                        column: x => x.PostEnterpriseId,
                        principalTable: "PostEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritePostEnterprise_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostEnterprise_PostEnterpriseId",
                table: "FavoritePostEnterprise",
                column: "PostEnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostEnterprise_UserId",
                table: "FavoritePostEnterprise",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostEnterprise_UserId_PostEnterpriseId",
                table: "FavoritePostEnterprise",
                columns: new[] { "UserId", "PostEnterpriseId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoritePostEnterprise");
        }
    }
}
