using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class UserSkillEntity: BaseEntity
{
    public string UserId { get; set; }
    public virtual UserEntity User { get; set; }
    
    public string SkillId { get; set; }
    public virtual SkillEntity Skill { get; set; }

    public int? YearsOfExperience { get; set; }
    public string? ExternalCertificateUrl { get; set; }
    public SkillProficiencyLevelEnum? ProficiencyLevel { get; set; }
}