using JobVacancy.API.Utils.Page;

namespace JobVacancy.API.Utils.Filters;

public class FilterBaseParams: PagebleParams
{
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
    public string? Id { get; set; }
}