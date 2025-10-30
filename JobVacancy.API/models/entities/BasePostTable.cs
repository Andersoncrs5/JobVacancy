namespace JobVacancy.API.models.entities;

public abstract class BasePostTable: BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsFeatured { get; set; } = false;
    public int? ReadingTimeMinutes { get; set; }
    public string CategoryId { get; set; } = string.Empty;
}