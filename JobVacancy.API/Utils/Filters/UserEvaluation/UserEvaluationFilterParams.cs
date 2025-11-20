using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.UserEvaluation;

public class UserEvaluationFilterParams: FilterBaseParams
{
    public string? Title { get; set; } 
    public string? Content { get; set; } 
    
    public int? RatingOverallMin { get; set; }
    public int? RatingOverallMax { get; set; }
    
    public int? RatingCultureMin { get; set; }
    public int? RatingCultureMax { get; set; }
    
    public int? RatingCompensationMin { get; set; }
    public int? RatingCompensationMax { get; set; }
    
    public int? RatingManagementMin { get; set; }
    public int? RatingManagementMax { get; set; }
    
    public int? RatingWorkLifeBalanceMin { get; set; }
    public int? RatingWorkLifeBalanceMax { get; set; }
    
    public int? RatingProfessionalismMin { get; set; } 
    public int? RatingProfessionalismMax { get; set; } 
    
    public int? RatingSkillMatchMin { get; set; } 
    public int? RatingSkillMatchMax { get; set; } 
    
    public int? RatingTeamworkMin { get; set; }
    public int? RatingTeamworkMax { get; set; }
    
    public int? RecommendationToneMin { get; set; } 
    public int? RecommendationToneMax { get; set; } 
    
    public bool? IsAnonymous { get; set; }
    
    public EmploymentTypeEnum? EmploymentStatus { get; set; } 
    
    public string? EnterpriseId { get; set; }
    public string? EnterpriseName { get; set; }
    public EnterpriseTypeEnum? EnterpriseType { get; set; }
    
    public string? TargetUserId { get; set; }
    public string? TargetUserFullName { get; set; }
    public string? TargetUserName { get; set; }
    public string? TargetUserEmail { get; set; }

    public string? ReviewerUserId { get; set; }
    public string? ReviewerUserFullName { get; set; }
    public string? ReviewerUserName { get; set; }
    public string? ReviewerUserEmail { get; set; }

    public string? PositionId { get; set; }
    public string? PositionName { get; set; }
    
    public bool? LoadEnterprise { get; set; }
    public bool? LoadPosition { get; set; }
    public bool? LoadTargetUser { get; set; }
    public bool? LoadReviewerUser { get; set; }
}