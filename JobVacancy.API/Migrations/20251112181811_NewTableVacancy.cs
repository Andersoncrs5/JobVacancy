using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableVacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Requirements = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    Responsibilities = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    Benefits = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    EmploymentType = table.Column<int>(type: "integer", nullable: false),
                    ExperienceLevel = table.Column<int>(type: "integer", nullable: true),
                    EducationLevel = table.Column<int>(type: "integer", nullable: true),
                    WorkplaceType = table.Column<int>(type: "integer", nullable: true),
                    Currency = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Seniority = table.Column<int>(type: "integer", nullable: true),
                    Opening = table.Column<int>(type: "integer", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric", nullable: true),
                    SalaryMax = table.Column<decimal>(type: "numeric", nullable: true),
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    AreaId = table.Column<string>(type: "text", nullable: false),
                    ApplicationDeadLine = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastApplication = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacancies_AreaEntities_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vacancies_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_AreaId",
                table: "Vacancies",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_EnterpriseId",
                table: "Vacancies",
                column: "EnterpriseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vacancies");
        }
    }
}
