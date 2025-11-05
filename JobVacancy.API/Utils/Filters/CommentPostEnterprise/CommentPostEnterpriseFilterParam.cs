using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Filters.Base;

namespace JobVacancy.API.Utils.Filters.CommentPostEnterprise;

public class CommentPostEnterpriseFilterParam: CommentBaseFilterParams
{
    public string? CategoryId { get; set; }
    
    public string? EnterpriseId { get; set; }
    public string? NameEnterprise { get; set; }
    public EnterpriseTypeEnum? Type  { get; set; }
    
    public string? PostId { get; set; }
    public string? Title { get; set; }
    public string? ContentPost { get; set; }
    public int? ReadingTimeMinutesMin { get; set; }
    public int? ReadingTimeMinutesMax { get; set; }
    
    
}