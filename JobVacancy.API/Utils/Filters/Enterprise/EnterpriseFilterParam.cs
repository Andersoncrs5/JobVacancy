using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Filters.User;

namespace JobVacancy.API.Utils.Filters.Enterprise;

public class EnterpriseFilterParam: FilterBaseParams
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? WebSiteUrl { get; set; }
    public EnterpriseTypeEnum? Type { get; set; }
    public UserFilterParams? UserFilterParams { get; set; }
}