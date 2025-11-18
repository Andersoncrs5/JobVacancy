using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableReviewUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewUser",
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
                    Recommendation = table.Column<bool>(type: "boolean", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false),
                    ActorId = table.Column<string>(type: "text", nullable: false),
                    TargetUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewUser_app_users_ActorId",
                        column: x => x.ActorId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewUser_app_users_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUser_ActorId_TargetUserId",
                table: "ReviewUser",
                columns: new[] { "ActorId", "TargetUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUser_TargetUserId",
                table: "ReviewUser",
                column: "TargetUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewUser");
        }
    }
}
