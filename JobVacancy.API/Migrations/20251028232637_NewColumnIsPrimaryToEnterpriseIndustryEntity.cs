using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnIsPrimaryToEnterpriseIndustryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnterpriseIndustryEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    IndustryId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseIndustryEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseIndustryEntity_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseIndustryEntity_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseIndustryEntity_EnterpriseId",
                table: "EnterpriseIndustryEntity",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseIndustryEntity_IndustryId",
                table: "EnterpriseIndustryEntity",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseIndustryEntity_IsPrimary",
                table: "EnterpriseIndustryEntity",
                column: "IsPrimary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnterpriseIndustryEntity");
        }
    }
}
