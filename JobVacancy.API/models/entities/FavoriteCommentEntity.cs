using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class FavoriteCommentEntity: BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public UserEntity? User { get; set; }
    
    public string CommentId { get; set; } = string.Empty;
    public CommentBaseEntity? Comment { get; set; }
}