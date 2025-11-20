using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class UserContentReactionEntity: BaseEntity
{
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public ReactionTypeEnum ReactionType { get; set; }
    public ReactionTargetEnum TargetType { get; set; }

    [MaxLength(450)] public string? PostUserId { get; set; }
    public PostUserEntity? PostUser { get; set; } 

    [MaxLength(450)]
    public string? PostEnterpriseId { get; set; }
    public PostEnterpriseEntity? PostEnterprise { get; set; } 

    [MaxLength(450)] public string? CommentUserId { get; set; }
    public CommentPostUserEntity? CommentUser { get; set; }

    [MaxLength(450)] public string? CommentEnterpriseId { get; set; }
    public CommentPostEnterpriseEntity? CommentEnterprise { get; set; }
}