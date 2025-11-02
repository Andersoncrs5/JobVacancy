namespace JobVacancy.API.models.entities;

public class FavoritePostUserEntity: BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual UserEntity? User { get; set; }
    
    public string PostUserId { get; set; } = string.Empty;
    public virtual PostUserEntity? PostUser { get; set; }
    
    public string? UserNotes { get; set; }
    public int? UserRating { get; set; }
    
}