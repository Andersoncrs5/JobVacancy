using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewColumnsToFavoritePostUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNotes",
                table: "FavoritePostUser",
                type: "character varying(600)",
                maxLength: 600,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "UserRating",
                table: "FavoritePostUser",
                type: "SMALLINT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNotes",
                table: "FavoritePostUser");

            migrationBuilder.DropColumn(
                name: "UserRating",
                table: "FavoritePostUser");
        }
    }
}
