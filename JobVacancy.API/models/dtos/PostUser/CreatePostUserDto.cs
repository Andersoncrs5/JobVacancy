namespace JobVacancy.API.models.dtos.PostUser;

public class CreatePostUserDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int? ReadingTimeMinutes { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    
}