using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.PostEnterprise;

namespace JobVacancy.API.models.dtos.CommentPostEnterprise;

public class CommentPostEnterpriseDto: CommentBase
{
    public string PostId { get; set; } = string.Empty;
    public PostEnterpriseDto? Post { get; set; }
}