using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.UserSkill;

public class UpdateUserSkillDto
{
    public int? YearsOfExperience { get; set; }
    public string? ExternalCertificateUrl { get; set; }
    public SkillProficiencyLevelEnum? ProficiencyLevel { get; set; }
}