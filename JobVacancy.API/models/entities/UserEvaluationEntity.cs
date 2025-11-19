using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Base;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class UserEvaluationEntity: ReviewBase
{
    public int? RatingProfessionalism { get; set; } 
    public int? RatingSkillMatch { get; set; } 
    public int? RatingTeamwork { get; set; }
    public int? RecommendationTone { get; set; } 
    
    public EmploymentTypeEnum EmploymentStatus { get; set; } 
    
    [MaxLength(450)]
    public required string EnterpriseId { get; set; }
    public EnterpriseEntity? Enterprise { get; set; } 
    
    [MaxLength(450)]
    public required string TargetUserId { get; set; }
    public UserEntity? TargetUser { get; set; }
    
    [MaxLength(450)]
    public string? ReviewerUserId { get; set; }
    public UserEntity? ReviewerUser { get; set; } 
    
    [MaxLength(450)]
    public string PositionId {get; set;} = string.Empty;
    public PositionEntity? Position { get; set; }

}