namespace JobVacancy.API.models.dtos.PostEnterprise;

public class UpdatePostEnterpriseDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
    public int? ReadingTimeMinutes { get; set; }
}