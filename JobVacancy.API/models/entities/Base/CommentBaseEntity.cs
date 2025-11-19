using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.entities.Base;

public class CommentBaseEntity: BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public int? Depth { get; set; }
    public string? ParentCommentId { get; set; }

    [MaxLength(450)] public required string UserId { get; set; } 

    public UserEntity? User { get; set; }
    public CommentBaseEntity? ParentComment { get; set; }
    public ICollection<CommentBaseEntity> Replies { get; set; } = new List<CommentBaseEntity>();
    public ICollection<FavoriteCommentEntity> FavoriteCommentEntities { get; set; } = new List<FavoriteCommentEntity>();
}