using JobVacancy.API.Utils.Filters.Enterprise;
using JobVacancy.API.Utils.Filters.Industry;

namespace JobVacancy.API.Utils.Filters.EnterpriseIndustry;

public class EnterpriseIndustryFilterParams: FilterBaseParams
{
    public bool? IsPrimary { get; set; }
    public string? EnterpriseId { get; set; }
    public string? IndustryId { get; set; }
    public EnterpriseFilterParam? EnterpriseFilterParam { get; set; }
    public IndustryFilterParams? IndustryFilterParams { get; set; }
}