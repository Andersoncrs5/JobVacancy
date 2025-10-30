using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class SomeThingWasChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnterpriseIndustryEntity_Enterprises_EnterpriseId",
                table: "EnterpriseIndustryEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_EnterpriseIndustryEntity_Industries_IndustryId",
                table: "EnterpriseIndustryEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnterpriseIndustryEntity",
                table: "EnterpriseIndustryEntity");

            migrationBuilder.RenameTable(
                name: "EnterpriseIndustryEntity",
                newName: "EnterpriseIndustries");

            migrationBuilder.RenameIndex(
                name: "IX_EnterpriseIndustryEntity_IsPrimary",
                table: "EnterpriseIndustries",
                newName: "IX_EnterpriseIndustries_IsPrimary");

            migrationBuilder.RenameIndex(
                name: "IX_EnterpriseIndustryEntity_IndustryId",
                table: "EnterpriseIndustries",
                newName: "IX_EnterpriseIndustries_IndustryId");

            migrationBuilder.RenameIndex(
                name: "IX_EnterpriseIndustryEntity_EnterpriseId",
                table: "EnterpriseIndustries",
                newName: "IX_EnterpriseIndustries_EnterpriseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnterpriseIndustries",
                table: "EnterpriseIndustries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnterpriseIndustries_Enterprises_EnterpriseId",
                table: "EnterpriseIndustries",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnterpriseIndustries_Industries_IndustryId",
                table: "EnterpriseIndustries",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnterpriseIndustries_Enterprises_EnterpriseId",
                table: "EnterpriseIndustries");

            migrationBuilder.DropForeignKey(
                name: "FK_EnterpriseIndustries_Industries_IndustryId",
                table: "EnterpriseIndustries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EnterpriseIndustries",
                table: "EnterpriseIndustries");

            migrationBuilder.RenameTable(
                name: "EnterpriseIndustries",
                newName: "EnterpriseIndustryEntity");

            migrationBuilder.RenameIndex(
                name: "IX_EnterpriseIndustries_IsPrimary",
                table: "EnterpriseIndustryEntity",
                newName: "IX_EnterpriseIndustryEntity_IsPrimary");

            migrationBuilder.RenameIndex(
                name: "IX_EnterpriseIndustries_IndustryId",
                table: "EnterpriseIndustryEntity",
                newName: "IX_EnterpriseIndustryEntity_IndustryId");

            migrationBuilder.RenameIndex(
                name: "IX_EnterpriseIndustries_EnterpriseId",
                table: "EnterpriseIndustryEntity",
                newName: "IX_EnterpriseIndustryEntity_EnterpriseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnterpriseIndustryEntity",
                table: "EnterpriseIndustryEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnterpriseIndustryEntity_Enterprises_EnterpriseId",
                table: "EnterpriseIndustryEntity",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EnterpriseIndustryEntity_Industries_IndustryId",
                table: "EnterpriseIndustryEntity",
                column: "IndustryId",
                principalTable: "Industries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
