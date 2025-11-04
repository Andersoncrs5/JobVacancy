namespace JobVacancy.API.models.dtos.CommentPostUser;

public class UpdateCommentPostUserDto
{
    public string? Content { get; set; }
    public bool? IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public int? Depth { get; set; }
}