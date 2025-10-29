using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.EnterpriseIndustry;

public class CreateEnterpriseIndustryDto
{
    [Required] public bool IsPrimary { get; set; } = false;
    [Required] public string EnterpriseId { get; set; } = string.Empty;
    [Required] public string IndustryId { get; set; } = string.Empty;
}