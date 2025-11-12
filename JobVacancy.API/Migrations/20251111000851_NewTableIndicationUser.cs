using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableIndicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EndorserId = table.Column<string>(type: "text", nullable: false),
                    EndorsedId = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    AcceptanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SkillRating = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicationUser_app_users_EndorsedId",
                        column: x => x.EndorsedId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicationUser_app_users_EndorserId",
                        column: x => x.EndorserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndicationUser_EndorsedId",
                table: "IndicationUser",
                column: "EndorsedId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicationUser_EndorserId_EndorsedId",
                table: "IndicationUser",
                columns: new[] { "EndorserId", "EndorsedId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndicationUser");
        }
    }
}
