namespace JobVacancy.API.models.dtos.Category;

public class CategoryDto: BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}