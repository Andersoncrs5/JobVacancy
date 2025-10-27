namespace JobVacancy.API.models.dtos.Category;

public class UpdateCategoryDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; } = true;
}