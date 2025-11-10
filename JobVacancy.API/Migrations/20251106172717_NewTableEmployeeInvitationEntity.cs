using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableEmployeeInvitationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeInvitations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    RejectReason = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    InvitationLink = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    Token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Position = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    SalaryRange = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmploymentType = table.Column<int>(type: "integer", nullable: false),
                    ProposedStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProposedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    InviteSenderId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeInvitations_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeInvitations_app_users_InviteSenderId",
                        column: x => x.InviteSenderId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EmployeeInvitations_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_EnterpriseId",
                table: "EmployeeInvitations",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_InviteSenderId",
                table: "EmployeeInvitations",
                column: "InviteSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_UserId",
                table: "EmployeeInvitations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeInvitations");
        }
    }
}
