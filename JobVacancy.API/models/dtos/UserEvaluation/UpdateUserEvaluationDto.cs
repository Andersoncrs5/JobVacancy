using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Annotations.Validations;

namespace JobVacancy.API.models.dtos.UserEvaluation;

public class UpdateUserEvaluationDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    
    [Range(0 , 5)] public int? RatingOverall { get; set; }
    [Range(0 , 5)] public int? RatingCulture { get; set; }
    [Range(0 , 5)] public int? RatingCompensation { get; set; }
    [Range(0 , 5)] public int? RatingManagement { get; set; }
    [Range(0 , 5)] public int? RatingWorkLifeBalance { get; set; }
    
    public bool? IsAnonymous { get; set; }
    
    [Range(0 , 5)] public int? RatingProfessionalism { get; set; } 
    [Range(0 , 5)] public int? RatingSkillMatch { get; set; } 
    [Range(0 , 5)] public int? RatingTeamwork { get; set; }
    [Range(0 , 5)] public int? RecommendationTone { get; set; } 
    
    public EmploymentTypeEnum? EmploymentStatus { get; set; } 
    
    public string? PositionId {get; set;}
}