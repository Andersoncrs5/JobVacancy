using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class CommentPostUserEntity: CommentBaseEntity
{
    [MaxLength(450)] public required string PostId { get; set; }
    public PostUserEntity? Post { get; set; }
    public ICollection<UserContentReactionEntity>? Reactions { get; set; }
}