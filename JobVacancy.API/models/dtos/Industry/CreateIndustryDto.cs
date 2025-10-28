namespace JobVacancy.API.models.dtos.Industry;

public class CreateIndustryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public bool IsActive { get; set; }
}