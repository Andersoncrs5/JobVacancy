using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTablePostUserMetrics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostUserMetrics",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    LikeCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    DislikeCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    CommentCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    FavoriteCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    RepublishedCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    SharedCount = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    PostId = table.Column<string>(type: "character varying(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUserMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostUserMetrics_PostUsers_PostId",
                        column: x => x.PostId,
                        principalTable: "PostUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUserMetrics_PostId",
                table: "PostUserMetrics",
                column: "PostId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUserMetrics");
        }
    }
}
