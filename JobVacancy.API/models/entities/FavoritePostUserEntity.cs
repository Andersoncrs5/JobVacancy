using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.entities;

public class FavoritePostUserEntity: BaseEntity
{
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    [MaxLength(450)] public required string PostUserId { get; set; }
    public PostUserEntity? PostUser { get; set; }
    
    public string? UserNotes { get; set; }
    public int? UserRating { get; set; }
    
}