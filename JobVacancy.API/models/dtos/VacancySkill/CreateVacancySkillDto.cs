using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Annotations.Validations.Skill;
using JobVacancy.API.Utils.Annotations.Validations.Vacancy;

namespace JobVacancy.API.models.dtos.VacancySkill;

public class CreateVacancySkillDto
{
    [Required]
    [ExistsVacancyById]
    public required string VacancyId { get; init; }
    
    [Required]
    [ExistsSkillById]
    public required string SkillId { get; init; }
    
    [Required]
    public SkillProficiencyLevelEnum RequiredLevel { get; init; }
    
    [Required]
    public bool IsMandatory { get; init; }
    
    [Required]
    public int Weight { get; init; }
    
    public int? YearsOfExperienceRequired { get; init; }
    public int? Order { get; set; }
    public string? Notes { get; set; }

}