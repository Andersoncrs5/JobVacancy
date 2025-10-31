namespace JobVacancy.API.Utils.Filters;

public class BasePostParams: FilterBaseParams
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsFeatured { get; set; }
    
    public int? ReadingTimeMinutesBefore { get; set; }
    public int? ReadingTimeMinutesAfter { get; set; }
    
    public string? CategoryId { get; set; }
    public string NameCategory { get; set; } = string.Empty;
}