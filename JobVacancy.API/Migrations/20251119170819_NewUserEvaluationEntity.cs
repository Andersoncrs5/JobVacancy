using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class NewUserEvaluationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VacancyId",
                table: "VacancySkills",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SkillId",
                table: "VacancySkills",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "VacancySkills",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "Vacancies",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "AreaId",
                table: "Vacancies",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Vacancies",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SkillId",
                table: "UserSkill",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UserSkill",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Skills",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ReviewUser",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "review_enterprise",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "review_enterprise",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "review_enterprise",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "PostUsers",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PostUsers",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PostsBase",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "PostEnterprises",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "PostEnterprises",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PostEnterprises",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Positions",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Industries",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "IndicationUser",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "FollowerUserRelationshipEnterprise",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FollowerUserRelationshipEnterprise",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FollowerRelationshipUsers",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PostUserId",
                table: "FavoritePostUser",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FavoritePostUser",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PostEnterpriseId",
                table: "FavoritePostEnterprise",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FavoritePostEnterprise",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CommentId",
                table: "FavoriteCommentEntities",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CommentBaseEntityId",
                table: "FavoriteCommentEntities",
                type: "character varying(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FavoriteCommentEntities",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Enterprises",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "IndustryId",
                table: "EnterpriseIndustries",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EnterpriseIndustries",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EnterpriseIndustries",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EnterpriseFollowsUser",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EnterpriseFollowsUser",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "EmployeeInvitations",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EmployeeInvitations",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EmployeeInvitations",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "EmployeeEnterprises",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EmployeeEnterprises",
                type: "character varying(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EmployeeEnterprises",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "CommentPostUser",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CommentPostUser",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "CommentPostEnterprise",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CommentPostEnterprise",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentBase",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ParentCommentId",
                table: "CommentBase",
                type: "character varying(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CommentBase",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Categories",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AreaEntities",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "VacancyId",
                table: "ApplicationVacancies",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ApplicationVacancies",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ApplicationVacancies",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "UserEvaluations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    RatingProfessionalism = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingSkillMatch = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingTeamwork = table.Column<short>(type: "SMALLINT", nullable: true),
                    RecommendationTone = table.Column<short>(type: "SMALLINT", nullable: true),
                    EmploymentStatus = table.Column<string>(type: "text", nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    TargetUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ReviewerUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    PositionId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Content = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    RatingOverall = table.Column<short>(type: "SMALLINT", nullable: false),
                    RatingCulture = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingCompensation = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingManagement = table.Column<short>(type: "SMALLINT", nullable: true),
                    RatingWorkLifeBalance = table.Column<short>(type: "SMALLINT", nullable: true),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEvaluations_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvaluations_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvaluations_app_users_ReviewerUserId",
                        column: x => x.ReviewerUserId,
                        principalTable: "app_users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserEvaluations_app_users_TargetUserId",
                        column: x => x.TargetUserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEvaluations_EnterpriseId_TargetUserId",
                table: "UserEvaluations",
                columns: new[] { "EnterpriseId", "TargetUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEvaluations_PositionId",
                table: "UserEvaluations",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvaluations_ReviewerUserId",
                table: "UserEvaluations",
                column: "ReviewerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvaluations_TargetUserId",
                table: "UserEvaluations",
                column: "TargetUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEvaluations");

            migrationBuilder.AlterColumn<string>(
                name: "VacancyId",
                table: "VacancySkills",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "SkillId",
                table: "VacancySkills",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "VacancySkills",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "Vacancies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AreaId",
                table: "Vacancies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Vacancies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "SkillId",
                table: "UserSkill",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UserSkill",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Skills",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ReviewUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "review_enterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "review_enterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "review_enterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "PostUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PostUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PostsBase",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "PostEnterprises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "PostEnterprises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PostEnterprises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Positions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Industries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "IndicationUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "FollowerUserRelationshipEnterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FollowerUserRelationshipEnterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FollowerRelationshipUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PostUserId",
                table: "FavoritePostUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FavoritePostUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PostEnterpriseId",
                table: "FavoritePostEnterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FavoritePostEnterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "CommentId",
                table: "FavoriteCommentEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CommentBaseEntityId",
                table: "FavoriteCommentEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "FavoriteCommentEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Enterprises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "IndustryId",
                table: "EnterpriseIndustries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EnterpriseIndustries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EnterpriseIndustries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EnterpriseFollowsUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EnterpriseFollowsUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "EmployeeInvitations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EmployeeInvitations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EmployeeInvitations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PositionId",
                table: "EmployeeEnterprises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EnterpriseId",
                table: "EmployeeEnterprises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EmployeeEnterprises",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "CommentPostUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CommentPostUser",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "PostId",
                table: "CommentPostEnterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CommentPostEnterprise",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentBase",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "ParentCommentId",
                table: "CommentBase",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CommentBase",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Categories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AreaEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "VacancyId",
                table: "ApplicationVacancies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ApplicationVacancies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ApplicationVacancies",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);
        }
    }
}
