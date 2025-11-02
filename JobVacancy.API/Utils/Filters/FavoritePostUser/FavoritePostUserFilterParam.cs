namespace JobVacancy.API.Utils.Filters.FavoritePostUser;

public class FavoritePostUserFilterParam: FilterBaseParams
{
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Fullname { get; set; }
    public string? Email { get; set; }
    
    public string? PostUserId { get; set; }
    public string? Title { get; set; }
    public int? ReadingTimeMinutesMin { get; set; }
    public int? ReadingTimeMinutesMax { get; set; }
    
    public string? CategoryId { get; set; }
    
    public string? UserNotes { get; set; }
    public int? UserRatingMin { get; set; }
    public int? UserRatingMax { get; set; }
    
}