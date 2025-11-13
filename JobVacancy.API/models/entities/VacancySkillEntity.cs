using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class VacancySkillEntity: BaseEntity
{
    public string VacancyId { get; set; }
    public VacancyEntity? Vacancy { get; set; }
    
    public string SkillId { get; set; }
    public SkillEntity? Skill { get; set; }
    
    public SkillProficiencyLevelEnum RequiredLevel { get; set; } 

    public bool IsMandatory { get; set; } 
    
    public int Weight { get; set; } = 1; 
    public int? YearsOfExperienceRequired { get; set; }
    
    public string? Notes { get; set; }
}