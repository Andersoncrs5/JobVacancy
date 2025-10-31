namespace JobVacancy.API.Utils.Filters.Category;

public class CategoryFilterParams: FilterBaseParams
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}