namespace JobVacancy.API.models.entities;

public class IndustryEntity: BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; }
    public ICollection<EnterpriseIndustryEntity>? EnterpriseLinks { get; set; }

}