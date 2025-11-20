using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.dtos.CommentPostEnterprise;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.dtos.PostUser;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.UserContentReaction;

public class UserContentReactionDto: BaseDto
{
    public required string UserId { get; set; }
    public UserDto? User { get; set; }
    
    public ReactionTypeEnum ReactionType { get; set; }
    public ReactionTargetEnum TargetType { get; set; }
    
    [MaxLength(450)] public string? PostUserId { get; set; }
    public PostUserDto? PostUser { get; set; } 
    
    public string? PostEnterpriseId { get; set; }
    public PostEnterpriseDto? PostEnterprise { get; set; } 
    
    [MaxLength(450)] public string? CommentUserId { get; set; }
    public CommentPostUserDto? CommentUser { get; set; }

    [MaxLength(450)] public string? CommentEnterpriseId { get; set; }
    public CommentPostEnterpriseDto? CommentEnterprise { get; set; }
}