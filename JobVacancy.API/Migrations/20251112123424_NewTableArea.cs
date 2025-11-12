using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewTableArea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndicationSkillEntity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IndicationUserId = table.Column<string>(type: "text", nullable: false),
                    UserSkillId = table.Column<string>(type: "text", nullable: false),
                    SkillId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicationSkillEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndicationSkillEntity_IndicationUser_IndicationUserId",
                        column: x => x.IndicationUserId,
                        principalTable: "IndicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicationSkillEntity_UserSkill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "UserSkill",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaEntities_IsActive",
                table: "AreaEntities",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_AreaEntities_Name",
                table: "AreaEntities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndicationSkillEntity_IndicationUserId",
                table: "IndicationSkillEntity",
                column: "IndicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicationSkillEntity_SkillId",
                table: "IndicationSkillEntity",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaEntities");

            migrationBuilder.DropTable(
                name: "IndicationSkillEntity");
        }
    }
}
