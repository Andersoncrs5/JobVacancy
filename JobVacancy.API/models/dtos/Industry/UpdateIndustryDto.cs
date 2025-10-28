namespace JobVacancy.API.models.dtos.Industry;

public class UpdateIndustryDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public bool? IsActive { get; set; }
}