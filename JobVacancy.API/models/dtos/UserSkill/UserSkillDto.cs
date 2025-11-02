using JobVacancy.API.models.dtos.Skill;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.UserSkill;

public class UserSkillDto: BaseDto
{
    public string UserId { get; set; }
    public virtual UserDto User { get; set; }
    
    public string SkillId { get; set; }
    public virtual SkillDto Skill { get; set; }

    public int? YearsOfExperience { get; set; }
    public string? ExternalCertificateUrl { get; set; }
    public SkillProficiencyLevelEnum? ProficiencyLevel { get; set; }
}