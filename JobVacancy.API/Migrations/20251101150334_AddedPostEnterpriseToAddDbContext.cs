using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedPostEnterpriseToAddDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostEnterpriseEntity_BasePostTable_Id",
                table: "PostEnterpriseEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEnterpriseEntity_Categories_CategoryId",
                table: "PostEnterpriseEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEnterpriseEntity_Enterprises_EnterpriseId",
                table: "PostEnterpriseEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostEnterpriseEntity",
                table: "PostEnterpriseEntity");

            migrationBuilder.RenameTable(
                name: "PostEnterpriseEntity",
                newName: "PostEnterprise");

            migrationBuilder.RenameIndex(
                name: "IX_PostEnterpriseEntity_EnterpriseId",
                table: "PostEnterprise",
                newName: "IX_PostEnterprise_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_PostEnterpriseEntity_CategoryId",
                table: "PostEnterprise",
                newName: "IX_PostEnterprise_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostEnterprise",
                table: "PostEnterprise",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostEnterprise_BasePostTable_Id",
                table: "PostEnterprise",
                column: "Id",
                principalTable: "BasePostTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEnterprise_Categories_CategoryId",
                table: "PostEnterprise",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEnterprise_Enterprises_EnterpriseId",
                table: "PostEnterprise",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostEnterprise_BasePostTable_Id",
                table: "PostEnterprise");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEnterprise_Categories_CategoryId",
                table: "PostEnterprise");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEnterprise_Enterprises_EnterpriseId",
                table: "PostEnterprise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostEnterprise",
                table: "PostEnterprise");

            migrationBuilder.RenameTable(
                name: "PostEnterprise",
                newName: "PostEnterpriseEntity");

            migrationBuilder.RenameIndex(
                name: "IX_PostEnterprise_EnterpriseId",
                table: "PostEnterpriseEntity",
                newName: "IX_PostEnterpriseEntity_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_PostEnterprise_CategoryId",
                table: "PostEnterpriseEntity",
                newName: "IX_PostEnterpriseEntity_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostEnterpriseEntity",
                table: "PostEnterpriseEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostEnterpriseEntity_BasePostTable_Id",
                table: "PostEnterpriseEntity",
                column: "Id",
                principalTable: "BasePostTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEnterpriseEntity_Categories_CategoryId",
                table: "PostEnterpriseEntity",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEnterpriseEntity_Enterprises_EnterpriseId",
                table: "PostEnterpriseEntity",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
