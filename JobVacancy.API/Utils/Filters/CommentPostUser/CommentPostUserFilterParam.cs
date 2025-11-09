using JobVacancy.API.Utils.Filters.Base;

namespace JobVacancy.API.Utils.Filters.CommentPostUser;

public class CommentPostUserFilterParam: CommentBaseFilterParams
{
    public string? CategoryId { get; set; }
    public string? PostId { get; set; }
    public string? Title { get; set; }
    public string? ContentPost { get; set; }
    public int? ReadingTimeMinutesMin { get; set; }
    public int? ReadingTimeMinutesMax { get; set; }
}