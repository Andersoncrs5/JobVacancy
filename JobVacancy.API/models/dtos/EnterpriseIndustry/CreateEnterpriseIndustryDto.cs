using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.EnterpriseIndustry;

public class CreateEnterpriseIndustryDto
{
    [Required] public bool IsPrimary { get; set; }
    [Required] public string EnterpriseId { get; set; }
    [Required] public string IndustryId { get; set; }
}