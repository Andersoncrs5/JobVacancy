namespace JobVacancy.API.Utils.Filters.Position;

public class PositionFilterParams: FilterBaseParams
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
}