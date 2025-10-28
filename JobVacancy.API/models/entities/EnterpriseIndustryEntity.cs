namespace JobVacancy.API.models.entities;

public class EnterpriseIndustryEntity: BaseEntity
{
    public bool IsPrimary  { get; set; }
    public string EnterpriseId { get; set; }
    
    public virtual EnterpriseEntity Enterprise { get; set; } 
    
    public string IndustryId { get; set; }
    
    public virtual IndustryEntity Industry { get; set; }
}