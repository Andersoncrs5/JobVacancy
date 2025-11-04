using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class FavoriteCommentEntity: BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual UserEntity? User { get; set; }
    
    public string CommentId { get; set; } = string.Empty;
    public virtual CommentBaseEntity? Comment { get; set; }
}