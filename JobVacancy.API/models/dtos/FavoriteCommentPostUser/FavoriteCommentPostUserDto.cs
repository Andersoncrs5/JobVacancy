using JobVacancy.API.models.dtos.Base;
using JobVacancy.API.models.dtos.CommentPostUser;
using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.dtos.FavoriteCommentPostUser;

public class FavoriteCommentPostUserDto: BaseDto
{
    public string UserId { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    
    public string CommentId { get; set; } = string.Empty;
    public CommentBase? Comment { get; set; }
}