namespace JobVacancy.API.models.entities;

public class EnterpriseEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }
    public string? LogoUrl { get; set; }
    public string UserId { get; set; } = string.Empty;
    public virtual UserEntity? User { get; set; }
    public virtual ICollection<EnterpriseIndustryEntity> IndustryLinks { get; set; } = new List<EnterpriseIndustryEntity>();
    
}