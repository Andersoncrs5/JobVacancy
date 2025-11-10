using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableReviewEnterpriseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "review_enterprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    RatingOverall = table.Column<short>(type: "SMALLINT", nullable: false),
                    RatingCulture = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingCompensation = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingManagement = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingWorkLifeBalance = table.Column<short>(type: "SMALLINT", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false),
                    PositionId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EnterpriseId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_enterprise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_enterprise_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_enterprise_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_enterprise_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_review_enterprise_EnterpriseId_UserId",
                table: "review_enterprise",
                columns: new[] { "EnterpriseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_enterprise_PositionId",
                table: "review_enterprise",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_review_enterprise_UserId",
                table: "review_enterprise",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "review_enterprise");
        }
    }
}
