namespace JobVacancy.API.Utils.Filters.FavoritePostEnterprise;

public class FavoritePostEnterpriseFilterParam: FilterBaseParams
{
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Fullname { get; set; }
    public string? Email { get; set; }
    
    public string? PostId { get; set; }
    public string? Title { get; set; }
    public int? ReadingTimeMinutesMin { get; set; }
    public int? ReadingTimeMinutesMax { get; set; }
    
    public string? CategoryId { get; set; }
}