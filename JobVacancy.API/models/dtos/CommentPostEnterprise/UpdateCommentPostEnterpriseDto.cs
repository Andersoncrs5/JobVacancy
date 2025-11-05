namespace JobVacancy.API.models.dtos.CommentPostEnterprise;

public class UpdateCommentPostEnterpriseDto
{
    public string? Content { get; set; }
    public bool? IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public int? Depth { get; set; }
}