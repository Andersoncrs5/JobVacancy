using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.entities;

public class EnterpriseIndustryEntity: BaseEntity
{
    public bool IsPrimary  { get; set; }
    [MaxLength(450)] public required string EnterpriseId { get; set; }
    
    public EnterpriseEntity? Enterprise { get; set; } 
    
    [MaxLength(450)] public required string IndustryId { get; set; }
    
    public IndustryEntity? Industry { get; set; }
}