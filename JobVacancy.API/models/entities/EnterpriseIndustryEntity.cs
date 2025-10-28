namespace JobVacancy.API.models.entities;

public class EnterpriseIndustryEntity: BaseEntity
{
    public string EnterpriseId { get; set; }
    public EnterpriseEntity Enterprise { get; set; }
    
    public string IndustryId { get; set; }
    public IndustryEntity Industry { get; set; }
}