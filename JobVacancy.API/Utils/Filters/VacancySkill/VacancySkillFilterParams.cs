using JobVacancy.API.models.dtos;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.VacancySkill;

public class VacancySkillFilterParams: FilterBaseParams
{
    public string? VacancyId { get; set; }
    public string? VacancyTitle { get; set; }
    
    public string? SkillId { get; set; }
    public string? SkillName { get; set; }
    
    public SkillProficiencyLevelEnum? RequiredLevel { get; set; } 
    public bool? IsMandatory { get; set; } 
    public int? WeightMin { get; set; }
    public int? WeightMax { get; set; }
    public int? YearsOfExperienceRequiredMin { get; set; }
    public int? YearsOfExperienceRequiredMax { get; set; }
}