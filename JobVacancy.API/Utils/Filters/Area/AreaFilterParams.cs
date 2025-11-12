namespace JobVacancy.API.Utils.Filters.Area;

public class AreaFilterParams: FilterBaseParams
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}