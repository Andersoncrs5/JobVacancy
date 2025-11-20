using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class EnterpriseEntity: BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }
    public string? LogoUrl { get; set; }
    public EnterpriseTypeEnum Type { get; set; }
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    public ICollection<EnterpriseIndustryEntity>? IndustryLinks { get; set; }
    public ICollection<PostEnterpriseEntity>? Posts { get; set; }
    public ICollection<EmployeeInvitationEntity>? EmployeeInvitations { get; set; }
    public ICollection<EmployeeEnterpriseEntity>? EmployeeEnterprise { get; set; }
    public ICollection<ReviewEnterpriseEntity>? Reviews { get; set; }
    public ICollection<VacancyEntity>? Vacancies { get; set; }
    public ICollection<FollowerUserRelationshipEnterpriseEntity>? FollowedEnterprise { get; init; }
    public ICollection<EnterpriseFollowsUserEntity>? UsersFollowing { get; init; }
    public ICollection<UserEvaluationEntity>? EvaluationsSent { get; init; }
}