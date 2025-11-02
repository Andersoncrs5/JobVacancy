using JobVacancy.API.models.dtos;

namespace JobVacancy.API.Utils.Filters.UserSkill;

public class UserSkillFilterParam: FilterBaseParams
{
    public string? SkillId { get; set; }
    public string? NameSkill { get; set; }
    
    public int? YearsOfExperience { get; set; }
    public string? ExternalCertificateUrl { get; set; }
    
    public string? FullNameUser { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? UserId { get; set; }
    
}