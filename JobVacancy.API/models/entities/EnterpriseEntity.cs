using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class EnterpriseEntity: BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }
    public string? LogoUrl { get; set; }
    public EnterpriseTypeEnum Type { get; set; }
    public string UserId { get; set; }
    public virtual UserEntity? User { get; set; }
    public virtual ICollection<EnterpriseIndustryEntity>? IndustryLinks { get; set; }
}