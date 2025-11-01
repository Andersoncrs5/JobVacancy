using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguredUserSkillInContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEntity_Skills_SkillId",
                table: "UserSkillEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEntity_app_users_UserId",
                table: "UserSkillEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkillEntity",
                table: "UserSkillEntity");

            migrationBuilder.RenameTable(
                name: "UserSkillEntity",
                newName: "UserSkill");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkillEntity_UserId_SkillId",
                table: "UserSkill",
                newName: "IX_UserSkill_UserId_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkillEntity_UserId",
                table: "UserSkill",
                newName: "IX_UserSkill_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkillEntity_SkillId",
                table: "UserSkill",
                newName: "IX_UserSkill_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Skills_SkillId",
                table: "UserSkill",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_app_users_UserId",
                table: "UserSkill",
                column: "UserId",
                principalTable: "app_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Skills_SkillId",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_app_users_UserId",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.RenameTable(
                name: "UserSkill",
                newName: "UserSkillEntity");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_UserId_SkillId",
                table: "UserSkillEntity",
                newName: "IX_UserSkillEntity_UserId_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkillEntity",
                newName: "IX_UserSkillEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkillEntity",
                newName: "IX_UserSkillEntity_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkillEntity",
                table: "UserSkillEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillEntity_Skills_SkillId",
                table: "UserSkillEntity",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillEntity_app_users_UserId",
                table: "UserSkillEntity",
                column: "UserId",
                principalTable: "app_users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
