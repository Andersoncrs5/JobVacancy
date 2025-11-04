namespace JobVacancy.API.Utils.Filters.Base;

public class CommentBaseFilterParams: FilterBaseParams
{
    public string? Content { get; set; }
    public bool? IsActive { get; set; }
    public int? DepthMin { get; set; }
    public int? DepthMax { get; set; }
    public string? ParentCommentId { get; set; }

    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Fullname { get; set; }
}