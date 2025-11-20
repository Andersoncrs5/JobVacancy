using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.UserContentReaction;

public class CreateUserContentReactionDto
{
    public ReactionTypeEnum ReactionType { get; set; }
    public ReactionTargetEnum TargetType { get; set; }
    
    [Required]
    [MaxLength(450)] 
    public required string ContentId { get; set; }
}