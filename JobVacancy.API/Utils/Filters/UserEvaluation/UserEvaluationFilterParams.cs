using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.UserEvaluation;

public class UserEvaluationFilterParams
{
    public string? Title { get; set; } 
    public string? Content { get; set; } 
    public int? RatingOverall { get; set; }
    
    public int? RatingCulture { get; set; }
    public int? RatingCompensation { get; set; }
    public int? RatingManagement { get; set; }
    public int? RatingWorkLifeBalance { get; set; }
    public int? RatingProfessionalism { get; set; } 
    public int? RatingSkillMatch { get; set; } 
    public int? RatingTeamwork { get; set; }
    public int? RecommendationTone { get; set; } 
    
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