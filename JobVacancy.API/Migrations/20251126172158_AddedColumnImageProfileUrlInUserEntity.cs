using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnImageProfileUrlInUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageProfileUrl",
                table: "app_users",
                type: "character varying(800)",
                maxLength: 800,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageProfileUrl",
                table: "app_users");
        }
    }
}
