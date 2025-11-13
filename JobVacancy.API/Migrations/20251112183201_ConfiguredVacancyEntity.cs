using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguredVacancyEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicationSkillEntity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndicationSkillEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IndicationUserId = table.Column<string>(type: "text", nullable: false),
                    SkillId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserSkillId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicationSkillEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicationSkillEntity_IndicationUser_IndicationUserId",
                        column: x => x.IndicationUserId,
                        principalTable: "IndicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicationSkillEntity_UserSkill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "UserSkill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndicationSkillEntity_IndicationUserId",
                table: "IndicationSkillEntity",
                column: "IndicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicationSkillEntity_SkillId",
                table: "IndicationSkillEntity",
                column: "SkillId");
        }
    }
}
