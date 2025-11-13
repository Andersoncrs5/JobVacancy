using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Annotations.Validations.Skill;
using JobVacancy.API.Utils.Annotations.Validations.Vacancy;

namespace JobVacancy.API.models.dtos.VacancySkill;

public class CreateVacancySkillDto
{
    [Required]
    [ExistsVacancyById]
    public required string VacancyId { get; set; }
    
    [Required]
    [ExistsSkillById]
    public required string SkillId { get; set; }
    
    [Required]
    public SkillProficiencyLevelEnum RequiredLevel { get; set; }
    
    [Required]
    public bool IsMandatory { get; set; }
    
    [Required]
    public int Weight { get; set; }
    
    public int? YearsOfExperienceRequired { get; set; }
    public string? Notes { get; set; }

}