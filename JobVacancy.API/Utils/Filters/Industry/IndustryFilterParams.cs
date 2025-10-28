namespace JobVacancy.API.Utils.Filters.Industry;

public class IndustryFilterParams: FilterBaseParams
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
}