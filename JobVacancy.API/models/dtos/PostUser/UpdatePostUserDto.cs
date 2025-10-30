namespace JobVacancy.API.models.dtos.PostUser;

public class UpdatePostUserDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsFeatured { get; set; }
    public int? ReadingTimeMinutes { get; set; }
}