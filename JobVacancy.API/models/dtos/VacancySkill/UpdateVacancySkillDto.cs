using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.VacancySkill;

public class UpdateVacancySkillDto
{
    public string? SkillId { get; set; }
    public SkillProficiencyLevelEnum? RequiredLevel { get; set; }
    public bool? IsMandatory { get; set; }
    public int? Weight { get; set; }
    public int? YearsOfExperienceRequired { get; set; }
    public string? Notes { get; set; }
}