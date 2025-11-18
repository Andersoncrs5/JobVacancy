using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDubleKeyOfTableEnterpriseFollowsUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EnterpriseFollowsUser",
                table: "EnterpriseFollowsUser");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EnterpriseFollowsUser",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnterpriseFollowsUser",
                table: "EnterpriseFollowsUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseFollowsUser_EnterpriseId_UserId",
                table: "EnterpriseFollowsUser",
                columns: new[] { "EnterpriseId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EnterpriseFollowsUser",
                table: "EnterpriseFollowsUser");

            migrationBuilder.DropIndex(
                name: "IX_EnterpriseFollowsUser_EnterpriseId_UserId",
                table: "EnterpriseFollowsUser");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EnterpriseFollowsUser",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EnterpriseFollowsUser",
                table: "EnterpriseFollowsUser",
                columns: new[] { "EnterpriseId", "UserId" });
        }
    }
}
