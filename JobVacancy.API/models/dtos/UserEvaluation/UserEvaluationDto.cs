using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Position;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.UserEvaluation;

public class UserEvaluationDto: ReviewBaseDto
{
    public int? RatingProfessionalism { get; set; } 
    public int? RatingSkillMatch { get; set; } 
    public int? RatingTeamwork { get; set; }
    public int? RecommendationTone { get; set; } 
    
    public EmploymentTypeEnum EmploymentStatus { get; set; } 
    
    public required string EnterpriseId { get; set; }
    public EnterpriseDto? Enterprise { get; set; } 
    
    public required string TargetUserId { get; set; }
    public UserDto? TargetUser { get; set; }
    
    public string? ReviewerUserId { get; set; }
    public UserDto? ReviewerUser { get; set; } 
    
    public string PositionId {get; set;} = string.Empty;
    public PositionDto? Position { get; set; }
}