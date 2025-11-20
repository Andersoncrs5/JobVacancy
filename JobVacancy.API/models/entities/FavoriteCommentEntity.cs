using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class FavoriteCommentEntity: BaseEntity
{
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    [MaxLength(450)] public required string CommentId { get; set; }
    public CommentBaseEntity? Comment { get; set; }
}