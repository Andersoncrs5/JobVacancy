using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedColumnSalaryCurrencyOfEmployeeEnterpriseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryCurrency",
                table: "EmployeeEnterprises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalaryCurrency",
                table: "EmployeeEnterprises",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
