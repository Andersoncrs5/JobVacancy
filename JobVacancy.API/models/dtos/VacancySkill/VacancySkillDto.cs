using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.dtos.Vacancy;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.VacancySkill;

public class VacancySkillDto: BaseDto
{
    public required string VacancyId { get; set; }
    public VacancyDto? Vacancy { get; set; }
    
    public required string SkillId { get; set; }
    public SkillDto? Skill { get; set; }
    
    public SkillProficiencyLevelEnum RequiredLevel { get; set; } 
    
    public bool IsMandatory { get; set; } 
    
    public int Weight { get; set; } 
    public int? YearsOfExperienceRequired { get; set; }
    
    public string? Notes { get; set; }
}