using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobVacancy.API.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "app_users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    ImageProfileUrl = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AreaEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
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
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Describe = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PostsBase",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    ReadingTimeMinutes = table.Column<short>(type: "SMALLINT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IconUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "app_role_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_role_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_app_role_claims_app_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "app_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_app_user_claims_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_app_user_logins_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_roles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_app_user_roles_app_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "app_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_app_user_roles_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_tokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_user_tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_app_user_tokens_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentBase",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Content = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Depth = table.Column<short>(type: "SMALLINT", nullable: true),
                    ParentCommentId = table.Column<string>(type: "character varying(450)", nullable: true),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentBase_CommentBase_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "CommentBase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentBase_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enterprises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    WebSiteUrl = table.Column<string>(type: "TEXT", nullable: true),
                    LogoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterprises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enterprises_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowerRelationshipUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    FollowerId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    FollowedId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    WishReceiveNotifyByNewPost = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewComment = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewInteraction = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowerRelationshipUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowerRelationshipUsers_app_users_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowerRelationshipUsers_app_users_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EndorserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EndorsedId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Version = table.Column<short>(type: "SMALLINT", nullable: true),
                    BucketName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ObjectKey = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    userId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resumes_app_users_userId",
                        column: x => x.userId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Recommendation = table.Column<bool>(type: "boolean", nullable: true),
                    ActorId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    TargetUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "PostUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<string>(type: "character varying(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostUsers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUsers_PostsBase_Id",
                        column: x => x.Id,
                        principalTable: "PostsBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUsers_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkill",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    SkillId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: true),
                    ExternalCertificateUrl = table.Column<string>(type: "text", nullable: true),
                    ProficiencyLevel = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkill_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteCommentEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CommentId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CommentBaseEntityId = table.Column<string>(type: "character varying(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteCommentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteCommentEntities_CommentBase_CommentBaseEntityId",
                        column: x => x.CommentBaseEntityId,
                        principalTable: "CommentBase",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteCommentEntities_CommentBase_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CommentBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteCommentEntities_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEnterprises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ContractLink = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SalaryRange = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TerminationReason = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                    Notes = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                    SalaryValue = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentFrequency = table.Column<int>(type: "integer", nullable: false),
                    ContractLegalType = table.Column<int>(type: "integer", nullable: true),
                    EmploymentType = table.Column<int>(type: "integer", nullable: false),
                    EmploymentStatus = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PositionId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    InviteSenderId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEnterprises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_app_users_InviteSenderId",
                        column: x => x.InviteSenderId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEnterprises_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeInvitations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Message = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    RejectReason = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    InvitationLink = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    Token = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SalaryRange = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmploymentType = table.Column<int>(type: "integer", nullable: false),
                    ProposedStartDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    ProposedEndDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    ResponseDate = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false),
                    InviteSenderId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    PositionId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeInvitations_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeInvitations_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeInvitations_app_users_InviteSenderId",
                        column: x => x.InviteSenderId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_EmployeeInvitations_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseFollowsUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    WishReceiveNotifyByNewPost = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewEndorsement = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByProfileUpdate = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewInteraction = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseFollowsUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseFollowsUser_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseFollowsUser_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnterpriseIndustries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    IndustryId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseIndustries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseIndustries_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseIndustries_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowerUserRelationshipEnterprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    WishReceiveNotifyByNewPost = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewVacancy = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewComment = table.Column<bool>(type: "boolean", nullable: false),
                    WishReceiveNotifyByNewInteraction = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowerUserRelationshipEnterprise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowerUserRelationshipEnterprise_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowerUserRelationshipEnterprise_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostEnterprises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", nullable: false),
                    CategoryId = table.Column<string>(type: "character varying(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostEnterprises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostEnterprises_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostEnterprises_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostEnterprises_PostsBase_Id",
                        column: x => x.Id,
                        principalTable: "PostsBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review_enterprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    PositionId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Requirements = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    Responsibilities = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    Benefits = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    EmploymentType = table.Column<int>(type: "integer", nullable: false),
                    ExperienceLevel = table.Column<int>(type: "integer", nullable: true),
                    EducationLevel = table.Column<int>(type: "integer", nullable: true),
                    WorkplaceType = table.Column<int>(type: "integer", nullable: true),
                    Currency = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Seniority = table.Column<short>(type: "SMALLINT", nullable: true),
                    Opening = table.Column<short>(type: "SMALLINT", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "numeric", nullable: true),
                    SalaryMax = table.Column<decimal>(type: "numeric", nullable: true),
                    EnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    AreaId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ApplicationDeadLine = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastApplication = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacancies_AreaEntities_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vacancies_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentPostUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    PostId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPostUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentPostUser_CommentBase_Id",
                        column: x => x.Id,
                        principalTable: "CommentBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentPostUser_PostUsers_PostId",
                        column: x => x.PostId,
                        principalTable: "PostUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoritePostUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    PostUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserNotes = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: true),
                    UserRating = table.Column<short>(type: "SMALLINT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePostUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoritePostUser_PostUsers_PostUserId",
                        column: x => x.PostUserId,
                        principalTable: "PostUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritePostUser_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "CommentPostEnterprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    PostId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentPostEnterprise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentPostEnterprise_CommentBase_Id",
                        column: x => x.Id,
                        principalTable: "CommentBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentPostEnterprise_PostEnterprises_PostId",
                        column: x => x.PostId,
                        principalTable: "PostEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoritePostEnterprise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    PostEnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoritePostEnterprise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoritePostEnterprise_PostEnterprises_PostEnterpriseId",
                        column: x => x.PostEnterpriseId,
                        principalTable: "PostEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoritePostEnterprise_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationVacancies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    VacancyId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LastStatusUpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CoverLetter = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: true),
                    IsViewedByRecruiter = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationVacancies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationVacancies_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationVacancies_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VacancySkills",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    VacancyId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    SkillId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    RequiredLevel = table.Column<int>(type: "integer", nullable: false),
                    IsMandatory = table.Column<bool>(type: "boolean", nullable: false),
                    Weight = table.Column<short>(type: "SMALLINT", nullable: false),
                    YearsOfExperienceRequired = table.Column<short>(type: "SMALLINT", nullable: true),
                    Order = table.Column<short>(type: "SMALLINT", nullable: true),
                    Notes = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacancySkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacancySkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VacancySkills_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContentReaction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ReactionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TargetType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PostUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    PostEnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    CommentUserId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    CommentEnterpriseId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContentReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContentReaction_CommentPostEnterprise_CommentEnterprise~",
                        column: x => x.CommentEnterpriseId,
                        principalTable: "CommentPostEnterprise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_UserContentReaction_CommentPostUser_CommentUserId",
                        column: x => x.CommentUserId,
                        principalTable: "CommentPostUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentReaction_PostEnterprises_PostEnterpriseId",
                        column: x => x.PostEnterpriseId,
                        principalTable: "PostEnterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentReaction_PostUsers_PostUserId",
                        column: x => x.PostUserId,
                        principalTable: "PostUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentReaction_app_users_UserId",
                        column: x => x.UserId,
                        principalTable: "app_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_role_claims_RoleId",
                table: "app_role_claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "app_roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_app_user_claims_UserId",
                table: "app_user_claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_logins_UserId",
                table: "app_user_logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_roles_RoleId",
                table: "app_user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "app_users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "app_users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationVacancies_UserId",
                table: "ApplicationVacancies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationVacancies_VacancyId",
                table: "ApplicationVacancies",
                column: "VacancyId");

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
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentBase_ParentCommentId",
                table: "CommentBase",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentBase_UserId",
                table: "CommentBase",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPostEnterprise_PostId",
                table: "CommentPostEnterprise",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentPostUser_PostId",
                table: "CommentPostUser",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_EnterpriseId",
                table: "EmployeeEnterprises",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_InviteSenderId",
                table: "EmployeeEnterprises",
                column: "InviteSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_PositionId",
                table: "EmployeeEnterprises",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEnterprises_UserId",
                table: "EmployeeEnterprises",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_EnterpriseId",
                table: "EmployeeInvitations",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_InviteSenderId",
                table: "EmployeeInvitations",
                column: "InviteSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_PositionId",
                table: "EmployeeInvitations",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInvitations_UserId",
                table: "EmployeeInvitations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseFollowsUser_EnterpriseId",
                table: "EnterpriseFollowsUser",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseFollowsUser_EnterpriseId_UserId",
                table: "EnterpriseFollowsUser",
                columns: new[] { "EnterpriseId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseFollowsUser_UserId",
                table: "EnterpriseFollowsUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseIndustries_EnterpriseId",
                table: "EnterpriseIndustries",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseIndustries_IndustryId",
                table: "EnterpriseIndustries",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseIndustries_IsPrimary",
                table: "EnterpriseIndustries",
                column: "IsPrimary");

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_Name",
                table: "Enterprises",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_UserId",
                table: "Enterprises",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCommentEntities_CommentBaseEntityId",
                table: "FavoriteCommentEntities",
                column: "CommentBaseEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCommentEntities_CommentId",
                table: "FavoriteCommentEntities",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteCommentEntities_UserId_CommentId",
                table: "FavoriteCommentEntities",
                columns: new[] { "UserId", "CommentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostEnterprise_PostEnterpriseId",
                table: "FavoritePostEnterprise",
                column: "PostEnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostEnterprise_UserId",
                table: "FavoritePostEnterprise",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostEnterprise_UserId_PostEnterpriseId",
                table: "FavoritePostEnterprise",
                columns: new[] { "UserId", "PostEnterpriseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostUser_PostUserId",
                table: "FavoritePostUser",
                column: "PostUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostUser_UserId",
                table: "FavoritePostUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoritePostUser_UserId_PostUserId",
                table: "FavoritePostUser",
                columns: new[] { "UserId", "PostUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowerRelationshipUsers_FollowedId",
                table: "FollowerRelationshipUsers",
                column: "FollowedId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowerRelationshipUsers_FollowerId_FollowedId",
                table: "FollowerRelationshipUsers",
                columns: new[] { "FollowerId", "FollowedId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowerUserRelationshipEnterprise_EnterpriseId",
                table: "FollowerUserRelationshipEnterprise",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowerUserRelationshipEnterprise_UserId",
                table: "FollowerUserRelationshipEnterprise",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowerUserRelationshipEnterprise_UserId_EnterpriseId",
                table: "FollowerUserRelationshipEnterprise",
                columns: new[] { "UserId", "EnterpriseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndicationUser_EndorsedId",
                table: "IndicationUser",
                column: "EndorsedId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicationUser_EndorserId_EndorsedId",
                table: "IndicationUser",
                columns: new[] { "EndorserId", "EndorsedId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Industries_Name",
                table: "Industries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_IsActive",
                table: "Positions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Name",
                table: "Positions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostEnterprises_CategoryId",
                table: "PostEnterprises",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostEnterprises_EnterpriseId",
                table: "PostEnterprises",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUserMetrics_PostId",
                table: "PostUserMetrics",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostUsers_CategoryId",
                table: "PostUsers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUsers_UserId",
                table: "PostUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ObjectKey",
                table: "Resumes",
                column: "ObjectKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_userId",
                table: "Resumes",
                column: "userId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUser_ActorId_TargetUserId",
                table: "ReviewUser",
                columns: new[] { "ActorId", "TargetUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUser_TargetUserId",
                table: "ReviewUser",
                column: "TargetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_IsActive",
                table: "Skills",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                table: "Skills",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserContentReaction_CommentEnterpriseId",
                table: "UserContentReaction",
                column: "CommentEnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentReaction_CommentUserId",
                table: "UserContentReaction",
                column: "CommentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentReaction_PostEnterpriseId",
                table: "UserContentReaction",
                column: "PostEnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentReaction_PostUserId",
                table: "UserContentReaction",
                column: "PostUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentReaction_UserId_TargetType",
                table: "UserContentReaction",
                columns: new[] { "UserId", "TargetType" });

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

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkill",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId_SkillId",
                table: "UserSkill",
                columns: new[] { "UserId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_AreaId",
                table: "Vacancies",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_EnterpriseId",
                table: "Vacancies",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancySkills_SkillId",
                table: "VacancySkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_VacancySkills_VacancyId_SkillId",
                table: "VacancySkills",
                columns: new[] { "VacancyId", "SkillId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_role_claims");

            migrationBuilder.DropTable(
                name: "app_user_claims");

            migrationBuilder.DropTable(
                name: "app_user_logins");

            migrationBuilder.DropTable(
                name: "app_user_roles");

            migrationBuilder.DropTable(
                name: "app_user_tokens");

            migrationBuilder.DropTable(
                name: "ApplicationVacancies");

            migrationBuilder.DropTable(
                name: "EmployeeEnterprises");

            migrationBuilder.DropTable(
                name: "EmployeeInvitations");

            migrationBuilder.DropTable(
                name: "EnterpriseFollowsUser");

            migrationBuilder.DropTable(
                name: "EnterpriseIndustries");

            migrationBuilder.DropTable(
                name: "FavoriteCommentEntities");

            migrationBuilder.DropTable(
                name: "FavoritePostEnterprise");

            migrationBuilder.DropTable(
                name: "FavoritePostUser");

            migrationBuilder.DropTable(
                name: "FollowerRelationshipUsers");

            migrationBuilder.DropTable(
                name: "FollowerUserRelationshipEnterprise");

            migrationBuilder.DropTable(
                name: "IndicationUser");

            migrationBuilder.DropTable(
                name: "PostUserMetrics");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "review_enterprise");

            migrationBuilder.DropTable(
                name: "ReviewUser");

            migrationBuilder.DropTable(
                name: "UserContentReaction");

            migrationBuilder.DropTable(
                name: "UserEvaluations");

            migrationBuilder.DropTable(
                name: "UserSkill");

            migrationBuilder.DropTable(
                name: "VacancySkills");

            migrationBuilder.DropTable(
                name: "app_roles");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropTable(
                name: "CommentPostEnterprise");

            migrationBuilder.DropTable(
                name: "CommentPostUser");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Vacancies");

            migrationBuilder.DropTable(
                name: "PostEnterprises");

            migrationBuilder.DropTable(
                name: "CommentBase");

            migrationBuilder.DropTable(
                name: "PostUsers");

            migrationBuilder.DropTable(
                name: "AreaEntities");

            migrationBuilder.DropTable(
                name: "Enterprises");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "PostsBase");

            migrationBuilder.DropTable(
                name: "app_users");
        }
    }
}
