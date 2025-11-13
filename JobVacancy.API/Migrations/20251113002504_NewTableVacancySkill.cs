using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableVacancySkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Seniority",
                table: "Vacancies",
                type: "SMALLINT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Opening",
                table: "Vacancies",
                type: "SMALLINT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "VacancySkills",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    VacancyId = table.Column<string>(type: "text", nullable: false),
                    SkillId = table.Column<string>(type: "text", nullable: false),
                    RequiredLevel = table.Column<int>(type: "integer", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Weight = table.Column<short>(type: "SMALLINT", nullable: false),
                    YearsOfExperienceRequired = table.Column<short>(type: "SMALLINT", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancySkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacancySkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancySkills_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VacancySkills_SkillId",
                table: "VacancySkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancySkills_VacancyId_SkillId",
                table: "VacancySkills",
                columns: new[] { "VacancyId", "SkillId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacancySkills");

            migrationBuilder.AlterColumn<int>(
                name: "Seniority",
                table: "Vacancies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Opening",
                table: "Vacancies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "SMALLINT");
        }
    }
}
