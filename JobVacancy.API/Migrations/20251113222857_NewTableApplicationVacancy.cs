using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableApplicationVacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Order",
                table: "VacancySkills",
                type: "SMALLINT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationVacancies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    VacancyId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LastStatusUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CoverLetter = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: true),
                    IsViewedByRecruiter = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationVacancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationVacancies_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationVacancies_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationVacancies_UserId",
                table: "ApplicationVacancies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationVacancies_VacancyId",
                table: "ApplicationVacancies",
                column: "VacancyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationVacancies");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "VacancySkills");
        }
    }
}
