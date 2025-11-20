using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class VacancySkillEntity: BaseEntity
{
    [MaxLength(450)] public required string VacancyId { get; set; }
    public VacancyEntity? Vacancy { get; set; }
    
    [MaxLength(450)] public required string SkillId { get; set; }
    public SkillEntity? Skill { get; set; }
    
    public SkillProficiencyLevelEnum RequiredLevel { get; set; } 

    public bool IsMandatory { get; set; } 
    
    public int Weight { get; set; } = 1; 
    public int? YearsOfExperienceRequired { get; set; }
    public int? Order { get; set; }
    
    public string? Notes { get; set; }
}