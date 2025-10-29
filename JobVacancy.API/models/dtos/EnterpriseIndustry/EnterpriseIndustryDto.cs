using JobVacancy.API.models.dtos.Enterprise;
using JobVacancy.API.models.dtos.Industry;

namespace JobVacancy.API.models.dtos.EnterpriseIndustry;

public class EnterpriseIndustryDto: BaseDto
{
    public bool IsPrimary  { get; set; }
    public string? EnterpriseId { get; set; }
    public string? IndustryId { get; set; }
    public virtual EnterpriseDto Enterprise { get; set; } 
    public virtual IndustryDto Industry { get; set; }
}