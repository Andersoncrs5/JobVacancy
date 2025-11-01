using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTablePostEnterprise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostEnterpriseEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostEnterpriseEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostEnterpriseEntity_BasePostTable_Id",
                        column: x => x.Id,
                        principalTable: "BasePostTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostEnterpriseEntity_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostEnterpriseEntity_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostEnterpriseEntity_CategoryId",
                table: "PostEnterpriseEntity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostEnterpriseEntity_EnterpriseId",
                table: "PostEnterpriseEntity",
                column: "EnterpriseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostEnterpriseEntity");
        }
    }
}
