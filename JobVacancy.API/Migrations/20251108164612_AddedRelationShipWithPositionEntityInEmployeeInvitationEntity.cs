using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationShipWithPositionEntityInEmployeeInvitationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "EmployeeInvitations");

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                table: "EmployeeInvitations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_PositionId",
                table: "EmployeeInvitations",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeInvitations_Positions_PositionId",
                table: "EmployeeInvitations",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeInvitations_Positions_PositionId",
                table: "EmployeeInvitations");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeInvitations_PositionId",
                table: "EmployeeInvitations");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "EmployeeInvitations");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "EmployeeInvitations",
                type: "character varying(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");
        }
    }
}
