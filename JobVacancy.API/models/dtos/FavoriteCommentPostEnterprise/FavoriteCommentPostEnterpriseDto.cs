using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.FavoriteCommentPostEnterprise;

public class FavoriteCommentPostEnterpriseDto: BaseDto
{
    public string UserId { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    
    public string CommentId { get; set; } = string.Empty;
    public CommentBase? Comment { get; set; }
}