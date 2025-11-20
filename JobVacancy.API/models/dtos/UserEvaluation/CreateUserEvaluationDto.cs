using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Annotations.Validations;

namespace JobVacancy.API.models.dtos.UserEvaluation;

public class CreateUserEvaluationDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    [Range(0 , 5)]
    public int RatingOverall { get; set; }
    
    [Range(0 , 5)] public int? RatingCulture { get; set; }
    [Range(0 , 5)] public int? RatingCompensation { get; set; }
    [Range(0 , 5)] public int? RatingManagement { get; set; }
    [Range(0 , 5)] public int? RatingWorkLifeBalance { get; set; }
    
    public bool IsAnonymous { get; set; }
    
    [Range(0 , 5)] public int? RatingProfessionalism { get; set; } 
    [Range(0 , 5)] public int? RatingSkillMatch { get; set; } 
    [Range(0 , 5)] public int? RatingTeamwork { get; set; }
    [Range(0 , 5)] public int? RecommendationTone { get; set; } 
    
    public EmploymentTypeEnum EmploymentStatus { get; set; } 
    
    [ExistsUserById] 
    public required string TargetUserId { get; set; }
    
    [ExistsPosition]
    public string PositionId {get; set;} = string.Empty;
}