using JobVacancy.API.models.entities;

namespace JobVacancy.API.models.dtos.FavoritePost;

public class FavoritePostDto
{
    public string UserId { get; set; } = string.Empty;
    public virtual UserEntity? User { get; set; }
    
    public string PostUserId { get; set; } = string.Empty;
    public virtual PostUserEntity? PostUser { get; set; }
    
    public string? UserNotes { get; set; }
    public int? UserRating { get; set; }
}