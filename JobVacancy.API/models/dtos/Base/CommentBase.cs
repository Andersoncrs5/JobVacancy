using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.dtos.Base;

public class CommentBase: BaseDto
{
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public int? Depth { get; set; }
    public string? ParentCommentId { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual UserDto? User { get; set; }
}