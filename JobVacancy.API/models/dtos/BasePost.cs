using JobVacancy.API.models.dtos.Category;

namespace JobVacancy.API.models.dtos;

public class BasePost: BaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public int? ReadingTimeMinutes { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public CategoryDto? Category { get; set; }
}