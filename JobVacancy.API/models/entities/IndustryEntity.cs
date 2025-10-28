namespace JobVacancy.API.models.entities;

public class IndustryEntity: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; }
    public virtual ICollection<EnterpriseIndustryEntity> EnterpriseLinks { get; set; } = new List<EnterpriseIndustryEntity>();

}