using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.EnterpriseIndustry;

public class UpdateEnterpriseIndustryDto
{
    public bool IsPrimary { get; set; } = false;
    [Required] public string IndustryId { get; set; } = string.Empty;
}