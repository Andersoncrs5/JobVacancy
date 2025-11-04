namespace JobVacancy.API.models.dtos.CommentPostUser;

public class CreateCommentPostUserDto
{
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public int? Depth { get; set; }
    public string PostId { get; set; }
}