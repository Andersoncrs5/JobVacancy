using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class UserSkillEntity: BaseEntity
{
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    [MaxLength(450)] public required string SkillId { get; set; }
    public SkillEntity? Skill { get; set; }

    public int? YearsOfExperience { get; set; }
    public string? ExternalCertificateUrl { get; set; }
    public SkillProficiencyLevelEnum? ProficiencyLevel { get; set; }
}