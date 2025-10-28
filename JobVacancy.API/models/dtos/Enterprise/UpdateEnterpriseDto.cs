using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.Enterprise;

public class UpdateEnterpriseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }
    public string? LogoUrl { get; set; }
    public EnterpriseTypeEnum? Type { get; set; }
}