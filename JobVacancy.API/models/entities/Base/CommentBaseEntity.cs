namespace JobVacancy.API.models.entities.Base;

public class CommentBaseEntity: BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public int? Depth { get; set; }
    public string? ParentCommentId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public virtual UserEntity? User { get; set; }
    public virtual CommentBaseEntity? ParentComment { get; set; }
    public virtual ICollection<CommentBaseEntity> Replies { get; set; } = new List<CommentBaseEntity>();
    public virtual ICollection<FavoriteCommentEntity> FavoriteCommentEntities { get; set; } = new List<FavoriteCommentEntity>();
}