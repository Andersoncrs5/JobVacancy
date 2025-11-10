using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmployeeEnterpriseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeEnterprises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    ContractLink = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SalaryRange = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TerminationReason = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                    Notes = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                    SalaryValue = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentFrequency = table.Column<int>(type: "integer", nullable: false),
                    ContractLegalType = table.Column<int>(type: "integer", nullable: true),
                    ContractType = table.Column<int>(type: "integer", nullable: false),
                    SalaryCurrency = table.Column<int>(type: "integer", nullable: false),
                    EmploymentType = table.Column<int>(type: "integer", nullable: false),
                    EmploymentStatus = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PositionId = table.Column<string>(type: "text", nullable: false),
                    InviteSenderId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEnterprises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_app_users_InviteSenderId",
                        column: x => x.InviteSenderId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_EnterpriseId",
                table: "EmployeeEnterprises",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_InviteSenderId",
                table: "EmployeeEnterprises",
                column: "InviteSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_PositionId",
                table: "EmployeeEnterprises",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_UserId",
                table: "EmployeeEnterprises",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeEnterprises");
        }
    }
}
